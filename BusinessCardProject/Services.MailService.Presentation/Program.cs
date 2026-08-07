using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Services.MailService.Infrastructure.Extensions;
using Services.MailService.Presentation.Services;

var builder = WebApplication.CreateBuilder(args);

var certPath = builder.Configuration["GrpcClientCertificate:Path"];
var keyPath = builder.Configuration["GrpcClientCertificate:KeyPath"];
var rootCaPath = builder.Configuration["RootCaCertificate:Path"];
var listenPort = int.Parse(builder.Configuration["GrpcServices:ListenPort"] ?? "5223");

if (!string.IsNullOrEmpty(certPath) && !string.IsNullOrEmpty(keyPath))
{
    var certFile = Path.Combine(AppContext.BaseDirectory, certPath);
    var keyFile = Path.Combine(AppContext.BaseDirectory, keyPath);
    var combinedPem = File.ReadAllText(certFile) + File.ReadAllText(keyFile);
    var certificate = X509Certificate2.CreateFromPem(combinedPem, combinedPem);

    builder.WebHost.ConfigureKestrel(options =>
    {
        options.ListenAnyIP(listenPort, listenOptions =>
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
        options.ListenAnyIP(listenPort, listenOptions =>
        {
            listenOptions.Protocols = HttpProtocols.Http1AndHttp2;
            if (builder.Environment.IsDevelopment())
            {
                listenOptions.UseHttps();
            }
        });
    });
}

builder.Services.AddGrpc();
builder.Services.AddCollectionInfrastructure(builder.Configuration);

if (!string.IsNullOrEmpty(rootCaPath))
{
    builder.Services.AddGrpcClient<AuthorizationService.Proto.AuthorizationService.AuthorizationServiceClient>(options =>
    {
        options.Address = new Uri(builder.Configuration["GrpcServices:AuthServiceUrl"] ??
                                  throw new Exception("Grpc services url is missing"));
    })
    .ConfigurePrimaryHttpMessageHandler(sp =>
    {
        var rootCaFile = Path.Combine(AppContext.BaseDirectory, rootCaPath);
        var rootCaBytes = File.ReadAllBytes(rootCaFile);
        var rootCert = new X509Certificate2(rootCaBytes);

        var sslOptions = new SslClientAuthenticationOptions
        {
            ClientCertificates = LoadCertificates(certPath, keyPath)
        };
        sslOptions.RemoteCertificateValidationCallback = ValidateRemoteCertificate;

        var handler = new SocketsHttpHandler
        {
            EnableMultipleHttp2Connections = true,
            SslOptions = sslOptions
        };

        return handler;
    });
}

var app = builder.Build();
app.MapGrpcService<EmailGrpcService>();
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

static X509Certificate2Collection LoadCertificates(string certPath, string keyPath)
{
    if (string.IsNullOrEmpty(certPath) || string.IsNullOrEmpty(keyPath))
        return new X509Certificate2Collection();

    var certFile = Path.Combine(AppContext.BaseDirectory, certPath);
    var keyFile = Path.Combine(AppContext.BaseDirectory, keyPath);
    var combinedPem = File.ReadAllText(certFile) + File.ReadAllText(keyFile);

    var cert = X509Certificate2.CreateFromPem(combinedPem, combinedPem);
    return new X509Certificate2Collection(cert);
}