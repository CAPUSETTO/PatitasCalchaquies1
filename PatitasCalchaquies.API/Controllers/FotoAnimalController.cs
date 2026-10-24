using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using PatitasCalchaquies.API.DTOs;
using PatitasCalchaquies.Core.Entities;
using PatitasCalchaquies.Core.Services;

namespace PatitasCalchaquies.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FotosAnimalesController : ControllerBase
{
    private readonly ServicioFotoAnimal _servicio;
    private readonly IValidator<FotoAnimalCrearDto> _validadorCrear;
    private readonly IValidator<FotoAnimalActualizarDto> _validadorActualizar;
    private readonly IMapper _mapper;

    public FotosAnimalesController(
        ServicioFotoAnimal servicio,
        IValidator<FotoAnimalCrearDto> validadorCrear,
        IValidator<FotoAnimalActualizarDto> validadorActualizar,
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
        var fotos = await _servicio.ObtenerTodasAsync();
        return Ok(_mapper.Map<IEnumerable<FotoAnimalDto>>(fotos));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var foto = await _servicio.ObtenerPorIdAsync(id);
        if (foto == null)
            return NotFound(new { mensaje = $"No se encontró la foto con ID {id}." });

        return Ok(_mapper.Map<FotoAnimalDto>(foto));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] FotoAnimalCrearDto dto)
    {
        var validacion = await _validadorCrear.ValidateAsync(dto);
        if (!validacion.IsValid)
        {
            var errores = validacion.Errors.Select(e => e.ErrorMessage).ToList();
            return BadRequest(new { mensaje = "Error de validación", errores });
        }

        try
        {
            var foto = _mapper.Map<FotoAnimal>(dto);
            int id = await _servicio.CrearAsync(foto);
            foto.IdFotoAnimal = id;

            var respuesta = _mapper.Map<FotoAnimalDto>(foto);
            return CreatedAtAction(nameof(GetById), new { id }, respuesta);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] FotoAnimalActualizarDto dto)
    {
        if (id != dto.IdFotoAnimal)
            return BadRequest(new { mensaje = "El ID de la ruta no coincide con el del cuerpo." });

        var validacion = await _validadorActualizar.ValidateAsync(dto);
        if (!validacion.IsValid)
        {
            var errores = validacion.Errors.Select(e => e.ErrorMessage).ToList();
            return BadRequest(new { mensaje = "Error de validación", errores });
        }

        try
        {
            var foto = _mapper.Map<FotoAnimal>(dto);
            bool actualizado = await _servicio.ActualizarAsync(foto);
            if (!actualizado)
                return NotFound(new { mensaje = $"No se pudo actualizar la foto con ID {id}." });

            return Ok(_mapper.Map<FotoAnimalDto>(foto));
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
                return NotFound(new { mensaje = $"No se encontró la foto con ID {id}." });

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