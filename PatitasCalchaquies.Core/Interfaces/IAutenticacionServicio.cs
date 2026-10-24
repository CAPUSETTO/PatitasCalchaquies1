using PatitasCalchaquies.Core.Entities;

namespace PatitasCalchaquies.Core.Interfaces;

public interface IAutenticacionServicio
{
    Task<Usuario?> ValidarCredencialesAsync(string usuario, string password_);
    string GenerarToken(Usuario usuario, List<string> roles_);
}