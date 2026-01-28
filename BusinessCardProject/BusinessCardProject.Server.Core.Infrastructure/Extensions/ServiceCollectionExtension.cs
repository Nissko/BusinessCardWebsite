using BusinessCardProject.Server.Core.Application.Application.Extensions;
using BusinessCardProject.Server.Core.Application.Common.Interfaces;
using BusinessCardProject.Server.Core.Application.Common.Interfaces.CustomMediator;
using BusinessCardProject.Server.Core.Infrastructure.Extensions.CustomFunctional;
using BusinessCardProject.Server.Core.Infrastructure.Repositories.Courses;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BusinessCardProject.Server.Core.Infrastructure.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddCollectionInfrastructure(this IServiceCollection services,
        IConfiguration configuration)
    {
        if (services == null)
            throw new ArgumentNullException(nameof(services));

        services.AddDbContext<ProjectDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("PostgreSqlDatabase")));

        services.AddScoped<IProjectDbContext>(provider => provider.GetService<ProjectDbContext>()
                                                          ?? throw new InvalidOperationException());

        //Регистрация кастомного Mediator(-a)
        services.AddScoped<ICustomMediator, CustomMediator>();
        var applicationAssembly = typeof(IRequest<>).Assembly;

        foreach (var type in applicationAssembly.GetTypes())
        {
            foreach (var @interface in type.GetInterfaces())
            {
                if (@interface.IsGenericType)
                {
                    var definition = @interface.GetGenericTypeDefinition();
                    if (definition == typeof(IRequestHandler<,>))
                    {
                        services.AddScoped(@interface, type);
                    }
                    else if (definition == typeof(INotificationHandler<>))
                    {
                        services.AddScoped(@interface, type);
                    }
                }
            }
        }

        //Репозитории
        services.AddScoped<IProgrammingLanguageRepository, ProgrammingLanguageRepository>();

        services.AddApplication();

        return services;
    }
}