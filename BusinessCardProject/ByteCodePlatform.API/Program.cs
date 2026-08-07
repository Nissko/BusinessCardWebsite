using System.Net.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using ByteCodePlatform.Application.Application.Extensions;
using ByteCodePlatform.Infrastructure.Extensions;
using ByteCodePlatform.Infrastructure.Extensions.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.IdentityModel.Tokens;
using CourseGrpcService = ByteCodePlatform.API.Services.CourseGrpcService;
using UserGrpcService = ByteCodePlatform.API.Services.UserGrpcService;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IAuthTokenAccessor, GrpcAuthTokenAccessor>();
builder.Services.AddScoped<AuthTokenPropagationHandler>();

var port = int.Parse(builder.Configuration["GrpcServices:ListenPort"] ?? throw new Exception("Port is missing"));
var certPath = builder.Configuration["GrpcClientCertificate:Path"];
var keyPath = builder.Configuration["GrpcClientCertificate:KeyPath"];
var rootCaPath = builder.Configuration["RootCaCertificate:Path"];

if (!string.IsNullOrEmpty(certPath) && !string.IsNullOrEmpty(keyPath))
{
    var certFile = Path.Combine(AppContext.BaseDirectory, certPath);
    var keyFile = Path.Combine(AppContext.BaseDirectory, keyPath);
    
    var certPem = File.ReadAllText(certFile);
    var keyPem = File.ReadAllText(keyFile);
    
    var certificate = X509Certificate2.CreateFromPem(certPem, keyPem);
    certificate = new X509Certificate2(certificate.Export(X509ContentType.Pfx));

    builder.WebHost.ConfigureKestrel(options =>
    {
        options.ListenAnyIP(port, listenOptions =>
        {
            listenOptions.Protocols = HttpProtocols.Http1AndHttp2;
            if (builder.Environment.IsDevelopment())
            {
                listenOptions.UseHttps();
            }
            else
            {
                listenOptions.UseHttps(certificate);
            }
        });
    });
}
else
{
    builder.WebHost.ConfigureKestrel(options =>
    {
        options.ListenAnyIP(port, listenOptions =>
        {
            listenOptions.Protocols = HttpProtocols.Http1AndHttp2;
            if (builder.Environment.IsDevelopment())
            {
                listenOptions.UseHttps();
            }
        });
    });
}

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorClient", policy =>
    {
        policy
            .WithOrigins(
                builder.Configuration["GrpcServices:ClientUrl"] ?? throw new Exception("Grpc client url is missing"),
                builder.Configuration["GrpcAdminService:UrlToAdminPanel"] ?? throw new Exception("Grpc admin service url is missing")
            )
            .AllowAnyMethod()
            .AllowAnyHeader()
            .WithExposedHeaders(
                "Grpc-Status",
                "Grpc-Message",
                "Grpc-Encoding",
                "Grpc-Accept-Encoding",
                "Content-Type"
            );
    });
});

var authServiceUrl = builder.Configuration["GrpcServices:AuthServiceUrl"] ?? throw new Exception("Grpc AuthService url is missing");

builder.Services.AddGrpcClient<AuthorizationService.Proto.AuthorizationService.AuthorizationServiceClient>(options =>
{
    options.Address = new Uri(authServiceUrl);
})
.ConfigurePrimaryHttpMessageHandler(sp =>
{
    var tokenHandler = sp.GetRequiredService<AuthTokenPropagationHandler>();
    
    var sslOptions = new SslClientAuthenticationOptions
    {
        TargetHost = new Uri(authServiceUrl).Host 
    };

    if (!string.IsNullOrEmpty(certPath) && !string.IsNullOrEmpty(keyPath))
    {
        sslOptions.ClientCertificates = LoadCertificates(certPath, keyPath);
    }

    if (!string.IsNullOrEmpty(rootCaPath))
    {
        sslOptions.RemoteCertificateValidationCallback = ValidateRemoteCertificate;
    }

    var handler = new SocketsHttpHandler
    {
        EnableMultipleHttp2Connections = true,
        SslOptions = sslOptions
    };

    tokenHandler.InnerHandler = handler;
    return tokenHandler;
});

bool ValidateRemoteCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
{
    if (errors == SslPolicyErrors.None)
        return true;

    if (string.IsNullOrEmpty(rootCaPath)) return false;

    var rootCaFile = Path.Combine(AppContext.BaseDirectory, rootCaPath);
    var rootCaBytes = File.ReadAllBytes(rootCaFile);
    var rootCert = new X509Certificate2(rootCaBytes);

    var chain2 = new X509Chain();
    chain2.ChainPolicy.TrustMode = X509ChainTrustMode.CustomRootTrust;
    chain2.ChainPolicy.CustomTrustStore.Add(rootCert);

    chain2.ChainPolicy.VerificationFlags = 
        X509VerificationFlags.IgnoreEndRevocationUnknown |
        X509VerificationFlags.IgnoreCertificateAuthorityRevocationUnknown |
        X509VerificationFlags.IgnoreRootRevocationUnknown;

    if (chain != null)
    {
        foreach (var element in chain.ChainElements)
        {
            chain2.ChainPolicy.ExtraStore.Add(element.Certificate);
        }
    }

    return chain2.Build((X509Certificate2)certificate);
}

static X509Certificate2Collection LoadCertificates(string certPath, string keyPath)
{
    var certFile = Path.Combine(AppContext.BaseDirectory, certPath);
    var keyFile = Path.Combine(AppContext.BaseDirectory, keyPath);
    
    var certPem = File.ReadAllText(certFile);
    var keyPem = File.ReadAllText(keyFile);
    
    var cert = X509Certificate2.CreateFromPem(certPem, keyPem);
    var certWithKey = new X509Certificate2(cert.Export(X509ContentType.Pfx));
    
    return new X509Certificate2Collection(certWithKey);
}

builder.Services.AddGrpc();
builder.Services.AddCollectionInfrastructure(builder.Configuration)
    .AddApplication();

var publicKeyPath = Path.Combine(AppContext.BaseDirectory, "certs", "public_key.pem");
if (File.Exists(publicKeyPath))
{
    var pemContent = File.ReadAllText(publicKeyPath);
    var rsa = RSA.Create();
    rsa.ImportFromPem(pemContent);

    System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = "https://bytecode.splinterkeenetic.netcraze.club/auth",
                ValidateAudience = true,
                ValidAudience = "grpc-services",
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
                IssuerSigningKey = new RsaSecurityKey(rsa),
                ValidAlgorithms = ["RS256"],
                RoleClaimType = "role"
            };
        });
}
else
{
    Console.WriteLine($"[Warning] Public key not found at {publicKeyPath}. JWT Authentication might fail.");
}

builder.Services.AddAuthorization();

var app = builder.Build();

app.UseRouting();
app.UseCors("AllowBlazorClient");
app.UseGrpcWeb();
app.UseAuthentication();
app.UseAuthorization();

if (builder.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.MapGrpcService<UserGrpcService>().EnableGrpcWeb();
app.MapGrpcService<CourseGrpcService>().EnableGrpcWeb();

app.Run();