using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PatitasCalchaquies.Core.Interfaces;
using PatitasCalchaquies.Core.Services;
using PatitasCalchaquies.Infrastructure.Context;
using PatitasCalchaquies.Infrastructure.Repositories;

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
            services.AddScoped<IRepositoryPersona, RepositorioPersona>();
            services.AddScoped<ServicioPersona>();

            return services;
        }
        
    }
}