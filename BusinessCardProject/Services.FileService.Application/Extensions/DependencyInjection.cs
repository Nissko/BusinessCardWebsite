using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Services.FileService.Application.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            ArgumentNullException.ThrowIfNull(services);

            // Регистрируем MediatR
            services.AddMediatR(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}