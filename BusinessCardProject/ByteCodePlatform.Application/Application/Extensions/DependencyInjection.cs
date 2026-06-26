using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace ByteCodePlatform.Application.Application.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));
            
            // Регистрируем MediatR
            services.AddMediatR(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}