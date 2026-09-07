using Microsoft.Extensions.DependencyInjection;
using PatitasCalchaquies.Core.Interfaces;
using PatitasCalchaquies.Infrastructure.Contexto;

using Microsoft.Extensions.Configuration;

namespace PatitasCalchaquies.Infrastructure.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("La cadena de conexión 'DefaultConnection' no está configurada.");

            services.AddSingleton<IMyConexion>(new MyConexion(connectionString));

            return services;
        }
    }
}