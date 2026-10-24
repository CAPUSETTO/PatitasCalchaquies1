using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PatitasCalchaquies.Core.Entities;
using PatitasCalchaquies.Core.Interfaces;

namespace PatitasCalchaquies.Core.Services;

public class AutenticacionService : IAutenticacionServicio
{
    private readonly IRepositorioUsuario _repositorio;
    private readonly IConfiguration _configuration;

    public AutenticacionService(IRepositorioUsuario repositorio, IConfiguration configuration)
    {
        _repositorio = repositorio;
        _configuration = configuration;
    }

    public async Task<Usuario?> ValidarCredencialesAsync(string usuario, string password_)
    {
        // Nota: Asegúrate de tener el método ObtenerPorUsuarioAsync en tu IRepositorioUsuario
        var user = await _repositorio.ObtenerPorUsuarioAsync(usuario);
        if (user == null || !user.Activo) return null;

        if (!ServicioPassword.VerificarPassword(password_, user.PasswordHash)) return null;

        return user;
    }

    public string GenerarToken(Usuario usuario, List<string> roles_)
    {
        var llave_secreta = _configuration["Jwt:llave_secreta"]!;
        var emisor = _configuration["Jwt:emisor"];
        var audiencia = _configuration["Jwt:audiencia"];
        var tiempo_expiracion_minutos = int.Parse(_configuration["Jwt:tiempo_expiracion_minutos"] ?? "60");

        var claveSeguridad = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(llave_secreta));
        var credenciales = new SigningCredentials(claveSeguridad, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, usuario.IdUsuario.ToString()),
            new Claim(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString()),
            new Claim(ClaimTypes.Name, usuario.User)
        };

        if (roles_ != null)
        {
            foreach (var rol in roles_)
            {
                claims.Add(new Claim(ClaimTypes.Role, rol));
            }
        }

        var token = new JwtSecurityToken(
            issuer: emisor,
            audience: audiencia,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(tiempo_expiracion_minutos),
            signingCredentials: credenciales
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}