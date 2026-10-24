using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using PatitasCalchaquies.API.DTOs;
using PatitasCalchaquies.Core.Entities;
using PatitasCalchaquies.Core.Services;

namespace PatitasCalchaquies.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EspeciesController : ControllerBase
{
    private readonly ServicioEspecie _servicio;
    private readonly IValidator<EspecieCrearDto> _validadorCrear;
    private readonly IValidator<EspecieActualizarDto> _validadorActualizar;
    private readonly IMapper _mapper;

    public EspeciesController(
        ServicioEspecie servicio,
        IValidator<EspecieCrearDto> validadorCrear,
        IValidator<EspecieActualizarDto> validadorActualizar,
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
        var especies = await _servicio.ObtenerTodasAsync();
        return Ok(_mapper.Map<IEnumerable<EspecieDto>>(especies));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var especie = await _servicio.ObtenerPorIdAsync(id);
        if (especie == null)
            return NotFound(new { mensaje = $"No se encontró la especie con ID {id}." });

        return Ok(_mapper.Map<EspecieDto>(especie));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] EspecieCrearDto dto)
    {
        var validacion = await _validadorCrear.ValidateAsync(dto);
        if (!validacion.IsValid)
        {
            var errores = validacion.Errors.Select(e => e.ErrorMessage).ToList();
            return BadRequest(new { mensaje = "Error de validación", errores });
        }

        try
        {
            var especie = _mapper.Map<Especie>(dto);
            int id = await _servicio.CrearAsync(especie);
            especie.IdEspecie = id;

            var respuesta = _mapper.Map<EspecieDto>(especie);
            return CreatedAtAction(nameof(GetById), new { id }, respuesta);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] EspecieActualizarDto dto)
    {
        if (id != dto.IdEspecie)
            return BadRequest(new { mensaje = "El ID de la ruta no coincide con el del cuerpo." });

        var validacion = await _validadorActualizar.ValidateAsync(dto);
        if (!validacion.IsValid)
        {
            var errores = validacion.Errors.Select(e => e.ErrorMessage).ToList();
            return BadRequest(new { mensaje = "Error de validación", errores });
        }

        try
        {
            var especie = _mapper.Map<Especie>(dto);
            bool actualizado = await _servicio.ActualizarAsync(especie);
            if (!actualizado)
                return NotFound(new { mensaje = $"No se pudo actualizar la especie con ID {id}." });

            return Ok(_mapper.Map<EspecieDto>(especie));
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
                return NotFound(new { mensaje = $"No se encontró la especie con ID {id}." });

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