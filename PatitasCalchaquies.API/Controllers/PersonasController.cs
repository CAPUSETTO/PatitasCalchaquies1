using Microsoft.AspNetCore.Mvc;
using PatitasCalchaquies.Core.Entities;
using PatitasCalchaquies.Core.Services;

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

    [HttpGet("{Id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var personas = await _servicio.GetByIdAsync(id);
        if (personas == null)
        {
            return NotFound();
        }
        return Ok(personas);
    }

    [HttpGet("{dni}")]
    public async Task<IActionResult> GetByDni(string dni)
    {
        var personas = await _servicio.GetByDniAsync(dni);
        if (personas == null)
        {
            return NotFound();
        }
        return Ok(personas);
    }

    [HttpGet("buscar")]
    public async Task<IActionResult> Buscar([FromQuery] string nombre)
    {
        var resultados = _servicio.BuscarPersonaAsync(nombre);
        return Ok(resultados);
    }

    [HttpPost]
    public async Task<IActionResult> Crear([FromBody] Persona persona)
    {
        try
        {
            int nuevoId = await _servicio.CrearPersonaAsync(persona);
            persona.IdPersona = nuevoId;
            
            return CreatedAtAction(nameof(GetById), new { id = nuevoId }, persona);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Actualizar([FromBody] int id, Persona persona)
    {
        if (id != persona.IdPersona)
        {
            return BadRequest(new {error = "El ID no coincide con el cuerpo petición"});
        }

        try
        {
            bool resultado = await _servicio.ActualizarPersonaAsync(persona);
            if (resultado)
            {
                return NotFound(new { mensaje = $"No se pudo encontrar la persona con ID {id} para actualizar." });
            }
            return Ok(new { mensaje = "Persona actualizada correctamente." });
        }
        catch (ArgumentException ex)
        {
            return  BadRequest(new { message = ex.Message });
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
            return Ok(new { mensaje = "Persona eliminada correctamente." });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
    
    
}