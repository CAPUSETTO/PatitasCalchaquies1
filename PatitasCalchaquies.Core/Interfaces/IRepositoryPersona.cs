using PatitasCalchaquies.Core.Entities;
namespace PatitasCalchaquies.Core.Interfaces;

public interface IRepositoryPersona
{
    Task<IEnumerable<Persona>> ObtenerTodosAsync();
    Task<IEnumerable<Persona>> BuscarPersona(string nombre);
    Task<Persona> ObtenerPorIdAsync(int id);
    Task<Persona> ObtenerPorDniAsync(string dni);
    Task<int> CrearPersonaAsync(Persona persona);
    Task<bool> ActualizarPersonaAsync(Persona persona);
    Task<bool> EliminarPersona(int id);
    Task<bool> ExisteDniAsync(string dni);
    Task<bool> ExisteEmailAsync(string email);
}