using PatitasCalchaquies.Core.Entities;
using PatitasCalchaquies.Core.Interfaces;
using PatitasCalchaquies.Core.Services;

namespace PatitasCalchaquies.API.Seed;

/// <summary>
/// Crea al iniciar la API un usuario con rol Administrador (superusuario)
/// si todavía no existe, para poder probar el login y las rutas protegidas.
/// Credenciales en configuración: AdminInicial:Usuario y AdminInicial:Password.
/// </summary>
public static class SemillaAdministrador
{
    public static async Task EjecutarAsync(IServiceProvider servicios)
    {
        using var scope = servicios.CreateScope();
        var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();
        var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("SemillaAdministrador");

        var usuario = config["AdminInicial:Usuario"] ?? "admin";
        var password = config["AdminInicial:Password"];
        if (string.IsNullOrWhiteSpace(password))
        {
            logger.LogWarning("AdminInicial:Password no está configurado: no se crea el administrador inicial.");
            return;
        }

        var repoUsuarios = scope.ServiceProvider.GetRequiredService<IRepositorioUsuario>();
        var repoRoles = scope.ServiceProvider.GetRequiredService<IRepositorioRol>();

        if (await repoUsuarios.ObtenerPorUsuarioAsync(usuario) != null)
            return;

        var roles = await repoRoles.ObtenerTodosAsync();
        var rolAdmin = roles.FirstOrDefault(r => r.Nombre.Equals("Administrador", StringComparison.OrdinalIgnoreCase));
        int idRol = rolAdmin?.IdRol
            ?? await repoRoles.CrearAsync(new Rol { Nombre = "Administrador", Descripcion = "Acceso total al sistema" });
        if (idRol <= 0)
        {
            logger.LogError("No se pudo obtener ni crear el rol Administrador.");
            return;
        }

        var idUsuario = await repoUsuarios.CrearAsync(new Usuario
        {
            User = usuario,
            PasswordHash = ServicioPassword.HashPassword(password),
            Activo = true,
            FechaCreacion = DateTime.UtcNow,
            Nombre = "Administrador",
            Apellido = "Sistema",
            EtiquetaArea = "Coordinación General"
        });
        if (idUsuario <= 0)
        {
            logger.LogError("No se pudo crear el usuario administrador inicial.");
            return;
        }

        await repoUsuarios.AsignarRolUnicoAsync(idUsuario, idRol);
        logger.LogInformation("Administrador inicial '{Usuario}' creado.", usuario);
    }
}
