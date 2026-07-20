using Services.MailService.Infrastructure.Extensions;
using Services.MailService.Presentation.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();
builder.Services.AddCollectionInfrastructure(builder.Configuration);

var app = builder.Build();

// 2. Настройка маршрутов
app.MapGrpcService<EmailGrpcService>();
app.MapGet("/", () => "Email gRPC Microservice (Clean Architecture) is running.");

app.Run();