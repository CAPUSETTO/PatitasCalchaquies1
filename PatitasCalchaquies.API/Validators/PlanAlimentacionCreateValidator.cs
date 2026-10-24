using FluentValidation;
using PatitasCalchaquies.API.DTOs;

namespace PatitasCalchaquies.API.Validators;

public class PlanAlimentacionCreateValidator : AbstractValidator<PlanAlimentacionCrearDto>
{
    public PlanAlimentacionCreateValidator()
    {
        RuleFor(x => x.IdAnimal)
            .GreaterThan(0).WithMessage("Debe seleccionar un animal válido.");

        RuleFor(x => x.TipoAlimento)
            .NotEmpty().WithMessage("El tipo de alimento es obligatorio.")
            .MaximumLength(100).WithMessage("El tipo de alimento no puede superar los 100 caracteres.");

        RuleFor(x => x.Cantidad)
            .GreaterThan(0).WithMessage("La cantidad debe ser mayor a 0.");

        RuleFor(x => x.Frecuencia)
            .NotEmpty().WithMessage("La frecuencia es obligatoria.")
            .MaximumLength(50).WithMessage("La frecuencia no puede superar los 50 caracteres.");

        RuleFor(x => x.FechaDesde)
            .NotEmpty().WithMessage("La fecha de inicio es obligatoria.");

        RuleFor(x => x.FechaHasta)
            .GreaterThanOrEqualTo(x => x.FechaDesde)
            .When(x => x.FechaHasta.HasValue)
            .WithMessage("La fecha hasta no puede ser anterior a la fecha de inicio.");

        RuleFor(x => x.Observacion)
            .MaximumLength(500).When(x => !string.IsNullOrEmpty(x.Observacion))
            .WithMessage("La observación no puede superar los 500 caracteres.");
    }
}