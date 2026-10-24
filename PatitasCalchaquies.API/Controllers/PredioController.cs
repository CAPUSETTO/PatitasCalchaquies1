using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using PatitasCalchaquies.API.DTOs;
using PatitasCalchaquies.Core.Entities;
using PatitasCalchaquies.Core.Services;

namespace PatitasCalchaquies.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PrediosController : ControllerBase
{
    private readonly ServicioPredio _servicio;
    private readonly IValidator<PredioCrearDto> _validadorCrear;
    private readonly IValidator<PredioActualizarDto> _validadorActualizar;
    private readonly IMapper _mapper;

    public PrediosController(
        ServicioPredio servicio,
        IValidator<PredioCrearDto> validadorCrear,
        IValidator<PredioActualizarDto> validadorActualizar,
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
        var predios = await _servicio.ObtenerTodosAsync();
        return Ok(_mapper.Map<IEnumerable<PredioDto>>(predios));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var predio = await _servicio.ObtenerPorIdAsync(id);
        if (predio == null)
            return NotFound(new { mensaje = $"No se encontró el predio con ID {id}." });

        return Ok(_mapper.Map<PredioDto>(predio));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] PredioCrearDto dto)
    {
        var validacion = await _validadorCrear.ValidateAsync(dto);
        if (!validacion.IsValid)
        {
            var errores = validacion.Errors.Select(e => e.ErrorMessage).ToList();
            return BadRequest(new { mensaje = "Error de validación", errores });
        }

        try
        {
            var predio = _mapper.Map<Predio>(dto);
            int id = await _servicio.CrearAsync(predio);
            predio.IdPredio = id;

            var respuesta = _mapper.Map<PredioDto>(predio);
            return CreatedAtAction(nameof(GetById), new { id }, respuesta);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] PredioActualizarDto dto)
    {
        if (id != dto.IdPredio)
            return BadRequest(new { mensaje = "El ID de la ruta no coincide con el del cuerpo." });

        var validacion = await _validadorActualizar.ValidateAsync(dto);
        if (!validacion.IsValid)
        {
            var errores = validacion.Errors.Select(e => e.ErrorMessage).ToList();
            return BadRequest(new { mensaje = "Error de validación", errores });
        }

        try
        {
            var predio = _mapper.Map<Predio>(dto);
            bool actualizado = await _servicio.ActualizarAsync(predio);
            if (!actualizado)
                return NotFound(new { mensaje = $"No se pudo actualizar el predio con ID {id}." });

            return Ok(_mapper.Map<PredioDto>(predio));
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
                return NotFound(new { mensaje = $"No se encontró el predio con ID {id}." });

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