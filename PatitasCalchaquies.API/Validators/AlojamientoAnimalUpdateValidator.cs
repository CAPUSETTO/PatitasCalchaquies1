using FluentValidation;
using PatitasCalchaquies.API.DTOs;

namespace PatitasCalchaquies.API.Validators;

public class AlojamientoAnimalUpdateValidator : AbstractValidator<AlojamientoAnimalActualizarDto>
{
    public AlojamientoAnimalUpdateValidator()
    {
        RuleFor(x => x.IdAlojamiento)
            .GreaterThan(0).WithMessage("El ID del alojamiento debe ser mayor a 0.");

        RuleFor(x => x.IdAnimal)
            .GreaterThan(0).WithMessage("Debe seleccionar un animal válido.");

        RuleFor(x => x.IdSector)
            .GreaterThan(0).WithMessage("Debe seleccionar un sector válido.");

        RuleFor(x => x.FechaIngreso)
            .NotEmpty().WithMessage("La fecha de ingreso es obligatoria.");

        RuleFor(x => x.FechaSalida)
            .GreaterThanOrEqualTo(x => x.FechaIngreso)
            .When(x => x.FechaSalida.HasValue)
            .WithMessage("La fecha de salida no puede ser anterior a la fecha de ingreso.");

        RuleFor(x => x.Observacion)
            .MaximumLength(500).When(x => !string.IsNullOrEmpty(x.Observacion))
            .WithMessage("La observación no puede superar los 500 caracteres.");
    }
}