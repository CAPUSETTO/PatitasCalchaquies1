using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using PatitasCalchaquies.API.DTOs;
using PatitasCalchaquies.Core.Entities;
using PatitasCalchaquies.Core.Services;

namespace PatitasCalchaquies.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AtencionesVeterinariasController : ControllerBase
{
    private readonly ServicioAtencionVeterinaria _servicio;
    private readonly IValidator<AtencionVeterinariaCrearDto> _validadorCrear;
    private readonly IValidator<AtencionVeterinarioActualizarDto> _validadorActualizar;
    private readonly IMapper _mapper;

    public AtencionesVeterinariasController(
        ServicioAtencionVeterinaria servicio,
        IValidator<AtencionVeterinariaCrearDto> validadorCrear,
        IValidator<AtencionVeterinarioActualizarDto> validadorActualizar,
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
        var atenciones = await _servicio.ObtenerTodasAsync();
        return Ok(_mapper.Map<IEnumerable<AtencionVeterinariaDto>>(atenciones));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var atencion = await _servicio.ObtenerPorIdAsync(id);
        if (atencion == null)
            return NotFound(new { mensaje = $"No se encontró la atención veterinaria con ID {id}." });

        return Ok(_mapper.Map<AtencionVeterinariaDto>(atencion));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] AtencionVeterinariaCrearDto dto)
    {
        var validacion = await _validadorCrear.ValidateAsync(dto);
        if (!validacion.IsValid)
        {
            var errores = validacion.Errors.Select(e => e.ErrorMessage).ToList();
            return BadRequest(new { mensaje = "Error de validación", errores });
        }

        try
        {
            var atencion = _mapper.Map<AtencionVeterinaria>(dto);
            int id = await _servicio.CrearAsync(atencion);
            atencion.IdAtencionVeterinaria = id;

            var respuesta = _mapper.Map<AtencionVeterinariaDto>(atencion);
            return CreatedAtAction(nameof(GetById), new { id }, respuesta);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] AtencionVeterinarioActualizarDto dto)
    {
        if (id != dto.IdAtencionVeterinaria)
            return BadRequest(new { mensaje = "El ID de la ruta no coincide con el del cuerpo." });

        var validacion = await _validadorActualizar.ValidateAsync(dto);
        if (!validacion.IsValid)
        {
            var errores = validacion.Errors.Select(e => e.ErrorMessage).ToList();
            return BadRequest(new { mensaje = "Error de validación", errores });
        }

        try
        {
            var atencion = _mapper.Map<AtencionVeterinaria>(dto);
            bool actualizado = await _servicio.ActualizarAsync(atencion);
            if (!actualizado)
                return NotFound(new { mensaje = $"No se pudo actualizar la atención con ID {id}." });

            return Ok(_mapper.Map<AtencionVeterinariaDto>(atencion));
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
                return NotFound(new { mensaje = $"No se encontró la atención con ID {id}." });

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