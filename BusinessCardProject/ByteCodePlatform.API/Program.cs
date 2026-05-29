using System.Security.Cryptography;
using ByteCodePlatform.Application.Application.Extensions;
using ByteCodePlatform.Infrastructure.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.IdentityModel.Tokens;
using CourseGrpcService = ByteCodePlatform.API.Services.CourseGrpcService;
using UserGrpcService = ByteCodePlatform.API.Services.UserGrpcService;

var builder = WebApplication.CreateBuilder(args);
     
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5221, listenOptions =>
    {
        listenOptions.Protocols = HttpProtocols.Http1AndHttp2;
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorClient", policy =>
    {
        policy
            .WithOrigins(builder.Configuration["GrpcServices:ClientUrl"] ?? throw new Exception("Grpc services url is missing"))
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

app.MapGrpcService<UserGrpcService>().EnableGrpcWeb();
app.MapGrpcService<CourseGrpcService>().EnableGrpcWeb();

app.Run();

/*
 * TODO: сделать чтобы при добавлении автора ставился признак пользователю
 */