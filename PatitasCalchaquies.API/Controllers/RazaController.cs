using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using PatitasCalchaquies.API.DTOs;
using PatitasCalchaquies.Core.Entities;
using PatitasCalchaquies.Core.Services;
using MySqlConnector;

namespace PatitasCalchaquies.API.Controllers;

[ApiController]
[Route("api/[controller]")]

public class RazaController : ControllerBase
{
    private readonly ServicioRaza _serviceRaza;
    private readonly IValidator<RazaCrearDto> _validatorCrearRaza;
    private readonly IMapper _mapper;

    public RazaController(ServicioRaza serviceRaza,  IValidator<RazaCrearDto> validatorCrearRaza,IMapper mapper)
    {
        _serviceRaza = serviceRaza;
        _validatorCrearRaza = validatorCrearRaza;
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
    }

}