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

    public RazaController(ServicioRaza serviceRaza)
    {
        _serviceRaza = serviceRaza;
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
        try
        {
            var raza = new Raza
            {
                IdEspecie = dto.IdEspecie,
                Nombre = dto.Nombre
            };

            int id = await _serviceRaza.CrearAsync(raza);
            raza.IdRaza = id;

            var respuesta = new RazaDto
            {
                IdRaza = raza.IdRaza,
                IdEspecie = raza.IdEspecie,
                Nombre = raza.Nombre
            };

            return Ok(respuesta);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

}