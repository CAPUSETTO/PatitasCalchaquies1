using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using PatitasCalchaquies.API.DTOs;
using PatitasCalchaquies.Core.Entities;
using PatitasCalchaquies.Core.Services;

namespace PatitasCalchaquies.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RolesController : ControllerBase
{
    private readonly ServicioRol _servicio;
    private readonly IValidator<RolCrearDto> _validadorCrear;
    private readonly IValidator<RolActualizarDto> _validadorActualizar;
    private readonly IMapper _mapper;

    public RolesController(
        ServicioRol servicio,
        IValidator<RolCrearDto> validadorCrear,
        IValidator<RolActualizarDto> validadorActualizar,
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
        var roles = await _servicio.ObtenerTodosAsync();
        return Ok(_mapper.Map<IEnumerable<RolDto>>(roles));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var rol = await _servicio.ObtenerPorIdAsync(id);
        if (rol == null)
            return NotFound(new { mensaje = $"No se encontró el rol con ID {id}." });

        return Ok(_mapper.Map<RolDto>(rol));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] RolCrearDto dto)
    {
        var validacion = await _validadorCrear.ValidateAsync(dto);
        if (!validacion.IsValid)
        {
            var errores = validacion.Errors.Select(e => e.ErrorMessage).ToList();
            return BadRequest(new { mensaje = "Error de validación", errores });
        }

        try
        {
            var rol = _mapper.Map<Rol>(dto);
            int id = await _servicio.CrearAsync(rol);
            rol.IdRol = id;

            var respuesta = _mapper.Map<RolDto>(rol);
            return CreatedAtAction(nameof(GetById), new { id }, respuesta);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] RolActualizarDto dto)
    {
        if (id != dto.IdRol)
            return BadRequest(new { mensaje = "El ID de la ruta no coincide con el del cuerpo." });

        var validacion = await _validadorActualizar.ValidateAsync(dto);
        if (!validacion.IsValid)
        {
            var errores = validacion.Errors.Select(e => e.ErrorMessage).ToList();
            return BadRequest(new { mensaje = "Error de validación", errores });
        }

        try
        {
            var rol = _mapper.Map<Rol>(dto);
            bool actualizado = await _servicio.ActualizarAsync(rol);
            if (!actualizado)
                return NotFound(new { mensaje = $"No se pudo actualizar el rol con ID {id}." });

            return Ok(_mapper.Map<RolDto>(rol));
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
                return NotFound(new { mensaje = $"No se encontró el rol con ID {id}." });

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