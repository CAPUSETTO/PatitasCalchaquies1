using PatitasCalchaquies.Core.Entities;
using PatitasCalchaquies.Core.Interfaces;

namespace PatitasCalchaquies.Core.Services;

public class ServicioUsuario
{
    private readonly IRepositorioUsuario _repositorio;
    private readonly IRepositorioVeterinario _repositorioVeterinario;

    public ServicioUsuario(
        IRepositorioUsuario repositorio,
        IRepositorioVeterinario repositorioVeterinario)
    {
        _repositorio = repositorio;
        _repositorioVeterinario = repositorioVeterinario;
    }

    public async Task<IEnumerable<Usuario>> ObtenerTodosAsync()
    {
        return await _repositorio.ObtenerTodosAsync();
    }

    public async Task<Usuario?> ObtenerPorIdAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("El ID debe ser mayor a 0.");

        return await _repositorio.ObtenerPorIdAsync(id);
    }

    public async Task<IEnumerable<Usuario>> ObtenerOperadoresAsync()
    {
        return await _repositorio.ObtenerOperadoresDetalladosAsync();
    }

    public async Task<Usuario?> ObtenerOperadorPorIdAsync(int idUsuario)
    {
        if (idUsuario <= 0)
            throw new ArgumentException("El ID debe ser mayor a 0.");

        return await _repositorio.ObtenerOperadorDetalladoPorIdAsync(idUsuario);
    }

    public async Task<int> CrearAsync(Usuario usuario)
    {
        if (usuario == null)
            throw new ArgumentNullException(nameof(usuario));

        if (string.IsNullOrWhiteSpace(usuario.User))
            throw new ArgumentException("El nombre de usuario es obligatorio.");

        if (string.IsNullOrWhiteSpace(usuario.PasswordHash))
            throw new ArgumentException("La contraseña hash es obligatoria.");

        var existente = await _repositorio.ObtenerPorUsuarioAsync(usuario.User.Trim());
        if (existente != null)
            throw new InvalidOperationException($"El nombre de usuario '{usuario.User.Trim()}' ya se encuentra registrado.");

        if (!usuario.PasswordHash.StartsWith("$2"))
        {
            usuario.PasswordHash = ServicioPassword.HashPassword(usuario.PasswordHash);
        }

        int id = await _repositorio.CrearAsync(usuario);
        if (id <= 0)
            throw new InvalidOperationException("No se pudo crear el usuario en la base de datos.");

        return id;
    }

    public async Task<bool> ActualizarAsync(Usuario usuario)
    {
        if (usuario == null)
            throw new ArgumentNullException(nameof(usuario));

        if (usuario.IdUsuario <= 0)
            throw new ArgumentException("El ID del usuario no es válido.");

        if (string.IsNullOrWhiteSpace(usuario.User))
            throw new ArgumentException("El nombre de usuario es obligatorio.");

        var existente = await _repositorio.ObtenerPorIdAsync(usuario.IdUsuario);
        if (existente == null)
            throw new KeyNotFoundException($"No se encontró el usuario con ID {usuario.IdUsuario}.");

        if (string.IsNullOrWhiteSpace(usuario.PasswordHash))
        {
            usuario.PasswordHash = existente.PasswordHash;
        }
        else if (!usuario.PasswordHash.StartsWith("$2"))
        {
            usuario.PasswordHash = ServicioPassword.HashPassword(usuario.PasswordHash);
        }

        return await _repositorio.ActualizarAsync(usuario);
    }

    public async Task<Usuario> CrearOperadorAsync(
        string user,
        string password,
        int idRol,
        string nombre,
        string apellido,
        string? etiquetaArea,
        bool activo,
        string? matricula,
        string? telefono)
    {
        var cleanUser = (user ?? string.Empty).Trim().ToLowerInvariant();
        var cleanPassword = (password ?? string.Empty).Trim();
        var cleanNombre = (nombre ?? string.Empty).Trim();
        var cleanApellido = (apellido ?? string.Empty).Trim();

        if (cleanUser.Length < 3)
            throw new ArgumentException("El nombre de usuario debe tener al menos 3 caracteres.");

        if (cleanPassword.Length < 6)
            throw new ArgumentException("La contraseña debe tener al menos 6 caracteres por seguridad.");

        if (string.IsNullOrWhiteSpace(cleanNombre) || string.IsNullOrWhiteSpace(cleanApellido))
            throw new ArgumentException("El nombre y apellido del integrante son obligatorios.");

        if (idRol < 1 || idRol > 3)
            idRol = 2;

        var existente = await _repositorio.ObtenerPorUsuarioAsync(cleanUser);
        if (existente != null)
            throw new InvalidOperationException($"El nombre de usuario \"{cleanUser}\" ya se encuentra registrado.");

        int? idVeterinario = null;
        if (idRol == 2)
        {
            var cleanMatricula = string.IsNullOrWhiteSpace(matricula)
                ? $"MP-SAL-0{Random.Shared.Next(100, 999)}"
                : matricula.Trim();
            var cleanTelefono = string.IsNullOrWhiteSpace(telefono)
                ? "+54 3868 421900"
                : telefono.Trim();

            var vetId = await _repositorioVeterinario.CrearAsync(new Veterinario
            {
                Matricula = cleanMatricula.Length > 50 ? cleanMatricula[..50] : cleanMatricula,
                Nombre = cleanNombre.Length > 50 ? cleanNombre[..50] : cleanNombre,
                Apellido = cleanApellido.Length > 50 ? cleanApellido[..50] : cleanApellido,
                Telefono = cleanTelefono.Length > 20 ? cleanTelefono[..20] : cleanTelefono
            });

            if (vetId > 0)
            {
                idVeterinario = vetId;
            }
        }

        var areaResuelta = !string.IsNullOrWhiteSpace(etiquetaArea)
            ? etiquetaArea.Trim()
            : idRol == 1
                ? "Coordinación General"
                : idRol == 2
                    ? "Veterinaria Clínica"
                    : "Guardia y Adopciones";

        var nuevoUsuario = new Usuario
        {
            User = cleanUser,
            PasswordHash = ServicioPassword.HashPassword(cleanPassword),
            Activo = activo,
            FechaCreacion = DateTime.UtcNow,
            UltimoAcceso = null,
            Nombre = cleanNombre,
            Apellido = cleanApellido,
            EtiquetaArea = areaResuelta,
            IdVeterinario = idVeterinario
        };

        int idUsuario = await _repositorio.CrearAsync(nuevoUsuario);
        if (idUsuario <= 0)
            throw new InvalidOperationException("No se pudo registrar el operador en la base de datos.");

        await _repositorio.AsignarRolUnicoAsync(idUsuario, idRol);

        var creado = await _repositorio.ObtenerOperadorDetalladoPorIdAsync(idUsuario);
        return creado ?? nuevoUsuario;
    }

    public async Task<Usuario> ActualizarOperadorAsync(
        int idUsuario,
        string user,
        string? password,
        int idRol,
        string nombre,
        string apellido,
        string? etiquetaArea,
        bool activo,
        string? matricula,
        string? telefono)
    {
        if (idUsuario <= 0)
            throw new ArgumentException("El ID del operador no es válido.");

        var existente = await _repositorio.ObtenerOperadorDetalladoPorIdAsync(idUsuario);
        if (existente == null)
            throw new KeyNotFoundException($"No se encontró el operador con ID {idUsuario}.");

        var cleanUser = (user ?? string.Empty).Trim().ToLowerInvariant();
        var cleanNombre = (nombre ?? string.Empty).Trim();
        var cleanApellido = (apellido ?? string.Empty).Trim();

        if (cleanUser.Length < 3)
            throw new ArgumentException("El nombre de usuario debe tener al menos 3 caracteres.");

        if (string.IsNullOrWhiteSpace(cleanNombre) || string.IsNullOrWhiteSpace(cleanApellido))
            throw new ArgumentException("El nombre y apellido del integrante son obligatorios.");

        var otroConMismoUser = await _repositorio.ObtenerPorUsuarioAsync(cleanUser);
        if (otroConMismoUser != null && otroConMismoUser.IdUsuario != idUsuario)
            throw new InvalidOperationException($"El nombre de usuario \"{cleanUser}\" ya está en uso por otra cuenta.");

        if (idRol < 1 || idRol > 3)
            idRol = existente.IdRol ?? 2;

        // Verificar que siempre quede al menos un Administrador activo
        if (existente.IdRol == 1 && (idRol != 1 || !activo))
        {
            var todos = await _repositorio.ObtenerOperadoresDetalladosAsync();
            var adminsActivosRestantes = todos.Count(o => o.IdUsuario != idUsuario && o.Activo && o.IdRol == 1);
            if (adminsActivosRestantes == 0)
            {
                throw new InvalidOperationException(
                    "Debe permanecer al menos un Administrador activo en el sistema para no perder el control del refugio."
                );
            }
        }

        int? idVeterinario = existente.IdVeterinario;
        if (idRol == 2)
        {
            var cleanMatricula = !string.IsNullOrWhiteSpace(matricula)
                ? matricula.Trim()
                : !string.IsNullOrWhiteSpace(existente.VeterinarioMatricula)
                    ? existente.VeterinarioMatricula
                    : $"MP-SAL-0{Random.Shared.Next(100, 999)}";

            var cleanTelefono = !string.IsNullOrWhiteSpace(telefono)
                ? telefono.Trim()
                : !string.IsNullOrWhiteSpace(existente.VeterinarioTelefono)
                    ? existente.VeterinarioTelefono
                    : "+54 3868 421900";

            if (idVeterinario.HasValue && idVeterinario.Value > 0)
            {
                await _repositorioVeterinario.ActualizarAsync(new Veterinario
                {
                    IdVeterinario = idVeterinario.Value,
                    Matricula = cleanMatricula.Length > 50 ? cleanMatricula[..50] : cleanMatricula,
                    Nombre = cleanNombre.Length > 50 ? cleanNombre[..50] : cleanNombre,
                    Apellido = cleanApellido.Length > 50 ? cleanApellido[..50] : cleanApellido,
                    Telefono = cleanTelefono.Length > 20 ? cleanTelefono[..20] : cleanTelefono
                });
            }
            else
            {
                var nuevoVetId = await _repositorioVeterinario.CrearAsync(new Veterinario
                {
                    Matricula = cleanMatricula.Length > 50 ? cleanMatricula[..50] : cleanMatricula,
                    Nombre = cleanNombre.Length > 50 ? cleanNombre[..50] : cleanNombre,
                    Apellido = cleanApellido.Length > 50 ? cleanApellido[..50] : cleanApellido,
                    Telefono = cleanTelefono.Length > 20 ? cleanTelefono[..20] : cleanTelefono
                });
                if (nuevoVetId > 0)
                {
                    idVeterinario = nuevoVetId;
                }
            }
        }
        else
        {
            idVeterinario = null;
        }

        string passwordHashFinal = existente.PasswordHash;
        if (!string.IsNullOrWhiteSpace(password))
        {
            var cleanPass = password.Trim();
            if (cleanPass.Length < 6)
                throw new ArgumentException("La nueva contraseña debe tener al menos 6 caracteres.");

            passwordHashFinal = cleanPass.StartsWith("$2")
                ? cleanPass
                : ServicioPassword.HashPassword(cleanPass);
        }

        var areaResuelta = !string.IsNullOrWhiteSpace(etiquetaArea)
            ? etiquetaArea.Trim()
            : idRol == 1
                ? "Coordinación General"
                : idRol == 2
                    ? "Veterinaria Clínica"
                    : "Guardia y Adopciones";

        existente.User = cleanUser;
        existente.PasswordHash = passwordHashFinal;
        existente.Activo = activo;
        existente.Nombre = cleanNombre;
        existente.Apellido = cleanApellido;
        existente.EtiquetaArea = areaResuelta;
        existente.IdVeterinario = idVeterinario;

        await _repositorio.ActualizarAsync(existente);
        await _repositorio.AsignarRolUnicoAsync(idUsuario, idRol);

        var actualizado = await _repositorio.ObtenerOperadorDetalladoPorIdAsync(idUsuario);
        return actualizado ?? existente;
    }

    public async Task<Usuario> CambiarEstadoOperadorAsync(int idUsuario, bool activo, int? idUsuarioSolicitante = null)
    {
        if (idUsuario <= 0)
            throw new ArgumentException("El ID del operador no es válido.");

        if (!activo && idUsuarioSolicitante.HasValue && idUsuarioSolicitante.Value == idUsuario)
            throw new InvalidOperationException("No puedes suspender tu propia cuenta mientras tienes la sesión abierta.");

        var existente = await _repositorio.ObtenerOperadorDetalladoPorIdAsync(idUsuario);
        if (existente == null)
            throw new KeyNotFoundException($"No se encontró el operador con ID {idUsuario}.");

        if (!activo && existente.IdRol == 1)
        {
            var todos = await _repositorio.ObtenerOperadoresDetalladosAsync();
            var adminsActivosRestantes = todos.Count(o => o.IdUsuario != idUsuario && o.Activo && o.IdRol == 1);
            if (adminsActivosRestantes == 0)
            {
                throw new InvalidOperationException(
                    "Debe permanecer al menos un Administrador activo en el sistema para no perder el control del refugio."
                );
            }
        }

        existente.Activo = activo;
        await _repositorio.ActualizarAsync(existente);

        var actualizado = await _repositorio.ObtenerOperadorDetalladoPorIdAsync(idUsuario);
        return actualizado ?? existente;
    }

    public async Task<bool> EliminarAsync(int id, int? idUsuarioSolicitante = null)
    {
        if (id <= 0)
            throw new ArgumentException("El ID debe ser mayor a 0.");

        if (idUsuarioSolicitante.HasValue && idUsuarioSolicitante.Value == id)
            throw new InvalidOperationException("No puedes eliminar la cuenta con la que estás operando actualmente.");

        var existente = await _repositorio.ObtenerOperadorDetalladoPorIdAsync(id);
        if (existente == null)
            throw new KeyNotFoundException($"No se encontró el usuario con ID {id}.");

        if (existente.IdRol == 1)
        {
            var todos = await _repositorio.ObtenerOperadoresDetalladosAsync();
            var adminsActivosRestantes = todos.Count(o => o.IdUsuario != id && o.Activo && o.IdRol == 1);
            if (adminsActivosRestantes == 0)
            {
                throw new InvalidOperationException("No se puede eliminar al único Administrador activo del sistema.");
            }
        }

        return await _repositorio.EliminarAsync(id);
    }
}
