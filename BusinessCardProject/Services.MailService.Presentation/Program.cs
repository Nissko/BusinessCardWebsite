using Microsoft.AspNetCore.Server.Kestrel.Core;
using Services.MailService.Infrastructure.Extensions;
using Services.MailService.Presentation.Services;

var builder = WebApplication.CreateBuilder(args);

var port = int.Parse(builder.Configuration["GrpcServices:ListenPort"] ?? throw new Exception("Port is missing"));
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

builder.Services.AddGrpc();
builder.Services.AddCollectionInfrastructure(builder.Configuration);

builder.Services.AddGrpcClient<AuthorizationService.Proto.AuthorizationService.AuthorizationServiceClient>(options =>
{
    options.Address = new Uri(builder.Configuration["GrpcServices:AuthServiceUrl"] ??
                              throw new Exception("Grpc services url is missing"));
});

var app = builder.Build();
app.MapGrpcService<EmailGrpcService>();
app.Run();