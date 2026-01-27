using BusinessCardProject.Server.Core.Application.Application.Extensions;
using BusinessCardProject.Server.Core.Infrastructure.Extensions;

//TODO: Autofac, удалить?
/*ContainerBuilder build = new ContainerBuilder();
build.RegisterModule(new ApplicationModule());*/

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddCollectionInfrastructure(builder.Configuration)
    .AddApplication();

// Настройка CORS
/*builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorClient",
        builder => builder.WithOrigins("https://localhost:7089")
            .AllowAnyMethod()
            .AllowAnyHeader());
});*/

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

var app = builder.Build();
app.UseCors("AllowBlazorClient");

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "OpenApi v1");
        options.RoutePrefix = "swagger";
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();