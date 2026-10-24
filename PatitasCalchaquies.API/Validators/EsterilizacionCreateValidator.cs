using FluentValidation;
using PatitasCalchaquies.API.DTOs;

namespace PatitasCalchaquies.API.Validators;

public class EsterilizacionCreateValidator : AbstractValidator<EsterilizacionCrearDto>
{
    public EsterilizacionCreateValidator()
    {
        RuleFor(x => x.IdAnimal)
            .GreaterThan(0).WithMessage("Debe seleccionar un animal válido.");

        RuleFor(x => x.IdVeterinario)
            .GreaterThan(0).WithMessage("Debe seleccionar un veterinario válido.");

        RuleFor(x => x.Fecha)
            .NotEmpty().WithMessage("La fecha de esterilización es obligatoria.")
            .LessThanOrEqualTo(DateTime.Now.AddDays(1)).WithMessage("La fecha no puede ser futura.");

        RuleFor(x => x.Tipo)
            .NotEmpty().WithMessage("El tipo de esterilización es obligatorio.")
            .MaximumLength(50).WithMessage("El tipo no puede superar los 50 caracteres.");

        RuleFor(x => x.Observacion)
            .MaximumLength(500).When(x => !string.IsNullOrEmpty(x.Observacion))
            .WithMessage("La observación no puede superar los 500 caracteres.");
    }
}