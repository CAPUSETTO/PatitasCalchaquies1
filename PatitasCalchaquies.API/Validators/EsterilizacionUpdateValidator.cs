using FluentValidation;
using PatitasCalchaquies.API.DTOs;

namespace PatitasCalchaquies.API.Validators;

public class EsterilizacionUpdateValidator : AbstractValidator<EsterilizacionActualizarDto>
{
    public EsterilizacionUpdateValidator()
    {
        RuleFor(x => x.IdEsterilizacion)
            .GreaterThan(0).WithMessage("El ID de la esterilización debe ser mayor a 0.");

        RuleFor(x => x.IdAnimal)
            .GreaterThan(0).WithMessage("Debe seleccionar un animal válido.");

        RuleFor(x => x.IdVeterinario)
            .GreaterThan(0).WithMessage("Debe seleccionar un veterinario válido.");

        RuleFor(x => x.Fecha)
            .NotEmpty().WithMessage("La fecha de esterilización es obligatoria.");

        RuleFor(x => x.Tipo)
            .NotEmpty().WithMessage("El tipo de esterilización es obligatorio.")
            .MaximumLength(50).WithMessage("El tipo no puede superar los 50 caracteres.");

        RuleFor(x => x.Observacion)
            .MaximumLength(500).When(x => !string.IsNullOrEmpty(x.Observacion))
            .WithMessage("La observación no puede superar los 500 caracteres.");
    }
}