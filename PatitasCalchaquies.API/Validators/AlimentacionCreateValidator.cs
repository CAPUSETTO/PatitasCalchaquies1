using FluentValidation;
using PatitasCalchaquies.API.DTOs;

namespace PatitasCalchaquies.API.Validators;

public class AlimentacionCreateValidator : AbstractValidator<AlimentacionCrearDto>
{
    public AlimentacionCreateValidator()
    {
        RuleFor(x => x.IdAnimal)
            .GreaterThan(0).WithMessage("Debe seleccionar un animal válido.");

        RuleFor(x => x.FechaHora)
            .NotEmpty().WithMessage("La fecha y hora son obligatorias.")
            .LessThanOrEqualTo(DateTime.Now.AddDays(1)).WithMessage("La fecha no puede ser futura.");

        RuleFor(x => x.TipoAlimento)
            .NotEmpty().WithMessage("El tipo de alimento es obligatorio.")
            .MaximumLength(100).WithMessage("El tipo de alimento no puede superar los 100 caracteres.");

        RuleFor(x => x.Cantidad)
            .GreaterThan(0).WithMessage("La cantidad debe ser mayor a 0.");

        RuleFor(x => x.Observacion)
            .MaximumLength(500).When(x => !string.IsNullOrEmpty(x.Observacion))
            .WithMessage("La observación no puede superar los 500 caracteres.");
    }
}