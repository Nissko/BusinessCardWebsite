using ByteCodePlatform.Application.Application.Extensions;
using ByteCodePlatform.Infrastructure.Extensions;
using CourseGrpcService = ByteCodePlatform.API.Services.CourseGrpcService;
using UserGrpcService = ByteCodePlatform.API.Services.UserGrpcService;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorClient", policy =>
    {
        policy
            .WithOrigins("https://localhost:7209")
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

builder.Services.AddGrpc();
builder.Services.AddCollectionInfrastructure(builder.Configuration)
    .AddApplication();

var app = builder.Build();

app.UseRouting();
app.UseCors("AllowBlazorClient");
app.UseGrpcWeb();

app.MapGrpcService<UserGrpcService>().EnableGrpcWeb();
app.MapGrpcService<CourseGrpcService>().EnableGrpcWeb();

app.MapGet("/", () => Results.Ok(new 
{ 
    status = "healthy", 
    timestamp = DateTime.UtcNow,
}));

app.Run();