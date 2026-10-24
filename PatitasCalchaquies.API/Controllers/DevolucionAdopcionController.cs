using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using PatitasCalchaquies.API.DTOs;
using PatitasCalchaquies.Core.Entities;
using PatitasCalchaquies.Core.Services;

namespace PatitasCalchaquies.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DevolucionesAdopcionesController : ControllerBase
{
    private readonly ServicioDevolucionAdopcion _servicio;
    private readonly IValidator<DevolucionAdopcionCrearDto> _validadorCrear;
    private readonly IValidator<DevolucionAdopcionActualizarDto> _validadorActualizar;
    private readonly IMapper _mapper;

    public DevolucionesAdopcionesController(
        ServicioDevolucionAdopcion servicio,
        IValidator<DevolucionAdopcionCrearDto> validadorCrear,
        IValidator<DevolucionAdopcionActualizarDto> validadorActualizar,
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
        var devoluciones = await _servicio.ObtenerTodasAsync();
        return Ok(_mapper.Map<IEnumerable<DevolucionAdopcionDto>>(devoluciones));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var devolucion = await _servicio.ObtenerPorIdAsync(id);
        if (devolucion == null)
            return NotFound(new { mensaje = $"No se encontró la devolución con ID {id}." });

        return Ok(_mapper.Map<DevolucionAdopcionDto>(devolucion));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] DevolucionAdopcionCrearDto dto)
    {
        var validacion = await _validadorCrear.ValidateAsync(dto);
        if (!validacion.IsValid)
        {
            var errores = validacion.Errors.Select(e => e.ErrorMessage).ToList();
            return BadRequest(new { mensaje = "Error de validación", errores });
        }

        try
        {
            var devolucion = _mapper.Map<DevolucionAdopcion>(dto);
            int id = await _servicio.CrearAsync(devolucion);
            devolucion.IdDevolucionAdopcion = id;

            var respuesta = _mapper.Map<DevolucionAdopcionDto>(devolucion);
            return CreatedAtAction(nameof(GetById), new { id }, respuesta);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] DevolucionAdopcionActualizarDto dto)
    {
        if (id != dto.IdDevolucionAdopcion)
            return BadRequest(new { mensaje = "El ID de la ruta no coincide con el del cuerpo." });

        var validacion = await _validadorActualizar.ValidateAsync(dto);
        if (!validacion.IsValid)
        {
            var errores = validacion.Errors.Select(e => e.ErrorMessage).ToList();
            return BadRequest(new { mensaje = "Error de validación", errores });
        }

        try
        {
            var devolucion = _mapper.Map<DevolucionAdopcion>(dto);
            bool actualizado = await _servicio.ActualizarAsync(devolucion);
            if (!actualizado)
                return NotFound(new { mensaje = $"No se pudo actualizar la devolución con ID {id}." });

            return Ok(_mapper.Map<DevolucionAdopcionDto>(devolucion));
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
                return NotFound(new { mensaje = $"No se encontró la devolución con ID {id}." });

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