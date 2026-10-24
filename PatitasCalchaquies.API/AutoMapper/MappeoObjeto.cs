using AutoMapper;
using PatitasCalchaquies.API.DTOs;
using PatitasCalchaquies.Core.Entities;

namespace PatitasCalchaquies.API.AutoMapper;

public class MappeoObjeto : Profile
{
    public MappeoObjeto()
    {

        // 1. Mapeos de Persona
        CreateMap<Persona, PersonaDto>();
        CreateMap<PersonaCrearDto, Persona>();
        CreateMap<PersonaActualizarDto, Persona>();


        // 2. Mapeos de Medicamento

        CreateMap<Medicamento, MedicamentoDto>();
        CreateMap<MedicamentoCrearDto, Medicamento>();
        CreateMap<MedicamentoActualizarDto, Medicamento>();
        
        // 3. Mapeos de Vacuna
        CreateMap<Vacuna, VacunaDto>();
        CreateMap<VacunaCrearDto, Vacuna>();
        CreateMap<VacunaActualizarDto, Vacuna>();
        
        // 4. Mapeos de Raza
        
        CreateMap<Raza, RazaDto>();
        CreateMap<RazaCrearDto, Raza>();
        
        // 5. Mapeo Animal
        
        CreateMap<Animal, AnimalDto>();
        CreateMap<AnimalCrearDto, Animal>();
        CreateMap<AnimalActualizarDto, Animal>();
        
        // 6. Mapeos de Adopción
        CreateMap<Adopcion, AdopcionDto>();
        CreateMap<AdopcionCrearDto, Adopcion>();
        CreateMap<AdopcionActualizarDto, Adopcion>();
        
        // 7. Mapeos de Alimentación
        CreateMap<Alimentacion, AlimentacionDto>();
        CreateMap<AlimentacionCrearDto, Alimentacion>();
        CreateMap<AlimentacionActualizarDto, Alimentacion>();
        
        // 8. Mapeos de AlojamientoAnimal
        CreateMap<AlojamientoAnimal, AlojamientoAnimalDto>();
        CreateMap<AlojamientoAnimalCrearDto, AlojamientoAnimal>();
        CreateMap<AlojamientoAnimalActualizarDto, AlojamientoAnimal>();
        
        // 9. Mapeos de AtencionVeterinaria
        CreateMap<AtencionVeterinaria, AtencionVeterinariaDto>();
        CreateMap<AtencionVeterinariaCrearDto, AtencionVeterinaria>();
        CreateMap<AtencionVeterinarioActualizarDto, AtencionVeterinaria>();
        
        // 10. Mapeos de DevolucionAdopcion
        CreateMap<DevolucionAdopcion, DevolucionAdopcionDto>();
        CreateMap<DevolucionAdopcionCrearDto, DevolucionAdopcion>();
        CreateMap<DevolucionAdopcionActualizarDto, DevolucionAdopcion>();
        
        // 11. Mapeos de Especie
        CreateMap<Especie, EspecieDto>();
        CreateMap<EspecieCrearDto, Especie>();
        CreateMap<EspecieActualizarDto, Especie>();
        
        // 12. Mapeos de EstadoAnimal
        CreateMap<EstadoAnimal, EstadoAnimalDto>();
        CreateMap<EstadoAnimalCrearDto, EstadoAnimal>();
        CreateMap<EstadoAnimalActualizarDto, EstadoAnimal>();
        
        // 13. Mapeos de Esterilizacion
        CreateMap<Esterilizacion, EsterilizacionDto>();
        CreateMap<EsterilizacionCrearDto, Esterilizacion>();
        CreateMap<EsterilizacionActualizarDto, Esterilizacion>();
        
        // 14. Mapeos de IngresoAnimal
        CreateMap<IngresoAnimal, IngresoAnimalDto>();
        CreateMap<IngresoAnimalCrearDto, IngresoAnimal>();
        CreateMap<IngresoAnimalActualizarDto, IngresoAnimal>();
        
        // 15. Mapeos de FotoAnimal
        CreateMap<FotoAnimal, FotoAnimalDto>();
        CreateMap<FotoAnimalCrearDto, FotoAnimal>();
        CreateMap<FotoAnimalActualizarDto, FotoAnimal>();
        
        // 16. Mapeos de PlanAlimentacion
        CreateMap<PlanAlimentacion, PlanAlimentacionDto>();
        CreateMap<PlanAlimentacionCrearDto, PlanAlimentacion>();
        CreateMap<PlanAlimentacionActualizarDto, PlanAlimentacion>();
        
        // 17. Mapeos de Predio
        CreateMap<Predio, PredioDto>();
        CreateMap<PredioCrearDto, Predio>();
        CreateMap<PredioActualizarDto, Predio>();
        
        // 18. Mapeos de Rol
        CreateMap<Rol, RolDto>();
        CreateMap<RolCrearDto, Rol>();
        CreateMap<RolActualizarDto, Rol>();
        
        // 19. Mapeos de Sector
        CreateMap<Sector, SectorDto>();
        CreateMap<SectorCrearDto, Sector>();
        CreateMap<SectorActualizarDto, Sector>();
        
        // 20. Mapeos de SeguimientoAdopcion
        CreateMap<SeguimientoAdopcion, SeguimientoAdopcionDto>();
        CreateMap<SeguimientoAdopcionCrearDto, SeguimientoAdopcion>();
        CreateMap<SeguimientoAdopcionActualizarDto, SeguimientoAdopcion>();
        
        // 21. Mapeos de Sexo
        CreateMap<Sexo, SexoDto>();
        CreateMap<SexoCrearDto, Sexo>();
        CreateMap<SexoActualizarDto, Sexo>();
        
        // 22. Mapeos de SolicitudAdopcion
        CreateMap<SolicitudAdopcion, SolicitudAdopcionDto>();
        CreateMap<SolicitudAdopcionCrearDto, SolicitudAdopcion>();
        CreateMap<SolicitudAdopcionActualizarDto, SolicitudAdopcion>();
        
        // 23. Mapeos de Tratamiento
        CreateMap<Tratamiento, TratamientoDto>();
        CreateMap<TratamientoCrearDto, Tratamiento>();
        CreateMap<TratamientoActualizarDto, Tratamiento>();
        
        // 24. Mapeos de TratamientoMedicamento
        CreateMap<TratamientoMedicamento, TratamientoMedicamentoDto>();
        CreateMap<TratamientoMedicamentoCrearDto, TratamientoMedicamento>();
        CreateMap<TratamientoMedicamentoActualizarDto, TratamientoMedicamento>();
        
        // 25. Mapeos de Usuario
        CreateMap<Usuario, UsuarioDto>();
        CreateMap<UsuarioCrearDto, Usuario>();
        CreateMap<UsuarioActualizarDto, Usuario>();
        
        // 26. Mapeos de UsuarioRol
        CreateMap<UsuarioRol, UsuarioRolDto>();
        CreateMap<UsuarioRolCrearDto, UsuarioRol>();
        
        // 27. Mapeos de Vacunacion
        CreateMap<Vacunacion, VacunacionDto>();
        CreateMap<VacunacionCrearDto, Vacunacion>();
        CreateMap<VacunacionActualizarDto, Vacunacion>();
        
        // 28. Mapeos de Veterinario
        CreateMap<Veterinario, VeterinarioDto>();
        CreateMap<VeterinarioCrearDto, Veterinario>();
        CreateMap<VeterinarioActualizarDto, Veterinario>();
    }
}