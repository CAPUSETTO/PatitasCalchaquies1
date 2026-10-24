using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using PatitasCalchaquies.API.DTOs;
using PatitasCalchaquies.Core.Entities;
using PatitasCalchaquies.Core.Services;

namespace PatitasCalchaquies.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SectoresController : ControllerBase
{
    private readonly ServicioSector _servicio;
    private readonly IValidator<SectorCrearDto> _validadorCrear;
    private readonly IValidator<SectorActualizarDto> _validadorActualizar;
    private readonly IMapper _mapper;

    public SectoresController(
        ServicioSector servicio,
        IValidator<SectorCrearDto> validadorCrear,
        IValidator<SectorActualizarDto> validadorActualizar,
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
        var sectores = await _servicio.ObtenerTodosAsync();
        return Ok(_mapper.Map<IEnumerable<SectorDto>>(sectores));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var sector = await _servicio.ObtenerPorIdAsync(id);
        if (sector == null)
            return NotFound(new { mensaje = $"No se encontró el sector con ID {id}." });

        return Ok(_mapper.Map<SectorDto>(sector));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] SectorCrearDto dto)
    {
        var validacion = await _validadorCrear.ValidateAsync(dto);
        if (!validacion.IsValid)
        {
            var errores = validacion.Errors.Select(e => e.ErrorMessage).ToList();
            return BadRequest(new { mensaje = "Error de validación", errores });
        }

        try
        {
            var sector = _mapper.Map<Sector>(dto);
            int id = await _servicio.CrearAsync(sector);
            sector.IdSector = id;

            var respuesta = _mapper.Map<SectorDto>(sector);
            return CreatedAtAction(nameof(GetById), new { id }, respuesta);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] SectorActualizarDto dto)
    {
        if (id != dto.IdSector)
            return BadRequest(new { mensaje = "El ID de la ruta no coincide con el del cuerpo." });

        var validacion = await _validadorActualizar.ValidateAsync(dto);
        if (!validacion.IsValid)
        {
            var errores = validacion.Errors.Select(e => e.ErrorMessage).ToList();
            return BadRequest(new { mensaje = "Error de validación", errores });
        }

        try
        {
            var sector = _mapper.Map<Sector>(dto);
            bool actualizado = await _servicio.ActualizarAsync(sector);
            if (!actualizado)
                return NotFound(new { mensaje = $"No se pudo actualizar el sector con ID {id}." });

            return Ok(_mapper.Map<SectorDto>(sector));
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
                return NotFound(new { mensaje = $"No se encontró el sector con ID {id}." });

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