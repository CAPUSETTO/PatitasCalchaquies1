using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using PatitasCalchaquies.API.DTOs;
using PatitasCalchaquies.Core.Entities;
using PatitasCalchaquies.Core.Services;

namespace PatitasCalchaquies.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AlojamientosAnimalesController : ControllerBase
{
    private readonly ServicioAlojamientoAnimal _servicio;
    private readonly IValidator<AlojamientoAnimalCrearDto> _validadorCrear;
    private readonly IValidator<AlojamientoAnimalActualizarDto> _validadorActualizar;
    private readonly IMapper _mapper;

    public AlojamientosAnimalesController(
        ServicioAlojamientoAnimal servicio,
        IValidator<AlojamientoAnimalCrearDto> validadorCrear,
        IValidator<AlojamientoAnimalActualizarDto> validadorActualizar,
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
        var alojamientos = await _servicio.ObtenerTodosAsync();
        return Ok(_mapper.Map<IEnumerable<AlojamientoAnimalDto>>(alojamientos));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var alojamiento = await _servicio.ObtenerPorIdAsync(id);
        if (alojamiento == null)
            return NotFound(new { mensaje = $"No se encontró el alojamiento con ID {id}." });

        return Ok(_mapper.Map<AlojamientoAnimalDto>(alojamiento));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] AlojamientoAnimalCrearDto dto)
    {
        var validacion = await _validadorCrear.ValidateAsync(dto);
        if (!validacion.IsValid)
        {
            var errores = validacion.Errors.Select(e => e.ErrorMessage).ToList();
            return BadRequest(new { mensaje = "Error de validación", errores });
        }

        try
        {
            var alojamiento = _mapper.Map<AlojamientoAnimal>(dto);
            int id = await _servicio.CrearAsync(alojamiento);
            alojamiento.IdAlojamiento = id;

            var respuesta = _mapper.Map<AlojamientoAnimalDto>(alojamiento);
            return CreatedAtAction(nameof(GetById), new { id }, respuesta);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] AlojamientoAnimalActualizarDto dto)
    {
        if (id != dto.IdAlojamiento)
            return BadRequest(new { mensaje = "El ID de la ruta no coincide con el del cuerpo." });

        var validacion = await _validadorActualizar.ValidateAsync(dto);
        if (!validacion.IsValid)
        {
            var errores = validacion.Errors.Select(e => e.ErrorMessage).ToList();
            return BadRequest(new { mensaje = "Error de validación", errores });
        }

        try
        {
            var alojamiento = _mapper.Map<AlojamientoAnimal>(dto);
            bool actualizado = await _servicio.ActualizarAsync(alojamiento);
            if (!actualizado)
                return NotFound(new { mensaje = $"No se pudo actualizar el alojamiento con ID {id}." });

            return Ok(_mapper.Map<AlojamientoAnimalDto>(alojamiento));
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
                return NotFound(new { mensaje = $"No se encontró el alojamiento con ID {id}." });

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