using FluentValidation;
using PatitasCalchaquies.API.DTOs;

namespace PatitasCalchaquies.API.Validators;

public class SeguimientoAdopcionCreateValidator : AbstractValidator<SeguimientoAdopcionCrearDto>
{
    public SeguimientoAdopcionCreateValidator()
    {
        RuleFor(x => x.IdAdopcion)
            .GreaterThan(0).WithMessage("Debe seleccionar una adopción válida.");

        RuleFor(x => x.Fecha)
            .NotEmpty().WithMessage("La fecha del seguimiento es obligatoria.")
            .LessThanOrEqualTo(DateTime.Now.AddDays(1)).WithMessage("La fecha no puede ser futura.");

        RuleFor(x => x.Tipo)
            .NotEmpty().WithMessage("El tipo de seguimiento es obligatorio.")
            .MaximumLength(50).WithMessage("El tipo no puede superar los 50 caracteres.");

        RuleFor(x => x.Resultado)
            .NotEmpty().WithMessage("El resultado es obligatorio.")
            .MaximumLength(100).WithMessage("El resultado no puede superar los 100 caracteres.");

        RuleFor(x => x.Observacion)
            .MaximumLength(500).When(x => !string.IsNullOrEmpty(x.Observacion))
            .WithMessage("La observación no puede superar los 500 caracteres.");
    }
}