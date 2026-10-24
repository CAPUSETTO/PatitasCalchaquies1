using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using PatitasCalchaquies.API.DTOs;
using PatitasCalchaquies.Core.Entities;
using PatitasCalchaquies.Core.Services;

namespace PatitasCalchaquies.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class IngresosAnimalesController : ControllerBase
{
    private readonly ServicioIngresoAnimal _servicio;
    private readonly IValidator<IngresoAnimalCrearDto> _validadorCrear;
    private readonly IValidator<IngresoAnimalActualizarDto> _validadorActualizar;
    private readonly IMapper _mapper;

    public IngresosAnimalesController(
        ServicioIngresoAnimal servicio,
        IValidator<IngresoAnimalCrearDto> validadorCrear,
        IValidator<IngresoAnimalActualizarDto> validadorActualizar,
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
        var ingresos = await _servicio.ObtenerTodosAsync();
        return Ok(_mapper.Map<IEnumerable<IngresoAnimalDto>>(ingresos));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var ingreso = await _servicio.ObtenerPorIdAsync(id);
        if (ingreso == null)
            return NotFound(new { mensaje = $"No se encontró el ingreso con ID {id}." });

        return Ok(_mapper.Map<IngresoAnimalDto>(ingreso));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] IngresoAnimalCrearDto dto)
    {
        var validacion = await _validadorCrear.ValidateAsync(dto);
        if (!validacion.IsValid)
        {
            var errores = validacion.Errors.Select(e => e.ErrorMessage).ToList();
            return BadRequest(new { mensaje = "Error de validación", errores });
        }

        try
        {
            var ingreso = _mapper.Map<IngresoAnimal>(dto);
            int id = await _servicio.CrearAsync(ingreso);
            ingreso.IdIngresoAnimal = id;

            var respuesta = _mapper.Map<IngresoAnimalDto>(ingreso);
            return CreatedAtAction(nameof(GetById), new { id }, respuesta);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] IngresoAnimalActualizarDto dto)
    {
        if (id != dto.IdIngresoAnimal)
            return BadRequest(new { mensaje = "El ID de la ruta no coincide con el del cuerpo." });

        var validacion = await _validadorActualizar.ValidateAsync(dto);
        if (!validacion.IsValid)
        {
            var errores = validacion.Errors.Select(e => e.ErrorMessage).ToList();
            return BadRequest(new { mensaje = "Error de validación", errores });
        }

        try
        {
            var ingreso = _mapper.Map<IngresoAnimal>(dto);
            bool actualizado = await _servicio.ActualizarAsync(ingreso);
            if (!actualizado)
                return NotFound(new { mensaje = $"No se pudo actualizar el ingreso con ID {id}." });

            return Ok(_mapper.Map<IngresoAnimalDto>(ingreso));
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
                return NotFound(new { mensaje = $"No se encontró el ingreso con ID {id}." });

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