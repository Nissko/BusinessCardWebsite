/*ContainerBuilder build = new ContainerBuilder();
build.RegisterModule(new ApplicationModule());*/

using BusinessCardProject.Server.Core.Application.Application.Extensions;
using BusinessCardProject.Server.Core.Infrastructure.Extensions;

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
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseCors("AllowBlazorClient");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

//app.MapControllers();

app.Run();