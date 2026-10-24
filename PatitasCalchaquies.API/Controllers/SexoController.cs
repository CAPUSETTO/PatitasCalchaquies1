using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using PatitasCalchaquies.API.DTOs;
using PatitasCalchaquies.Core.Entities;
using PatitasCalchaquies.Core.Services;

namespace PatitasCalchaquies.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SexosController : ControllerBase
{
    private readonly ServicioSexo _servicio;
    private readonly IValidator<SexoCrearDto> _validadorCrear;
    private readonly IValidator<SexoActualizarDto> _validadorActualizar;
    private readonly IMapper _mapper;

    public SexosController(
        ServicioSexo servicio,
        IValidator<SexoCrearDto> validadorCrear,
        IValidator<SexoActualizarDto> validadorActualizar,
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
        var sexos = await _servicio.ObtenerTodosAsync();
        return Ok(_mapper.Map<IEnumerable<SexoDto>>(sexos));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var sexo = await _servicio.ObtenerPorIdAsync(id);
        if (sexo == null)
            return NotFound(new { mensaje = $"No se encontró el sexo con ID {id}." });

        return Ok(_mapper.Map<SexoDto>(sexo));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] SexoCrearDto dto)
    {
        var validacion = await _validadorCrear.ValidateAsync(dto);
        if (!validacion.IsValid)
        {
            var errores = validacion.Errors.Select(e => e.ErrorMessage).ToList();
            return BadRequest(new { mensaje = "Error de validación", errores });
        }

        try
        {
            var sexo = _mapper.Map<Sexo>(dto);
            int id = await _servicio.CrearAsync(sexo);
            sexo.IdSexo = id;

            var respuesta = _mapper.Map<SexoDto>(sexo);
            return CreatedAtAction(nameof(GetById), new { id }, respuesta);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] SexoActualizarDto dto)
    {
        if (id != dto.IdSexo)
            return BadRequest(new { mensaje = "El ID de la ruta no coincide con el del cuerpo." });

        var validacion = await _validadorActualizar.ValidateAsync(dto);
        if (!validacion.IsValid)
        {
            var errores = validacion.Errors.Select(e => e.ErrorMessage).ToList();
            return BadRequest(new { mensaje = "Error de validación", errores });
        }

        try
        {
            var sexo = _mapper.Map<Sexo>(dto);
            bool actualizado = await _servicio.ActualizarAsync(sexo);
            if (!actualizado)
                return NotFound(new { mensaje = $"No se pudo actualizar el sexo con ID {id}." });

            return Ok(_mapper.Map<SexoDto>(sexo));
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
                return NotFound(new { mensaje = $"No se encontró el sexo con ID {id}." });

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