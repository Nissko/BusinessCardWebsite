using Services.MailService.Infrastructure.Extensions;
using Services.MailService.Presentation.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();
builder.Services.AddCollectionInfrastructure(builder.Configuration);

var app = builder.Build();
app.MapGrpcService<EmailGrpcService>();
app.Run();