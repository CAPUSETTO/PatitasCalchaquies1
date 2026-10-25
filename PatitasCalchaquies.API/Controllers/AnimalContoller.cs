using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using PatitasCalchaquies.API.DTOs;
using PatitasCalchaquies.Core.Entities;
using PatitasCalchaquies.Core.Services;

namespace PatitasCalchaquies.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AnimalesController : ControllerBase
{
    private readonly ServicioAnimal _servicio;
    private readonly IValidator<AnimalCrearDto> _validadorCrear;
    private readonly IValidator<AnimalActualizarDto> _validadorActualizar;
    private readonly IMapper _mapper;

    public AnimalesController(
        ServicioAnimal servicio,
        IValidator<AnimalCrearDto> validadorCrear,
        IValidator<AnimalActualizarDto> validadorActualizar,
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
        var animales = await _servicio.ObtenerTodosAsync();
        var dtos = _mapper.Map<IEnumerable<AnimalDto>>(animales);
        return Ok(dtos);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var animal = await _servicio.ObtenerPorIdAsync(id);
        if (animal == null)
            return NotFound(new { mensaje = $"No se encontró el animal con ID {id}." });

        return Ok(_mapper.Map<AnimalDto>(animal));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] AnimalCrearDto dto)
    {
        var validacion = await _validadorCrear.ValidateAsync(dto);
        if (!validacion.IsValid)
        {
            var errores = validacion.Errors.Select(e => e.ErrorMessage).ToList();
            return BadRequest(new { mensaje = "Error de validación", errores });
        }

        try
        {
            var animal = _mapper.Map<Animal>(dto);
            int id = await _servicio.CrearAsync(animal);
            animal.IdAnimal = id;

            var respuesta = _mapper.Map<AnimalDto>(animal);
            return CreatedAtAction(nameof(GetById), new { id }, respuesta);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] AnimalActualizarDto dto)
    {
        if (id != dto.IdAnimal)
            return BadRequest(new { mensaje = "El ID de la ruta no coincide con el del cuerpo." });

        var validacion = await _validadorActualizar.ValidateAsync(dto);
        if (!validacion.IsValid)
        {
            var errores = validacion.Errors.Select(e => e.ErrorMessage).ToList();
            return BadRequest(new { mensaje = "Error de validación", errores });
        }

        try
        {
            var animal = _mapper.Map<Animal>(dto);
            bool actualizado = await _servicio.ActualizarAsync(animal);
            if (!actualizado)
                return NotFound(new { mensaje = $"No se pudo actualizar el animal con ID {id}." });

            return Ok(_mapper.Map<AnimalDto>(animal));
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
                return NotFound(new { mensaje = $"No se encontró el animal con ID {id}." });

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
