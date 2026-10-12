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

    public VacunaController(ServicioVacuna serviceVacuna)
        {
            _serviceVacuna = serviceVacuna;
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
            try
            {
                Vacuna vacuna = new Vacuna
                {
                    Nombre = dto.nombre,
                    Descripcion = dto.descripcion
                };
                
                int nuevoId = await _serviceVacuna.CrearVacunaAsync(vacuna);
                vacuna.IdVacuna = nuevoId;

                VacunaDto nuevoVacuna = new VacunaDto
                {
                    IdVacuna = vacuna.IdVacuna,
                    nombre = vacuna.Nombre,
                    descripcion = vacuna.Descripcion
                };
                
                return CreatedAtAction(nameof(GetById), new { id = vacuna.IdVacuna }, nuevoVacuna);
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
                Vacuna vacuna = new Vacuna
                {
                    IdVacuna = dto.IdVacuna,
                    Nombre = dto.nombre,
                    Descripcion = dto.descripcion
                };
                bool resultado = await _serviceVacuna.ActualizarAsync(vacuna);
                if (!resultado)
                {
                    return NotFound(new {mensaje = $"No se pudo encontrar la persona con ID {idVacuna} para actualizar."});
                }

                VacunaDto vacunaActualizado = new VacunaDto
                {
                    IdVacuna = vacuna.IdVacuna,
                    nombre = vacuna.Nombre,
                    descripcion = vacuna.Descripcion
                };
                
                return Ok(vacunaActualizado);
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