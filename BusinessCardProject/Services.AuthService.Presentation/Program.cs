using System.Net.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.IdentityModel.Tokens;
using Services.AuthService.Application.Application.Extensions;
using Services.AuthService.Infrastructure.Extensions;
using Services.AuthService.Infrastructure.Extensions.Interfaces;
using Services.AuthService.Presentation.Services;
using UserService.Proto;

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

HttpMessageHandler ConfigureGrpcClientHandler(IServiceProvider sp)
{
    var tokenHandler = sp.GetRequiredService<AuthTokenPropagationHandler>();
    
    var sslOptions = new SslClientAuthenticationOptions();

    if (!string.IsNullOrEmpty(clientCertPath) && !string.IsNullOrEmpty(clientCertKeyPath))
    {
        sslOptions.ClientCertificates = LoadCertificates(clientCertPath, clientCertKeyPath);
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
}

builder.Services.AddGrpcClient<UserGrpcService.UserGrpcServiceClient>(options =>
{
    options.Address = new Uri(builder.Configuration["GrpcServices:CoreServiceUrl"] ?? throw new Exception("Grpc services url is missing"));
})
.ConfigurePrimaryHttpMessageHandler(ConfigureGrpcClientHandler);

builder.Services.AddGrpcClient<SmtpMailService.Proto.SmtpMailGrpcService.SmtpMailGrpcServiceClient>(options =>
{
    options.Address = new Uri(builder.Configuration["GrpcServices:SmtpServiceUrl"] ?? throw new Exception("Grpc services url is missing"));
})
.ConfigurePrimaryHttpMessageHandler(ConfigureGrpcClientHandler);

builder.Services.AddGrpc();
builder.Services.AddCollectionInfrastructure(builder.Configuration)
    .AddApplication();

var privateKeyPath = Path.Combine("certs", "private_key.pem");
var rsa = RSA.Create();

if (!File.Exists(privateKeyPath))
{
    if (builder.Environment.IsDevelopment())
    {
        Console.WriteLine("Private key not found. Generating ephemeral key (restart will invalidate tokens)");
        rsa.KeySize = 2048;
    }
    else
    {
        throw new FileNotFoundException(
            "Private key not found at. " +
            "Generate it with: openssl genpkey -algorithm RSA -out private_key.pem -pkeyopt rsa_keygen_bits:4096");
    }
}
else
{
    var pemContent = File.ReadAllText(privateKeyPath);
    rsa.ImportFromPem(pemContent);
}

builder.Services.AddSingleton(rsa);

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

app.MapGrpcService<AuthService>().EnableGrpcWeb();

app.Run();
return;

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
    chain2.ChainPolicy.VerificationFlags = X509VerificationFlags.NoFlag;
    
    return chain2.Build((X509Certificate2)certificate);
}

X509Certificate2Collection LoadCertificates(string certPath, string keyPath)
{
    if (string.IsNullOrEmpty(certPath) || string.IsNullOrEmpty(keyPath)) 
        return new X509Certificate2Collection();

    var certFile = Path.Combine(AppContext.BaseDirectory, certPath);
    var keyFile = Path.Combine(AppContext.BaseDirectory, keyPath);
    var combinedPem = File.ReadAllText(certFile) + File.ReadAllText(keyFile);
    
    var cert = X509Certificate2.CreateFromPem(combinedPem, combinedPem);
    return new X509Certificate2Collection(cert);
}