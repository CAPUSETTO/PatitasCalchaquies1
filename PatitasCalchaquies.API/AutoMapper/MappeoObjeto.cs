using AutoMapper;
using PatitasCalchaquies.API.DTOs;
using PatitasCalchaquies.Core.Entities;

namespace PatitasCalchaquies.API.AutoMapper;

public class MappeoObjeto : Profile
{
    public MappeoObjeto()
    {
        // ==========================================
        // 1. Mapeos de Persona
        // ==========================================
        CreateMap<Persona, PersonaDto>();
        CreateMap<PersonaCrearDto, Persona>();
        CreateMap<PersonaActualizarDto, Persona>();

        // ==========================================
        // 2. Mapeos de Medicamento
        // ==========================================
        CreateMap<Medicamento, MedicamentoDto>();
        CreateMap<MedicamentoCrearDto, Medicamento>();
        CreateMap<MedicamentoActualizarDto, Medicamento>();

        // ==========================================
        // 3. Mapeos de Vacuna
        // ==========================================
        CreateMap<Vacuna, VacunaDto>();
        CreateMap<VacunaCrearDto, Vacuna>();
        CreateMap<VacunaActualizarDto, Vacuna>();

        // ==========================================
        // 4. Mapeos de Raza
        // ==========================================
        CreateMap<Raza, RazaDto>();
        CreateMap<RazaCrearDto, Raza>();
        // (Si llegas a usar actualizar para Raza, se agrega: CreateMap<RazaActualizarDto, Raza>();)
        
        // 5. Mapeo Animal
        
        CreateMap<Animal, AnimalDto>();
        CreateMap<AnimalCrearDto, Animal>();
        CreateMap<AnimalActualizarDto, Animal>();
    }
}