using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using PatitasCalchaquies.API.DTOs;
using PatitasCalchaquies.Core.Entities;
using PatitasCalchaquies.Core.Services;

namespace PatitasCalchaquies.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SolicitudesAdopcionesController : ControllerBase
{
    private readonly ServicioSolicitudAdopcion _servicio;
    private readonly IValidator<SolicitudAdopcionCrearDto> _validadorCrear;
    private readonly IValidator<SolicitudAdopcionActualizarDto> _validadorActualizar;
    private readonly IMapper _mapper;

    public SolicitudesAdopcionesController(
        ServicioSolicitudAdopcion servicio,
        IValidator<SolicitudAdopcionCrearDto> validadorCrear,
        IValidator<SolicitudAdopcionActualizarDto> validadorActualizar,
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
        var solicitudes = await _servicio.ObtenerTodasAsync();
        return Ok(_mapper.Map<IEnumerable<SolicitudAdopcionDto>>(solicitudes));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var solicitud = await _servicio.ObtenerPorIdAsync(id);
        if (solicitud == null)
            return NotFound(new { mensaje = $"No se encontró la solicitud de adopción con ID {id}." });

        return Ok(_mapper.Map<SolicitudAdopcionDto>(solicitud));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] SolicitudAdopcionCrearDto dto)
    {
        var validacion = await _validadorCrear.ValidateAsync(dto);
        if (!validacion.IsValid)
        {
            var errores = validacion.Errors.Select(e => e.ErrorMessage).ToList();
            return BadRequest(new { mensaje = "Error de validación", errores });
        }

        try
        {
            var solicitud = _mapper.Map<SolicitudAdopcion>(dto);
            int id = await _servicio.CrearAsync(solicitud);
            solicitud.IdSolicitudAdopcion = id;

            var respuesta = _mapper.Map<SolicitudAdopcionDto>(solicitud);
            return CreatedAtAction(nameof(GetById), new { id }, respuesta);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] SolicitudAdopcionActualizarDto dto)
    {
        if (id != dto.IdSolicitudAdopcion)
            return BadRequest(new { mensaje = "El ID de la ruta no coincide con el del cuerpo." });

        var validacion = await _validadorActualizar.ValidateAsync(dto);
        if (!validacion.IsValid)
        {
            var errores = validacion.Errors.Select(e => e.ErrorMessage).ToList();
            return BadRequest(new { mensaje = "Error de validación", errores });
        }

        try
        {
            var solicitud = _mapper.Map<SolicitudAdopcion>(dto);
            bool actualizado = await _servicio.ActualizarAsync(solicitud);
            if (!actualizado)
                return NotFound(new { mensaje = $"No se pudo actualizar la solicitud con ID {id}." });

            return Ok(_mapper.Map<SolicitudAdopcionDto>(solicitud));
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
                return NotFound(new { mensaje = $"No se encontró la solicitud con ID {id}." });

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