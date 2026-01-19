using BusinessCardProject.Server.Core.Domain.Commons.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BusinessCardProject.Server.Core.Infrastructure.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddCollectionInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        if (services == null)
            throw new ArgumentNullException(nameof(services));

        services.AddDbContext<ProjectDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("PostgreSqlDatabase")));
            
        services.AddScoped<IProjectDbContext>(provider => provider.GetService<ProjectDbContext>()
                                                            ?? throw new InvalidOperationException());
        /*//Репозитории
        services.AddScoped<ISubjectRepository, SubjectRepository>();*/
        
            
        //services.AddApplication();
        
        return services;
    }
}