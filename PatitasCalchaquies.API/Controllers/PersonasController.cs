using Microsoft.AspNetCore.Mvc;
using PatitasCalchaquies.API.DTOs;
using PatitasCalchaquies.Core.Entities;
using PatitasCalchaquies.Core.Services;
using MySqlConnector;

namespace PatitasCalchaquies.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PersonasController : ControllerBase
{
    private readonly ServicioPersona _servicio;

    public PersonasController(ServicioPersona servicio)
    {
        _servicio = servicio;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var personas = await _servicio.Allsync();
        return Ok(personas);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var persona = await _servicio.GetByIdAsync(id);
        if (persona == null)
        {
            return NotFound();
        }
        return Ok(persona);
    }

    [HttpGet("dni/{dni}")]
    public async Task<IActionResult> GetByDni(string dni)
    {
        var persona = await _servicio.GetByDniAsync(dni);
        if (persona == null)
        {
            return NotFound();
        }
        return Ok(persona);
    }

    [HttpGet("buscar")]
    public async Task<IActionResult> Buscar([FromQuery] string nombre)
    {
        try
        {
            var resultados = await _servicio.BuscarPersonaAsync(nombre);
            return Ok(resultados);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost]
    public async Task<IActionResult> Crear([FromBody] PersonaCrearDto dto)
    {
        try
        {
            Persona persona = new Persona
            {
                Apellido = dto.Apellido,
                Nombre = dto.Nombre,
                Dni = dto.Dni,
                Telefono = dto.Telefono,
                Email = dto.Email,
                Direccion = dto.Direccion
            };

            int nuevoId = await _servicio.CrearPersonaAsync(persona);
            persona.IdPersona = nuevoId;

            PersonaDto nuevaPersona = new PersonaDto
            {
                IdPersona = persona.IdPersona,
                Apellido = persona.Apellido ?? string.Empty,
                Nombre = persona.Nombre ?? string.Empty,
                Dni = persona.Dni,
                Telefono = persona.Telefono,
                Email = persona.Email,
                Direccion = persona.Direccion,
                FechaAlta = persona.FechaAlta,
                Activo = persona.Activo
            };
            
            return CreatedAtAction(nameof(GetById), new { id = nuevoId }, nuevaPersona);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Actualizar(int id, [FromBody] PersonaActualizarDto dto)
    {
        if (id != dto.IdPersona)
        {
            return BadRequest(new { error = "El ID de la URL no coincide con el cuerpo de la petición" });
        }

        try
        {
            Persona persona = new Persona
            {
                IdPersona = dto.IdPersona,
                Apellido = dto.Apellido,
                Nombre = dto.Nombre,
                Dni = dto.Dni,
                Telefono = dto.Telefono,
                Email = dto.Email,
                Direccion = dto.Direccion,
                Activo = dto.Activo ?? false
            };

            bool resultado = await _servicio.ActualizarPersonaAsync(persona);
            if (!resultado)
            {
                return NotFound(new { mensaje = $"No se pudo encontrar la persona con ID {id} para actualizar." });
            }

            PersonaDto personaActualizada = new PersonaDto
            {
                IdPersona = persona.IdPersona,
                Apellido = persona.Apellido ?? string.Empty,
                Nombre = persona.Nombre ?? string.Empty,
                Dni = persona.Dni,
                Telefono = persona.Telefono,
                Email = persona.Email,
                Direccion = persona.Direccion,
                FechaAlta = persona.FechaAlta,
                Activo = persona.Activo
            };

            return Ok(personaActualizada);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (MySqlException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Eliminar(int id)
    {
        try
        {
            bool eliminado = await _servicio.EliminarPersonaAsync(id);
            if (!eliminado)
            {
                return NotFound(new { mensaje = $"No se encontró la persona con ID {id} para eliminar." });
            }
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}