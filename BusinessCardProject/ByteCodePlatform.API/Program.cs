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

if (!string.IsNullOrEmpty(certPath) && !string.IsNullOrEmpty(keyPath))
{
    var certFile = Path.Combine(AppContext.BaseDirectory, certPath);
    var keyFile = Path.Combine(AppContext.BaseDirectory, keyPath);
    var combinedPem = File.ReadAllText(certFile) + File.ReadAllText(keyFile);
    var certificate = X509Certificate2.CreateFromPem(combinedPem, combinedPem);

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
                builder.Configuration["GrpcServices:ClientUrl"] ?? throw new Exception("Grpc services url is missing"),
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

var rootCaPath = builder.Configuration["RootCaCertificate:Path"];
var clientCertPath = builder.Configuration["GrpcClientCertificate:Path"];
var clientCertKeyPath = builder.Configuration["GrpcClientCertificate:KeyPath"];

builder.Services.AddGrpcClient<AuthorizationService.Proto.AuthorizationService.AuthorizationServiceClient>(options =>
    {
        options.Address = new Uri(builder.Configuration["GrpcServices:AuthServiceUrl"]
                                  ?? throw new Exception("Grpc services url is missing"));
    })
    .ConfigurePrimaryHttpMessageHandler(sp =>
    {
        var tokenHandler = sp.GetRequiredService<AuthTokenPropagationHandler>();
        
        var rootCaFile = Path.Combine(AppContext.BaseDirectory, rootCaPath);
        var rootCaBytes = File.ReadAllBytes(rootCaFile);
        var rootCert = new X509Certificate2(rootCaBytes);
        
        var sslOptions = new SslClientAuthenticationOptions
        {
            ClientCertificates = LoadCertificates(clientCertPath, clientCertKeyPath)
        };

        sslOptions.RemoteCertificateValidationCallback = ValidateRemoteCertificate;

        var handler = new SocketsHttpHandler
        {
            EnableMultipleHttp2Connections = true,
            SslOptions = sslOptions
        };
        
        tokenHandler.InnerHandler = handler;
        return tokenHandler;
    });

builder.Services.AddGrpc();
builder.Services.AddCollectionInfrastructure(builder.Configuration)
    .AddApplication();

var publicKeyPath = Path.Combine(AppContext.BaseDirectory, "certs", "public_key.pem");
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
return;

static X509Certificate2Collection LoadCertificates(string certPath, string keyPath)
{
    var certFile = Path.Combine(AppContext.BaseDirectory, certPath);
    var keyFile = Path.Combine(AppContext.BaseDirectory, keyPath);
    var combinedPem = File.ReadAllText(certFile) + File.ReadAllText(keyFile);
    
    var cert = X509Certificate2.CreateFromPem(combinedPem, combinedPem);
    return new X509Certificate2Collection(cert);
}

bool ValidateRemoteCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
{
    if (errors == SslPolicyErrors.None)
        return true;
    
    var rootCaFile = Path.Combine(AppContext.BaseDirectory, rootCaPath);
    var rootCaBytes = File.ReadAllBytes(rootCaFile);
    var rootCert = new X509Certificate2(rootCaBytes);
    
    var chain2 = new X509Chain();
    chain2.ChainPolicy.TrustMode = X509ChainTrustMode.CustomRootTrust;
    chain2.ChainPolicy.CustomTrustStore.Add(rootCert);
    chain2.ChainPolicy.VerificationFlags = X509VerificationFlags.NoFlag;
    
    return chain2.Build((X509Certificate2)certificate);
}