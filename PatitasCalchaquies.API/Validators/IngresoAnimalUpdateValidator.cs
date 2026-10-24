using FluentValidation;
using PatitasCalchaquies.API.DTOs;

namespace PatitasCalchaquies.API.Validators;

public class IngresoAnimalUpdateValidator : AbstractValidator<IngresoAnimalActualizarDto>
{
    public IngresoAnimalUpdateValidator()
    {
        RuleFor(x => x.IdIngresoAnimal)
            .GreaterThan(0).WithMessage("El ID del ingreso debe ser mayor a 0.");

        RuleFor(x => x.IdAnimal)
            .GreaterThan(0).WithMessage("Debe seleccionar un animal válido.");

        RuleFor(x => x.Fecha)
            .NotEmpty().WithMessage("La fecha de ingreso es obligatoria.");

        RuleFor(x => x.Motivo)
            .NotEmpty().WithMessage("El motivo es obligatorio.")
            .MaximumLength(255).WithMessage("El motivo no puede superar los 255 caracteres.");

        RuleFor(x => x.TipoIngreso)
            .NotEmpty().WithMessage("El tipo de ingreso es obligatorio.")
            .MaximumLength(50).WithMessage("El tipo de ingreso no puede superar los 50 caracteres.");

        RuleFor(x => x.LugarOrigen)
            .MaximumLength(150).When(x => !string.IsNullOrEmpty(x.LugarOrigen))
            .WithMessage("El lugar de origen no puede superar los 150 caracteres.");

        RuleFor(x => x.Observacion)
            .MaximumLength(500).When(x => !string.IsNullOrEmpty(x.Observacion))
            .WithMessage("La observación no puede superar los 500 caracteres.");
    }
}