using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using PatitasCalchaquies.API.DTOs;
using PatitasCalchaquies.Core.Entities;
using PatitasCalchaquies.Core.Services;

namespace PatitasCalchaquies.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VacunacionesController : ControllerBase
{
    private readonly ServicioVacunacion _servicio;
    private readonly IValidator<VacunacionCrearDto> _validadorCrear;
    private readonly IValidator<VacunacionActualizarDto> _validadorActualizar;
    private readonly IMapper _mapper;

    public VacunacionesController(
        ServicioVacunacion servicio,
        IValidator<VacunacionCrearDto> validadorCrear,
        IValidator<VacunacionActualizarDto> validadorActualizar,
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
        var vacunaciones = await _servicio.ObtenerTodasAsync();
        return Ok(_mapper.Map<IEnumerable<VacunacionDto>>(vacunaciones));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var vacunacion = await _servicio.ObtenerPorIdAsync(id);
        if (vacunacion == null)
            return NotFound(new { mensaje = $"No se encontró la vacunación con ID {id}." });

        return Ok(_mapper.Map<VacunacionDto>(vacunacion));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] VacunacionCrearDto dto)
    {
        var validacion = await _validadorCrear.ValidateAsync(dto);
        if (!validacion.IsValid)
        {
            var errores = validacion.Errors.Select(e => e.ErrorMessage).ToList();
            return BadRequest(new { mensaje = "Error de validación", errores });
        }

        try
        {
            var vacunacion = _mapper.Map<Vacunacion>(dto);
            int id = await _servicio.CrearAsync(vacunacion);
            vacunacion.IdVacunacion = id;

            var respuesta = _mapper.Map<VacunacionDto>(vacunacion);
            return CreatedAtAction(nameof(GetById), new { id }, respuesta);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] VacunacionActualizarDto dto)
    {
        if (id != dto.IdVacunacion)
            return BadRequest(new { mensaje = "El ID de la ruta no coincide con el del cuerpo." });

        var validacion = await _validadorActualizar.ValidateAsync(dto);
        if (!validacion.IsValid)
        {
            var errores = validacion.Errors.Select(e => e.ErrorMessage).ToList();
            return BadRequest(new { mensaje = "Error de validación", errores });
        }

        try
        {
            var vacunacion = _mapper.Map<Vacunacion>(dto);
            bool actualizado = await _servicio.ActualizarAsync(vacunacion);
            if (!actualizado)
                return NotFound(new { mensaje = $"No se pudo actualizar la vacunación con ID {id}." });

            return Ok(_mapper.Map<VacunacionDto>(vacunacion));
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
                return NotFound(new { mensaje = $"No se encontró la vacunación con ID {id}." });

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