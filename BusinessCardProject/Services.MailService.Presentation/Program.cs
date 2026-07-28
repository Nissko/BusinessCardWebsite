using Services.MailService.Infrastructure.Extensions;
using Services.MailService.Presentation.Services;

var builder = WebApplication.CreateBuilder(args);

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