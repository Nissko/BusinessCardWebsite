using ByteCodePlatform.Application.Application.Extensions;
using ByteCodePlatform.Application.Common.Interfaces;
using ByteCodePlatform.Application.Common.Interfaces.Repositories;
using ByteCodePlatform.Infrastructure.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ByteCodePlatform.Infrastructure.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddCollectionInfrastructure(this IServiceCollection services,
            IConfiguration configuration)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            services.AddDbContext<ByteCodeCoreDbContext>(options =>
                options.UseNpgsql(
                    configuration.GetConnectionString("PostgreSql"),
                    npgsqlOptions => npgsqlOptions.UseNodaTime()
                ).UseLazyLoadingProxies());
            
            //Регистрация сервисов
            services.AddScoped<IMediator, Mediator>();
            
            //Репозитории
            services.AddScoped<IByteCodeCoreDbContext, ByteCodeCoreDbContext>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ICourseRepository, CourseRepository>();
            
            //Регистрация gRPC
            //services.AddScoped<IChatGrpcClient, ChatGrpcClientService>();

            services.AddApplication();

            return services;
        }
    }
}