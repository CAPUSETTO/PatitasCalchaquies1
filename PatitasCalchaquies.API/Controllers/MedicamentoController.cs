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

public class MedicamentoController : ControllerBase
    {
        private readonly ServicioMedicamento _serviceMedicamento;
        private readonly IValidator<MedicamentoCrearDto> _validatorCrearMedicamento;
        private readonly IMapper _mapper;

        public MedicamentoController(ServicioMedicamento serviceMedicamento, IValidator<MedicamentoCrearDto> validatorCrearMedicamento, IMapper mapper)
        {
            _serviceMedicamento = serviceMedicamento;
            _validatorCrearMedicamento = validatorCrearMedicamento;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var medicamento = await _serviceMedicamento.Allsync();
            return Ok(medicamento);
        }

        [HttpGet("{idMedicamento:int}")]
        public async Task<IActionResult> GetById(int idMedicamento)
        {
            var medicamento = await _serviceMedicamento.GetByIdAsync(idMedicamento);
            if (medicamento==null)
            {
                return NotFound();
            }

            return Ok(medicamento);
        }

        [HttpGet("buscar")]
        public async Task<IActionResult> Buscar([FromQuery] string nombre)
        {
            try
            {
                var resultados = await _serviceMedicamento.BuscarAsync(nombre);
                return Ok(resultados);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] MedicamentoCrearDto dto)
        {
            var validationResult = await _validatorCrearMedicamento.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                var errores =  validationResult.Errors.Select(x => x.ErrorMessage).ToList();
                return BadRequest(new { Mensaje = "Error de validación", Errores = errores });
            }
            
            try
            {
                //Mapeo de DTO -> Entidad con AutoMapper (¡en una sola línea!)
                Medicamento medicamento =  _mapper.Map<Medicamento>(dto);
                
                int nuevoId = await _serviceMedicamento.CrearMedicamentoAsync(medicamento);
                medicamento.IdMedicamento = nuevoId;
                
                //Respuesta
                MedicamentoDto medicamentoDto = _mapper.Map<MedicamentoDto>(medicamento);
                
                return CreatedAtAction(nameof(GetById), new { idMedicamento = medicamento.IdMedicamento }, medicamentoDto);
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

        [HttpPut("{idMedicamento:int}")]
        public async Task<IActionResult> Actualizar(int idMedicamento, [FromBody] MedicamentoDto dto)
        {
            if (idMedicamento != dto.IdMedicamento)
            {
                return BadRequest(new {error = "El id de la url no coincide con el cuerpo de la petición"});
            }

            try
            {
                Medicamento medicamento = _mapper.Map<Medicamento>(dto);
                
                bool resultado = await _serviceMedicamento.ActualizarAsync(medicamento);
                if (!resultado)
                {
                    return NotFound(new {mensaje = $"No se pudo encontrar la persona con ID {idMedicamento} para actualizar."});
                }
                
                MedicamentoDto medicamentoDto = _mapper.Map<MedicamentoDto>(medicamento);
                
                return Ok(medicamentoDto);
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

        [HttpDelete("{idMedicamento:int}")]
        public async Task<IActionResult> Eliminar(int idMedicamento)
        {
            try
            {
                bool eliminado = await _serviceMedicamento.EliminarAsync(idMedicamento);
                if (!eliminado)
                {
                    return NotFound(new { mensaje = $"No se encontró la persona con ID {idMedicamento} para eliminar."});
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