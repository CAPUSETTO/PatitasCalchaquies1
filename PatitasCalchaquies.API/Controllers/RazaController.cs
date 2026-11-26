using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using PatitasCalchaquies.API.DTOs;
using PatitasCalchaquies.Core.Entities;
using PatitasCalchaquies.Core.Services;
using MySqlConnector;

namespace PatitasCalchaquies.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]

public class RazaController : ControllerBase
{
    private readonly ServicioRaza _serviceRaza;
    private readonly IValidator<RazaCrearDto> _validatorCrearRaza;
    private readonly IValidator<RazaActualizarDto> _validatorActualizarRaza;
    private readonly IMapper _mapper;

    public RazaController(ServicioRaza serviceRaza,  IValidator<RazaCrearDto> validatorCrearRaza,IValidator<RazaActualizarDto> validatorActualizarRaza,IMapper mapper)
    {
        _serviceRaza = serviceRaza;
        _validatorCrearRaza = validatorCrearRaza;
        _validatorActualizarRaza = validatorActualizarRaza;
        _mapper = mapper;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var razas = await _serviceRaza.AllAsync();
        return Ok(razas);
    }

    [HttpGet("especie/{idEspecie:int}")]
    public async Task<IActionResult> GetByEspecie(int idEspecie)
    {
        try
        {
            var razas = await _serviceRaza.GetByEspecieAsync(idEspecie);
            return Ok(razas);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] RazaCrearDto dto)
    {
        var validationResult = await _validatorCrearRaza.ValidateAsync(dto);
        if (!validationResult.IsValid)
        {
            var errores =  validationResult.Errors.Select(x => x.ErrorMessage).ToList();
            return BadRequest(new { Mensaje = "Error de validación", Errores = errores });
        }
        try
        {
            Raza raza = _mapper.Map<Raza>(dto);

            int id = await _serviceRaza.CrearAsync(raza);
            raza.IdRaza = id;

            RazaDto razaDto = _mapper.Map<RazaDto>(raza);

            return Ok(razaDto);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
    
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] RazaActualizarDto dto)
    {
        if (id != dto.IdRaza)
            return BadRequest(new { mensaje = "El ID de la ruta no coincide con el del cuerpo." });

        var validacion = await _validatorActualizarRaza.ValidateAsync(dto);
        if (!validacion.IsValid)
        {
            var errores = validacion.Errors.Select(e => e.ErrorMessage).ToList();
            return BadRequest(new { mensaje = "Error de validación", errores });
        }

        try
        {
            var raza = _mapper.Map<Raza>(dto);
            bool actualizado = await _serviceRaza.ActualizarAsync(raza);
            if (!actualizado)
                return NotFound(new { mensaje = $"No se pudo actualizar la raza con ID {id}." });

            return Ok(_mapper.Map<RazaDto>(raza));
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
            bool eliminado = await _serviceRaza.EliminarAsync(id);
            if (!eliminado)
                return NotFound(new { mensaje = $"No se encontró la raza con ID {id}." });

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
