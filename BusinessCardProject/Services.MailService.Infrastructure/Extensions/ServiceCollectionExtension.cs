using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Services.MailService.Application.Common.Interfaces;
using Services.MailService.Application.Extensions;
using Services.MailService.Infrastructure.Repositories;
using Services.MailService.Infrastructure.Settings;

namespace Services.MailService.Infrastructure.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddCollectionInfrastructure(this IServiceCollection services,
            IConfiguration configuration)
        {
            ArgumentNullException.ThrowIfNull(services);

            services.AddDbContext<MailDbContext>(options =>
                options.UseNpgsql(
                    configuration.GetConnectionString("PostgreSql"),
                    npgsqlOptions => npgsqlOptions.UseNodaTime()
                ).UseLazyLoadingProxies());
            
            services.Configure<MailSettings>(configuration.GetSection("MailSettings"));
            services.AddTransient<IEmailSender, EmailRepository>();
        
            services.AddApplication();
            
            return services;
        }
    }
}