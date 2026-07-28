using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Services.FileService.Application.Common.Interfaces;
using Services.FileService.Infrastructure.Repositories;

namespace Services.FileService.Infrastructure.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            ArgumentNullException.ThrowIfNull(services);
            ArgumentNullException.ThrowIfNull(configuration);
            
            var mongoConnectionString = configuration.GetConnectionString("MongoDb")
                                        ?? throw new InvalidOperationException("MongoDb connection string is not configured");
            var databaseName = configuration["MongoDbSettings:DatabaseName"]
                               ?? throw new InvalidOperationException("MongoDbSettings:DatabaseName is not configured");
            
            services.AddSingleton<IMongoClient>(sp =>
            {
                var settings = MongoClientSettings.FromConnectionString(mongoConnectionString);
                settings.MaxConnectionPoolSize = 100;
                settings.ConnectTimeout = TimeSpan.FromSeconds(5);
                settings.ServerSelectionTimeout = TimeSpan.FromSeconds(5);
                
                return new MongoClient(settings);
            });
            
            services.AddScoped<IMongoDatabase>(sp =>
            {
                var client = sp.GetRequiredService<IMongoClient>();
                return client.GetDatabase(databaseName);
            });
            
            services.AddScoped<IImageRepository, ImageRepository>();

            return services;
        }
    }
}