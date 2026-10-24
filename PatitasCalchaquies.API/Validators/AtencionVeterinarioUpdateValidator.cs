using FluentValidation;
using PatitasCalchaquies.API.DTOs;

namespace PatitasCalchaquies.API.Validators;

public class AtencionVeterinarioUpdateValidator : AbstractValidator<AtencionVeterinarioActualizarDto>
{
    public AtencionVeterinarioUpdateValidator()
    {
        RuleFor(x => x.IdAtencionVeterinaria)
            .GreaterThan(0).WithMessage("El ID de la atención debe ser mayor a 0.");

        RuleFor(x => x.IdAnimal)
            .GreaterThan(0).WithMessage("Debe seleccionar un animal válido.");

        RuleFor(x => x.IdVeterinario)
            .GreaterThan(0).WithMessage("Debe seleccionar un veterinario válido.");

        RuleFor(x => x.Fecha)
            .NotEmpty().WithMessage("La fecha de atención es obligatoria.");

        RuleFor(x => x.Motivo)
            .NotEmpty().WithMessage("El motivo es obligatorio.")
            .MaximumLength(255).WithMessage("El motivo no puede superar los 255 caracteres.");
    }
}