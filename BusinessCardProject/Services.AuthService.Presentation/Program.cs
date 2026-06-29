using System.Security.Cryptography;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.IdentityModel.Tokens;
using Services.AuthService.Application.Application.Extensions;
using Services.AuthService.Infrastructure.Extensions;
using Services.AuthService.Presentation.Services;
using UserService.Proto;

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

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorClient", policy =>
    {
        policy
            .WithOrigins(
                builder.Configuration["GrpcServices:ClientUrl"] ?? throw new Exception("Grpc services url is missing"),
                "https://localhost:7237"
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

builder.Services.AddGrpcClient<UserGrpcService.UserGrpcServiceClient>(options =>
{
    options.Address = new Uri(builder.Configuration["GrpcServices:CoreServiceUrl"] ?? throw new Exception("Grpc services url is missing"));
});

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

/*
 * TODO: сделать MiddleWare проверку на существование токенов
 */