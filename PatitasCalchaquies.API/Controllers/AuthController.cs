using Microsoft.AspNetCore.Mvc;
using PatitasCalchaquies.API.DTOs;
using PatitasCalchaquies.Core.Interfaces;

namespace PatitasCalchaquies.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAutenticacionServicio _authService;
    private readonly IRepositorioUsuario _repoUsuario;

    public AuthController(IAutenticacionServicio authService, IRepositorioUsuario repoUsuario)
    {
        _authService = authService;
        _repoUsuario = repoUsuario;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto dto)
    {
        var usuario = await _authService.ValidarCredencialesAsync(dto.Usuario, dto.Password);

        if (usuario == null)
            return Unauthorized(new { mensaje = "Usuario o contraseña incorrectos, o cuenta inactiva." });

        await _repoUsuario.ActualizarUltimoAccesoAsync(usuario.IdUsuario, DateTime.UtcNow);

        var rolesEntidades = await _repoUsuario.ObtenerRolesAsync(usuario.IdUsuario);
        var roles = rolesEntidades.Select(r => r.Nombre).ToList();
        if (roles.Count == 0)
        {
            roles.Add("Voluntario");
        }

        var token = _authService.GenerarToken(usuario, roles);
        var operadorDetallado = await _repoUsuario.ObtenerOperadorDetalladoPorIdAsync(usuario.IdUsuario);
        var operadorDto = UsuariosController.MapearOperadorDto(operadorDetallado ?? usuario);

        return Ok(new
        {
            Token = token,
            Operador = operadorDto
        });
    }
}
