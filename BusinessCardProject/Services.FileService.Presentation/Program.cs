using Services.FileService.Application.Extensions;
using Services.FileService.Infrastructure.Extensions;
using Services.FileService.Presentation.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddGrpc();
builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorClient", policy =>
    {
        policy
            .WithOrigins(
                builder.Configuration["GrpcServices:ClientUrl"] ?? throw new Exception("Grpc services url is missing"),
                builder.Configuration["GrpcAdminService:UrlToAdminPanel"] ??
                throw new Exception("Grpc admin service url is missing")
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

var app = builder.Build();

app.UseCors("AllowBlazorClient");
app.UseGrpcWeb();
app.MapGrpcService<FileGrpcService>().EnableGrpcWeb();
app.MapControllers();

app.Run();