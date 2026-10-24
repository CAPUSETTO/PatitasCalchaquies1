using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using PatitasCalchaquies.API.DTOs;
using PatitasCalchaquies.Core.Entities;
using PatitasCalchaquies.Core.Services;

namespace PatitasCalchaquies.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AlimentacionesController : ControllerBase
{
    private readonly ServicioAlimentacion _servicio;
    private readonly IValidator<AlimentacionCrearDto> _validadorCrear;
    private readonly IValidator<AlimentacionActualizarDto> _validadorActualizar;
    private readonly IMapper _mapper;

    public AlimentacionesController(
        ServicioAlimentacion servicio,
        IValidator<AlimentacionCrearDto> validadorCrear,
        IValidator<AlimentacionActualizarDto> validadorActualizar,
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
        var alimentaciones = await _servicio.ObtenerTodasAsync();
        return Ok(_mapper.Map<IEnumerable<AlimentacionDto>>(alimentaciones));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var alimentacion = await _servicio.ObtenerPorIdAsync(id);
        if (alimentacion == null)
            return NotFound(new { mensaje = $"No se encontró el registro de alimentación con ID {id}." });

        return Ok(_mapper.Map<AlimentacionDto>(alimentacion));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] AlimentacionCrearDto dto)
    {
        var validacion = await _validadorCrear.ValidateAsync(dto);
        if (!validacion.IsValid)
        {
            var errores = validacion.Errors.Select(e => e.ErrorMessage).ToList();
            return BadRequest(new { mensaje = "Error de validación", errores });
        }

        try
        {
            var alimentacion = _mapper.Map<Alimentacion>(dto);
            int id = await _servicio.CrearAsync(alimentacion);
            alimentacion.IdAlimentacion = id;

            var respuesta = _mapper.Map<AlimentacionDto>(alimentacion);
            return CreatedAtAction(nameof(GetById), new { id }, respuesta);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] AlimentacionActualizarDto dto)
    {
        if (id != dto.IdAlimentacion)
            return BadRequest(new { mensaje = "El ID de la ruta no coincide con el del cuerpo." });

        var validacion = await _validadorActualizar.ValidateAsync(dto);
        if (!validacion.IsValid)
        {
            var errores = validacion.Errors.Select(e => e.ErrorMessage).ToList();
            return BadRequest(new { mensaje = "Error de validación", errores });
        }

        try
        {
            var alimentacion = _mapper.Map<Alimentacion>(dto);
            bool actualizado = await _servicio.ActualizarAsync(alimentacion);
            if (!actualizado)
                return NotFound(new { mensaje = $"No se pudo actualizar el registro con ID {id}." });

            return Ok(_mapper.Map<AlimentacionDto>(alimentacion));
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
                return NotFound(new { mensaje = $"No se encontró el registro con ID {id}." });

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