using ApiEndpoints;
using BusinessCardProject.Server.Core.Application.Application.Extensions;
using BusinessCardProject.Server.Core.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

/*TODO: вкл.на хостинге*/
//builder.Services.AddResponseCaching();

builder.Services
    .AddCollectionInfrastructure(builder.Configuration)
    .AddApplication();

// Настройка CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorClient",
        builder => builder.WithOrigins(ApiEndpointRoutes.BaseFrontUrl)
            .AllowAnyMethod()
            .AllowAnyHeader());
});

// Add services to the container.
builder.Services.AddControllers();

// Настройка Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Business Card Project API",
        Version = "v1",
        Description = "API для управления бизнес-картами",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Ваше имя/команда",
            Email = "email@example.com"
        }
    });
});

var app = builder.Build();

// TODO: избавиться от swagger(-a) и переделать на тесты
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Business Card API v1");
        options.RoutePrefix = "swagger"; // Доступ по /swagger
        options.DisplayRequestDuration();
        options.EnableTryItOutByDefault();
    });
}
else
{
    /*TODO: включить на серваке*/
    app.UseResponseCaching();
    app.UseRouting();
}

app.UseCors("AllowBlazorClient");
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();