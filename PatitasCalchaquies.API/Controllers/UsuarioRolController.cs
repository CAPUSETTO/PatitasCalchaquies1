using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using PatitasCalchaquies.API.DTOs;
using PatitasCalchaquies.Core.Entities;
using PatitasCalchaquies.Core.Services;

namespace PatitasCalchaquies.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsuariosRolesController : ControllerBase
{
    private readonly ServicioUsuarioRol _servicio;
    private readonly IValidator<UsuarioRolCrearDto> _validadorCrear;
    private readonly IMapper _mapper;

    public UsuariosRolesController(
        ServicioUsuarioRol servicio,
        IValidator<UsuarioRolCrearDto> validadorCrear,
        IMapper mapper)
    {
        _servicio = servicio;
        _validadorCrear = validadorCrear;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var lista = await _servicio.ObtenerTodosAsync();
        return Ok(_mapper.Map<IEnumerable<UsuarioRolDto>>(lista));
    }

    [HttpGet("{idUsuario:int}/{idRol:int}")]
    public async Task<IActionResult> GetByClave(int idUsuario, int idRol)
    {
        var asignacion = await _servicio.ObtenerPorClaveAsync(idUsuario, idRol);
        if (asignacion == null)
            return NotFound(new { mensaje = $"No se encontró la asignación para el usuario {idUsuario} y rol {idRol}." });

        return Ok(_mapper.Map<UsuarioRolDto>(asignacion));
    }

    [HttpGet("usuario/{idUsuario:int}")]
    public async Task<IActionResult> GetByUsuario(int idUsuario)
    {
        var roles = await _servicio.ObtenerPorUsuarioAsync(idUsuario);
        return Ok(_mapper.Map<IEnumerable<UsuarioRolDto>>(roles));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] UsuarioRolCrearDto dto)
    {
        var validacion = await _validadorCrear.ValidateAsync(dto);
        if (!validacion.IsValid)
        {
            var errores = validacion.Errors.Select(e => e.ErrorMessage).ToList();
            return BadRequest(new { mensaje = "Error de validación", errores });
        }

        try
        {
            var entidad = _mapper.Map<UsuarioRol>(dto);
            bool creado = await _servicio.CrearAsync(entidad);

            if (!creado)
                return BadRequest(new { mensaje = "No se pudo registrar la asignación del rol." });

            var respuesta = _mapper.Map<UsuarioRolDto>(entidad);
            return CreatedAtAction(
                nameof(GetByClave), 
                new { idUsuario = entidad.IdUsuario, idRol = entidad.IdRol }, 
                respuesta
            );
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { error = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpDelete("{idUsuario:int}/{idRol:int}")]
    public async Task<IActionResult> Delete(int idUsuario, int idRol)
    {
        try
        {
            bool eliminado = await _servicio.EliminarAsync(idUsuario, idRol);
            if (!eliminado)
                return NotFound(new { mensaje = $"No se encontró la asignación para el usuario {idUsuario} y rol {idRol}." });

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