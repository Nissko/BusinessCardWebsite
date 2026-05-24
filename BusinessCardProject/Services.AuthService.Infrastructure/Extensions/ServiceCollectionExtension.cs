using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Services.AuthService.Application.Application.Extensions;
using Services.AuthService.Application.Application.GrpcClients;
using Services.AuthService.Application.Common.Interfaces;
using Services.AuthService.Application.Common.Interfaces.GrpcClients;
using Services.AuthService.Infrastructure.Repositories;

namespace Services.AuthService.Infrastructure.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddCollectionInfrastructure(this IServiceCollection services,
            IConfiguration configuration)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            services.AddDbContext<AuthDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("PostgreSqlDatabase")));

            services.AddScoped<AuthDbContext>(provider => provider.GetService<AuthDbContext>()
                                                                 ?? throw new InvalidOperationException());
            
            //Регистрация сервисов
            services.AddScoped<IMediator, Mediator>();
            
            //Репозитории
            services.AddScoped<IAuthDbContext, AuthDbContext>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRefreshToken, RefreshToken>();
            
            //Клиенты
            services.AddScoped<IUserCoreGrpcService, CoreUserServiceClient>();

            services.AddApplication();

            return services;
        }
    }
}