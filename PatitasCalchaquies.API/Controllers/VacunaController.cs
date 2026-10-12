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

public class VacunaController : ControllerBase
{
    private readonly ServicioVacuna _serviceVacuna;
    private readonly IValidator<VacunaCrearDto> _validatorCrearVacuna;
    private readonly IMapper _mapper;

    public VacunaController(ServicioVacuna serviceVacuna,  IValidator<VacunaCrearDto> validatorCrearVacuna, IMapper mapper)
        {
            _serviceVacuna = serviceVacuna;
            _validatorCrearVacuna = validatorCrearVacuna;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var vacuna = await _serviceVacuna.Allsync();
            return Ok(vacuna);
        }

        [HttpGet("{idVacuna:int}")]
        public async Task<IActionResult> GetById(int idVacuna)
        {
            var vacuna = await _serviceVacuna.GetByIdAsync(idVacuna);
            if (vacuna==null)
            {
                return NotFound();
            }

            return Ok(vacuna);
        }

        [HttpGet("buscar")]
        public async Task<IActionResult> Buscar([FromQuery] string nombre)
        {
            try
            {
                var resultados = await _serviceVacuna.BuscarAsync(nombre);
                return Ok(resultados);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] VacunaCrearDto dto)
        {
            var validationResult = await _validatorCrearVacuna.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                var errores =  validationResult.Errors.Select(x => x.ErrorMessage).ToList();
                return BadRequest(new { Mensaje = "Error de validación", Errores = errores });
            }
            try
            {
                Vacuna vacuna = _mapper.Map<Vacuna>(dto);
                
                int nuevoId = await _serviceVacuna.CrearVacunaAsync(vacuna);
                vacuna.IdVacuna = nuevoId;

                VacunaDto vacunaDto = _mapper.Map<VacunaDto>(vacuna);
                
                return CreatedAtAction(nameof(GetById), new { id = vacuna.IdVacuna }, vacunaDto);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPut("{idVacuna:int}")]
        public async Task<IActionResult> Actualizar(int idVacuna, [FromBody] VacunaDto dto)
        {
            if (idVacuna != dto.IdVacuna)
            {
                return BadRequest(new {error = "El id de la url no coincide con el cuerpo de la petición"});
            }

            try
            {
                Vacuna vacuna = _mapper.Map<Vacuna>(dto);
                
                bool resultado = await _serviceVacuna.ActualizarAsync(vacuna);
                if (!resultado)
                {
                    return NotFound(new {mensaje = $"No se pudo encontrar la persona con ID {idVacuna} para actualizar."});
                }

                VacunaDto vacunaDto = _mapper.Map<VacunaDto>(vacuna);
                
                return Ok(vacunaDto);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (MySqlException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{idVacuna:int}")]
        public async Task<IActionResult> Eliminar(int idVacuna)
        {
            try
            {
                bool eliminado = await _serviceVacuna.EliminarAsync(idVacuna);
                if (!eliminado)
                {
                    return NotFound(new { mensaje = $"No se encontró la persona con ID {idVacuna} para eliminar."});
                }

                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
}