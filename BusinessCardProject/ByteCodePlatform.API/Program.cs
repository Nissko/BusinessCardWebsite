using ByteCodePlatform.Application.Application.Extensions;
using ByteCodePlatform.Infrastructure.Extensions;
using CourseGrpcService = ByteCodePlatform.API.Services.CourseGrpcService;
using UserGrpcService = ByteCodePlatform.API.Services.UserGrpcService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();
builder.Services
    .AddCollectionInfrastructure(builder.Configuration)
    .AddApplication();

var app = builder.Build();
app.MapGrpcService<UserGrpcService>().EnableGrpcWeb();
app.MapGrpcService<CourseGrpcService>().EnableGrpcWeb();

app.MapGet("/", () => Results.Ok(new 
{ 
    status = "healthy", 
    timestamp = DateTime.UtcNow,
}));

app.Run();