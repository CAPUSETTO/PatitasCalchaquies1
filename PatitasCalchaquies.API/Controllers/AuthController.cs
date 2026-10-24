using Microsoft.AspNetCore.Mvc;
using PatitasCalchaquies.API.DTOs;
using PatitasCalchaquies.Core.Interfaces;

namespace PatitasCalchaquies.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAutenticacionServicio _authService;
    private readonly IRepositorioUsuario _repoUsuario; // Para buscar roles si es necesario

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

        // Obtener los nombres de los roles (requiere que implementes ObtenerRolesAsync en tu repo)
        var rolesEntidades = await _repoUsuario.ObtenerRolesAsync(usuario.IdUsuario);
        var roles = rolesEntidades.Select(r => r.Nombre).ToList();

        var token = _authService.GenerarToken(usuario, roles);

        return Ok(new { Token = token });
    }
}