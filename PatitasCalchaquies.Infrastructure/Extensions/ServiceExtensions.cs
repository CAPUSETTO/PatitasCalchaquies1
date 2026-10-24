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
            
            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
            
            services.AddSingleton<IMyConexion>(new MyConexion(connectionString));
            //persona
            services.AddScoped<IRepositoryPersona, RepositorioPersona>();
            services.AddScoped<ServicioPersona>();
            //medicamento
            services.AddScoped<IRepositoryMedicamento, RepositorioMedicamento>();
            services.AddScoped<ServicioMedicamento>();
            //Vacuna
            services.AddScoped<IRepositoryVacuna, RepositorioVacuna>();
            services.AddScoped<ServicioVacuna>();
            //Raza
            services.AddScoped<IRepositoryRaza, RepositorioRaza>();
            services.AddScoped<ServicioRaza>();
            //Animal
            services.AddScoped<IRepositoryAnimal, RepositorioAnimal>();
            services.AddScoped<ServicioAnimal>();
            //Adopcion
            services.AddScoped<IRepositorioAdopcion, RepositorioAdopcion>();
            services.AddScoped<ServicioAdopcion>();
            //Alimentacion
            services.AddScoped<IRepositorioAlimentacion, RepositorioAlimentacion>();
            services.AddScoped<ServicioAlimentacion>();
            //AlojamientoAnimal
            services.AddScoped<IRepositorioAlojamientoAnimal, RepositorioAlojamientoAnimal>();
            services.AddScoped<ServicioAlojamientoAnimal>();
            
            //AtencionVeterinario
            services.AddScoped<IRepositorioAtencionVeterinaria, RepositorioAtencionVeterinaria>();
            services.AddScoped<ServicioAtencionVeterinaria>();
            
            //DevolucionAdopcion
            services.AddScoped<IRepositorioDevolucionAdopcion, RepositorioDevolucionAdopcion>();
            services.AddScoped<ServicioDevolucionAdopcion>();
            
            //Especie
            services.AddScoped<IRepositorioEspecie, RepositorioEspecie>();
            services.AddScoped<ServicioEspecie>();
            
            //EstadoAnimal
            services.AddScoped<IRepositorioEstadoAnimal, RepositorioEstadoAnimal>();
            services.AddScoped<ServicioEstadoAnimal>();

            //Esterilizacion
            services.AddScoped<IRepositorioEsterilizacion, RepositorioEsterilizacion>();
            services.AddScoped<ServicioEsterilizacion>();
            
            //IngresoAnimal
            services.AddScoped<IRepositorioIngresoAnimal, RepositorioIngresoAnimal>();
            services.AddScoped<ServicioIngresoAnimal>();
            
            //FotoAnimal
            services.AddScoped<IRepositorioFotoAnimal, RepositorioFotoAnimal>();
            services.AddScoped<ServicioFotoAnimal>();
            
            //PlanAlimentacion
            services.AddScoped<IRepositorioPlanAlimentacion, RepositorioPlanAlimentacion>();
            services.AddScoped<ServicioPlanAlimentacion>();
            
            //Predio
            services.AddScoped<IRepositorioPredio, RepositorioPredio>();
            services.AddScoped<ServicioPredio>();
            
            //Rol
            services.AddScoped<IRepositorioRol, RepositorioRol>();
            services.AddScoped<ServicioRol>();
            
            //Sector
            services.AddScoped<IRepositorioSector, RepositorioSector>();
            services.AddScoped<ServicioSector>();
            
            //SeguimientoAdopcion
            services.AddScoped<IRepositorioSeguimientoAdopcion, RepositorioSeguimientoAdopcion>();
            services.AddScoped<ServicioSeguimientoAdopcion>();
            
            //Sexo
            services.AddScoped<IRepositorioSexo, RepositorioSexo>();
            services.AddScoped<ServicioSexo>();
            
            //SolicitudAdopcion
            services.AddScoped<IRepositorioSolicitudAdopcion, RepositorioSolicitudAdopcion>();
            services.AddScoped<ServicioSolicitudAdopcion>();
            
            //Tratamiento
            services.AddScoped<IRepositorioTratamiento, RepositorioTratamiento>();
            services.AddScoped<ServicioTratamiento>();
            
            //TratamientoMedicamento
            services.AddScoped<IRepositorioTratamientoMedicamento, RepositorioTratamientoMedicamento>();
            services.AddScoped<ServicioTratamientoMedicamento>();
            
            //Usuarios
            services.AddScoped<IRepositorioUsuario, RepositorioUsuario>();
            services.AddScoped<ServicioUsuario>();
            
            //UsuarioRol
            services.AddScoped<IRepositorioUsuarioRol, RepositorioUsuarioRol>();
            services.AddScoped<ServicioUsuarioRol>();
            
            //Vacunacion
            services.AddScoped<IRepositorioVacunacion, RepositorioVacunacion>();
            services.AddScoped<ServicioVacunacion>();
            
            //Veterinario
            services.AddScoped<IRepositorioVeterinario, RepositorioVeterinario>();
            services.AddScoped<ServicioVeterinario>();
            
            return services;
        }
        
    }
}