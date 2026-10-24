using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using PatitasCalchaquies.API.DTOs;
using PatitasCalchaquies.Core.Entities;
using PatitasCalchaquies.Core.Services;

namespace PatitasCalchaquies.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SeguimientosAdopcionesController : ControllerBase
{
    private readonly ServicioSeguimientoAdopcion _servicio;
    private readonly IValidator<SeguimientoAdopcionCrearDto> _validadorCrear;
    private readonly IValidator<SeguimientoAdopcionActualizarDto> _validadorActualizar;
    private readonly IMapper _mapper;

    public SeguimientosAdopcionesController(
        ServicioSeguimientoAdopcion servicio,
        IValidator<SeguimientoAdopcionCrearDto> validadorCrear,
        IValidator<SeguimientoAdopcionActualizarDto> validadorActualizar,
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
        var seguimientos = await _servicio.ObtenerTodosAsync();
        return Ok(_mapper.Map<IEnumerable<SeguimientoAdopcionDto>>(seguimientos));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var seguimiento = await _servicio.ObtenerPorIdAsync(id);
        if (seguimiento == null)
            return NotFound(new { mensaje = $"No se encontró el seguimiento con ID {id}." });

        return Ok(_mapper.Map<SeguimientoAdopcionDto>(seguimiento));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] SeguimientoAdopcionCrearDto dto)
    {
        var validacion = await _validadorCrear.ValidateAsync(dto);
        if (!validacion.IsValid)
        {
            var errores = validacion.Errors.Select(e => e.ErrorMessage).ToList();
            return BadRequest(new { mensaje = "Error de validación", errores });
        }

        try
        {
            var seguimiento = _mapper.Map<SeguimientoAdopcion>(dto);
            int id = await _servicio.CrearAsync(seguimiento);
            seguimiento.IdSeguimientoAdopcion = id;

            var respuesta = _mapper.Map<SeguimientoAdopcionDto>(seguimiento);
            return CreatedAtAction(nameof(GetById), new { id }, respuesta);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] SeguimientoAdopcionActualizarDto dto)
    {
        if (id != dto.IdSeguimientoAdopcion)
            return BadRequest(new { mensaje = "El ID de la ruta no coincide con el del cuerpo." });

        var validacion = await _validadorActualizar.ValidateAsync(dto);
        if (!validacion.IsValid)
        {
            var errores = validacion.Errors.Select(e => e.ErrorMessage).ToList();
            return BadRequest(new { mensaje = "Error de validación", errores });
        }

        try
        {
            var seguimiento = _mapper.Map<SeguimientoAdopcion>(dto);
            bool actualizado = await _servicio.ActualizarAsync(seguimiento);
            if (!actualizado)
                return NotFound(new { mensaje = $"No se pudo actualizar el seguimiento con ID {id}." });

            return Ok(_mapper.Map<SeguimientoAdopcionDto>(seguimiento));
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
                return NotFound(new { mensaje = $"No se encontró el seguimiento con ID {id}." });

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