using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using PatitasCalchaquies.API.DTOs;
using PatitasCalchaquies.Core.Entities;
using PatitasCalchaquies.Core.Services;

namespace PatitasCalchaquies.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsuariosController : ControllerBase
{
    private readonly ServicioUsuario _servicio;
    private readonly IValidator<UsuarioCrearDto> _validadorCrear;
    private readonly IValidator<UsuarioActualizarDto> _validadorActualizar;
    private readonly IMapper _mapper;

    public UsuariosController(
        ServicioUsuario servicio,
        IValidator<UsuarioCrearDto> validadorCrear,
        IValidator<UsuarioActualizarDto> validadorActualizar,
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
        var usuarios = await _servicio.ObtenerTodosAsync();
        return Ok(_mapper.Map<IEnumerable<UsuarioDto>>(usuarios));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var usuario = await _servicio.ObtenerPorIdAsync(id);
        if (usuario == null)
            return NotFound(new { mensaje = $"No se encontró el usuario con ID {id}." });

        return Ok(_mapper.Map<UsuarioDto>(usuario));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] UsuarioCrearDto dto)
    {
        var validacion = await _validadorCrear.ValidateAsync(dto);
        if (!validacion.IsValid)
        {
            var errores = validacion.Errors.Select(e => e.ErrorMessage).ToList();
            return BadRequest(new { mensaje = "Error de validación", errores });
        }

        try
        {
            var usuario = _mapper.Map<Usuario>(dto);
            int id = await _servicio.CrearAsync(usuario);
            usuario.IdUsuario = id;

            var respuesta = _mapper.Map<UsuarioDto>(usuario);
            return CreatedAtAction(nameof(GetById), new { id }, respuesta);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UsuarioActualizarDto dto)
    {
        if (id != dto.IdUsuario)
            return BadRequest(new { mensaje = "El ID de la ruta no coincide con el del cuerpo." });

        var validacion = await _validadorActualizar.ValidateAsync(dto);
        if (!validacion.IsValid)
        {
            var errores = validacion.Errors.Select(e => e.ErrorMessage).ToList();
            return BadRequest(new { mensaje = "Error de validación", errores });
        }

        try
        {
            var usuario = _mapper.Map<Usuario>(dto);
            bool actualizado = await _servicio.ActualizarAsync(usuario);
            if (!actualizado)
                return NotFound(new { mensaje = $"No se pudo actualizar el usuario con ID {id}." });

            return Ok(_mapper.Map<UsuarioDto>(usuario));
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
                return NotFound(new { mensaje = $"No se encontró el usuario con ID {id}." });

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