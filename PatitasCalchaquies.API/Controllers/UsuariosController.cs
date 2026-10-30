using System.Security.Claims;
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

    public static OperadorSistemaDto MapearOperadorDto(Usuario u)
    {
        int idRol = u.IdRol is >= 1 and <= 3 ? u.IdRol.Value : 3;
        string rolNombre = !string.IsNullOrWhiteSpace(u.RolNombre)
            ? u.RolNombre
            : idRol == 1
                ? "Administrador"
                : idRol == 2
                    ? "Veterinario"
                    : "Voluntario";

        string rolDescripcion = !string.IsNullOrWhiteSpace(u.RolDescripcion)
            ? u.RolDescripcion
            : idRol == 1
                ? "Coordinación General · Control total de ingresos, adopciones, clínica y alta de personal"
                : idRol == 2
                    ? "Veterinaria Clínica · Gestión de historial clínico, vacunaciones, cirugías y vademécum"
                    : "Guardia y Adopciones · Seguimiento de solicitudes de adopción, entrevistas y fichas";

        string resumenPermisos = idRol == 1
            ? "Control total de ingresos, adopciones, catálogo sanitario y alta de personal."
            : idRol == 2
                ? "Gestión de historial clínico, vacunaciones, cirugías y vademécum."
                : "Seguimiento de solicitudes de adopción, entrevistas y fichas de animales.";

        string nombre = !string.IsNullOrWhiteSpace(u.Nombre)
            ? u.Nombre.Trim()
            : !string.IsNullOrWhiteSpace(u.VeterinarioNombre)
                ? u.VeterinarioNombre.Trim()
                : u.User;

        string apellido = !string.IsNullOrWhiteSpace(u.Apellido)
            ? u.Apellido.Trim()
            : !string.IsNullOrWhiteSpace(u.VeterinarioApellido)
                ? u.VeterinarioApellido.Trim()
                : string.Empty;

        string prefijo = idRol == 2 && !nombre.StartsWith("Dr", StringComparison.OrdinalIgnoreCase)
            ? "Dr/a. "
            : string.Empty;

        string nombreCompleto = $"{prefijo}{nombre} {apellido}".Trim();

        string etiquetaArea = !string.IsNullOrWhiteSpace(u.EtiquetaArea)
            ? u.EtiquetaArea
            : idRol == 1
                ? "Coordinación General"
                : idRol == 2
                    ? "Veterinaria Clínica"
                    : "Guardia y Adopciones";

        DatosProfesionalesVeterinarioDto? vetDto = null;
        if (idRol == 2)
        {
            vetDto = new DatosProfesionalesVeterinarioDto
            {
                IdVeterinario = u.IdVeterinario,
                Matricula = !string.IsNullOrWhiteSpace(u.VeterinarioMatricula)
                    ? u.VeterinarioMatricula
                    : $"MP-SAL-0{100 + u.IdUsuario}",
                Nombre = nombre,
                Apellido = apellido,
                Telefono = !string.IsNullOrWhiteSpace(u.VeterinarioTelefono)
                    ? u.VeterinarioTelefono
                    : "+54 3868 421900"
            };
        }

        return new OperadorSistemaDto
        {
            IdUsuario = u.IdUsuario,
            User = u.User,
            NombreCompleto = nombreCompleto,
            EtiquetaArea = etiquetaArea,
            ResumenPermisos = resumenPermisos,
            Activo = u.Activo,
            FechaCreacion = u.FechaCreacion,
            UltimoAcceso = u.UltimoAcceso,
            Rol = new RolDto
            {
                IdRol = idRol,
                Nombre = rolNombre,
                Descripcion = rolDescripcion
            },
            Veterinario = vetDto
        };
    }

    private async Task<(bool Autorizado, int? IdUsuarioSolicitante, string Motivo)> VerificarPermisoAdministradorAsync()
    {
        int? idSolicitante = null;

        // 1. Verificar claims del token JWT si está autenticado
        if (User?.Identity?.IsAuthenticated == true)
        {
            var subClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                           ?? User.FindFirst("sub")?.Value;
            if (int.TryParse(subClaim, out int parsedSub))
            {
                idSolicitante = parsedSub;
            }

            var rolesClaims = User.FindAll(ClaimTypes.Role)
                .Concat(User.FindAll("role"))
                .Select(c => c.Value)
                .ToList();

            if (rolesClaims.Count > 0)
            {
                bool esAdminJwt = rolesClaims.Any(r =>
                    r.Contains("admin", StringComparison.OrdinalIgnoreCase) ||
                    r.Contains("coordinaci", StringComparison.OrdinalIgnoreCase) ||
                    r == "1");

                if (!esAdminJwt)
                {
                    return (false, idSolicitante, $"Permiso insuficiente: Tu rol actual ({string.Join(", ", rolesClaims)}) no está autorizado para dar de alta ni modificar Veterinarios o Administradores. Se requiere rol Administrador.");
                }

                return (true, idSolicitante, string.Empty);
            }
        }

        // 2. Verificar cabeceras enviadas por el cliente operativo (X-Operador-Id / X-Operador-Rol / X-Operador-Usuario)
        if (Request.Headers.TryGetValue("X-Operador-Id", out var headerId) &&
            int.TryParse(headerId.FirstOrDefault(), out int parsedHeaderId))
        {
            idSolicitante ??= parsedHeaderId;
        }

        if (Request.Headers.TryGetValue("X-Operador-Rol", out var headerRol))
        {
            var rolValor = headerRol.FirstOrDefault()?.Trim() ?? string.Empty;
            if (!string.IsNullOrEmpty(rolValor))
            {
                bool esAdminHeader = rolValor == "1" ||
                                     rolValor.Contains("admin", StringComparison.OrdinalIgnoreCase) ||
                                     rolValor.Contains("coordinaci", StringComparison.OrdinalIgnoreCase);
                if (!esAdminHeader)
                {
                    return (false, idSolicitante, $"Permiso insuficiente: El rol '{rolValor}' no tiene permisos para gestionar Veterinarios ni Administradores. Se requiere rol Administrador.");
                }
            }
        }

        if (idSolicitante.HasValue && idSolicitante.Value > 0)
        {
            var opEnDb = await _servicio.ObtenerOperadorPorIdAsync(idSolicitante.Value);
            if (opEnDb != null)
            {
                if (!opEnDb.Activo)
                {
                    return (false, idSolicitante, "Tu cuenta de operador se encuentra suspendida.");
                }
                if (opEnDb.IdRol.HasValue && opEnDb.IdRol.Value != 1)
                {
                    return (false, idSolicitante, $"Permiso insuficiente: Tu cuenta ({opEnDb.User}) tiene rol {opEnDb.RolNombre} y se requiere rol Administrador.");
                }
            }
        }

        return (true, idSolicitante, string.Empty);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var usuarios = await _servicio.ObtenerTodosAsync();
        return Ok(_mapper.Map<IEnumerable<UsuarioDto>>(usuarios));
    }

    [HttpGet("operadores")]
    public async Task<IActionResult> GetOperadores()
    {
        var operadores = await _servicio.ObtenerOperadoresAsync();
        return Ok(operadores.Select(MapearOperadorDto));
    }

    [HttpGet("operadores/{id:int}")]
    public async Task<IActionResult> GetOperadorById(int id)
    {
        var operador = await _servicio.ObtenerOperadorPorIdAsync(id);
        if (operador == null)
            return NotFound(new { mensaje = $"No se encontró el operador con ID {id}." });

        return Ok(MapearOperadorDto(operador));
    }

    [HttpPost("operadores")]
    public async Task<IActionResult> CrearOperador([FromBody] OperadorCrearDto dto)
    {
        var permiso = await VerificarPermisoAdministradorAsync();
        if (!permiso.Autorizado)
        {
            return StatusCode(403, new { mensaje = permiso.Motivo });
        }

        try
        {
            var creado = await _servicio.CrearOperadorAsync(
                dto.User,
                dto.Password,
                dto.IdRol,
                dto.Nombre,
                dto.Apellido,
                dto.EtiquetaArea,
                dto.Activo ?? true,
                dto.Matricula,
                dto.Telefono
            );

            var respuesta = MapearOperadorDto(creado);
            return CreatedAtAction(nameof(GetOperadorById), new { id = respuesta.IdUsuario }, respuesta);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { error = ex.Message });
        }
    }

    [HttpPut("operadores/{id:int}")]
    public async Task<IActionResult> ActualizarOperador(int id, [FromBody] OperadorActualizarDto dto)
    {
        var permiso = await VerificarPermisoAdministradorAsync();
        if (!permiso.Autorizado)
        {
            return StatusCode(403, new { mensaje = permiso.Motivo });
        }

        if (dto.IdUsuario > 0 && id != dto.IdUsuario)
            return BadRequest(new { mensaje = "El ID de la ruta no coincide con el del cuerpo." });

        try
        {
            var actualizado = await _servicio.ActualizarOperadorAsync(
                id,
                dto.User,
                dto.Password,
                dto.IdRol,
                dto.Nombre,
                dto.Apellido,
                dto.EtiquetaArea,
                dto.Activo,
                dto.Matricula,
                dto.Telefono
            );

            return Ok(MapearOperadorDto(actualizado));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { error = ex.Message });
        }
    }

    [HttpPatch("operadores/{id:int}/estado")]
    public async Task<IActionResult> CambiarEstadoOperador(int id, [FromBody] CambiarEstadoOperadorDto dto)
    {
        var permiso = await VerificarPermisoAdministradorAsync();
        if (!permiso.Autorizado)
        {
            return StatusCode(403, new { mensaje = permiso.Motivo });
        }

        try
        {
            var actualizado = await _servicio.CambiarEstadoOperadorAsync(id, dto.Activo, permiso.IdUsuarioSolicitante);
            return Ok(MapearOperadorDto(actualizado));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { error = ex.Message });
        }
    }

    [HttpDelete("operadores/{id:int}")]
    public async Task<IActionResult> EliminarOperador(int id)
    {
        var permiso = await VerificarPermisoAdministradorAsync();
        if (!permiso.Autorizado)
        {
            return StatusCode(403, new { mensaje = permiso.Motivo });
        }

        try
        {
            bool eliminado = await _servicio.EliminarAsync(id, permiso.IdUsuarioSolicitante);
            if (!eliminado)
                return NotFound(new { mensaje = $"No se encontró el operador con ID {id}." });

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
        catch (InvalidOperationException ex)
        {
            return Conflict(new { error = ex.Message });
        }
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
        var permiso = await VerificarPermisoAdministradorAsync();
        if (!permiso.Autorizado)
        {
            return StatusCode(403, new { mensaje = permiso.Motivo });
        }

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
        catch (InvalidOperationException ex)
        {
            return Conflict(new { error = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UsuarioActualizarDto dto)
    {
        var permiso = await VerificarPermisoAdministradorAsync();
        if (!permiso.Autorizado)
        {
            return StatusCode(403, new { mensaje = permiso.Motivo });
        }

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
        catch (InvalidOperationException ex)
        {
            return Conflict(new { error = ex.Message });
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var permiso = await VerificarPermisoAdministradorAsync();
        if (!permiso.Autorizado)
        {
            return StatusCode(403, new { mensaje = permiso.Motivo });
        }

        try
        {
            bool eliminado = await _servicio.EliminarAsync(id, permiso.IdUsuarioSolicitante);
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
        catch (InvalidOperationException ex)
        {
            return Conflict(new { error = ex.Message });
        }
    }
}
