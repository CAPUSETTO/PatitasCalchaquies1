using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using PatitasCalchaquies.API.DTOs;
using PatitasCalchaquies.Core.Entities;
using PatitasCalchaquies.Core.Services;

namespace PatitasCalchaquies.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AdopcionesController : ControllerBase
{
    private readonly ServicioAdopcion _servicio;
    private readonly IValidator<AdopcionCrearDto> _validadorCrear;
    private readonly IValidator<AdopcionActualizarDto> _validadorActualizar;
    private readonly IMapper _mapper;

    public AdopcionesController(
        ServicioAdopcion servicio,
        IValidator<AdopcionCrearDto> validadorCrear,
        IValidator<AdopcionActualizarDto> validadorActualizar,
        IMapper mapper)
    {
        _servicio = servicio;
        _validadorCrear = validadorCrear;
        _validadorActualizar = validadorActualizar;
        _mapper = mapper;
    }

    [HttpGet]
    
    public async Task<IActionResult> GetAll()
    {
        var adopciones = await _servicio.ObtenerTodasAsync();
        return Ok(_mapper.Map<IEnumerable<AdopcionDto>>(adopciones));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var adopcion = await _servicio.ObtenerPorIdAsync(id);
        if (adopcion == null)
            return NotFound(new { mensaje = $"No se encontró la adopción con ID {id}." });

        return Ok(_mapper.Map<AdopcionDto>(adopcion));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] AdopcionCrearDto dto)
    {
        var validacion = await _validadorCrear.ValidateAsync(dto);
        if (!validacion.IsValid)
        {
            var errores = validacion.Errors.Select(e => e.ErrorMessage).ToList();
            return BadRequest(new { mensaje = "Error de validación", errores });
        }

        try
        {
            var adopcion = _mapper.Map<Adopcion>(dto);
            int id = await _servicio.CrearAsync(adopcion);
            adopcion.IdAdopcion = id;

            var respuesta = _mapper.Map<AdopcionDto>(adopcion);
            return CreatedAtAction(nameof(GetById), new { id }, respuesta);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] AdopcionActualizarDto dto)
    {
        if (id != dto.IdAdopcion)
            return BadRequest(new { mensaje = "El ID de la ruta no coincide con el del cuerpo." });

        var validacion = await _validadorActualizar.ValidateAsync(dto);
        if (!validacion.IsValid)
        {
            var errores = validacion.Errors.Select(e => e.ErrorMessage).ToList();
            return BadRequest(new { mensaje = "Error de validación", errores });
        }

        try
        {
            var adopcion = _mapper.Map<Adopcion>(dto);
            bool actualizado = await _servicio.ActualizarAsync(adopcion);
            if (!actualizado)
                return NotFound(new { mensaje = $"No se pudo actualizar la adopción con ID {id}." });

            return Ok(_mapper.Map<AdopcionDto>(adopcion));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            bool eliminado = await _servicio.EliminarAsync(id);
            if (!eliminado)
                return NotFound(new { mensaje = $"No se encontró la adopción con ID {id}." });

            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}