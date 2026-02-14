using ApiEndpoints;
using BusinessCardProject.Server.Core.Application.Application.Extensions;
using BusinessCardProject.Server.Core.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);
/*TODO: вкл.на хостинге*/
//builder.Services.AddResponseCaching();

ApiEndpointRoutes.Init(builder.Configuration);

builder.Services
    .AddCollectionInfrastructure(builder.Configuration)
    .AddApplication();

// Настройка CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorClient",
        policyBuilder => policyBuilder
            .WithOrigins(
                ApiEndpointRoutes.BaseFrontUrl.TrimEnd('/')
            )
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Business Card Project API",
        Version = "v1",
        Description = "API для курсами",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "NisskoDevelop",
            Email = "skibko.nik@mail.ru"
        }
    });
});

var app = builder.Build();

app.UseCors("AllowBlazorClient");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Business Card API v1");
        options.RoutePrefix = "swagger";
        options.DisplayRequestDuration();
        options.EnableTryItOutByDefault();
    });
    app.UseHttpsRedirection();
}
else
{
    app.UseResponseCaching();
}

app.UseRouting();
app.UseAuthorization();
app.MapControllers();
app.Run();