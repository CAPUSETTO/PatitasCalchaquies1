using PatitasCalchaquies.Core.Entities;
using PatitasCalchaquies.Core.Interfaces;

namespace PatitasCalchaquies.Core.Services;

public class ServicioPersona
{
    private readonly IRepositoryPersona _repositorio;

    public ServicioPersona(IRepositoryPersona repositorio)
    {
        _repositorio = repositorio;
    }

    public async Task<IEnumerable<Persona>> Allsync()
    {
        return await _repositorio.ObtenerTodosAsync();
    }
    
    public async Task<Persona> GetByIdAsync(int id)
    {
        if (id<=0)
        {
            throw new ArgumentException("El id debe ser mayor a 0");
        }
        return await _repositorio.ObtenerPorIdAsync(id);
    }
    
    public async Task<Persona?> GetByDniAsync(string dni)
    {
        if (string.IsNullOrWhiteSpace(dni))
        {
            throw new ArgumentException("El DNI no puede estar vacío");
        }
        return await _repositorio.ObtenerPorDniAsync(dni);
    }

    public async Task<IEnumerable<Persona>> BuscarPersonaAsync(string texto)
    {
        if (string.IsNullOrWhiteSpace(texto))
        {
            return await _repositorio.ObtenerTodosAsync();
        }
        return await _repositorio.BuscarPersona(texto);
    }

    public async Task<int> CrearPersonaAsync(Persona persona)
    {
        if (string.IsNullOrWhiteSpace(persona.Nombre) || string.IsNullOrWhiteSpace(persona.Apellido))
        {
            throw new ArgumentException("El nombre y el apellido son obligatorios");
        }

        if (string.IsNullOrWhiteSpace(persona.Dni))
        {
            throw new ArgumentException("El dni es obligatorio");
        }

        if (await _repositorio.ExisteDniAsync(persona.Dni))
        {
            throw new ArgumentException("El DNI ya existe registrado en otra persona.");
        }

        persona.Email = string.IsNullOrWhiteSpace(persona.Email) ? null : persona.Email.Trim();

        if (!string.IsNullOrWhiteSpace(persona.Email) && await _repositorio.ExisteEmailAsync(persona.Email))
        {
            throw new InvalidOperationException("Ya existe una persona registrada con ese email.");
        }
        
        int id = await _repositorio.CrearPersonaAsync(persona);
        if (id <= 0)
        {
            throw new InvalidOperationException("No se pudo guardar la persona en la base de datos.");
        }
        return id;
    }

    public async Task<bool> ActualizarPersonaAsync(Persona persona)
    {
        if (persona.IdPersona <= 0)
        {
            throw new ArgumentException("El Id de la persona debe ser mayor a 0");
        }
        
        var existente  = await _repositorio.ObtenerPorIdAsync(persona.IdPersona);
        if (existente == null)
        {
            return false;
        }

        persona.Email = string.IsNullOrWhiteSpace(persona.Email) ? null : persona.Email.Trim();
        return await _repositorio.ActualizarPersonaAsync(persona);
    }

    public async Task<bool> EliminarPersonaAsync(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentException("El Id de la persona debe ser mayor a 0");
        }

        return await _repositorio.EliminarPersona(id);
    }
}
