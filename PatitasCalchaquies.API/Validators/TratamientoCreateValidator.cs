using FluentValidation;
using PatitasCalchaquies.API.DTOs;

namespace PatitasCalchaquies.API.Validators;

public class TratamientoCreateValidator : AbstractValidator<TratamientoCrearDto>
{
    public TratamientoCreateValidator()
    {
        RuleFor(x => x.IdAtencionVeterinaria)
            .GreaterThan(0).WithMessage("Debe seleccionar una atención veterinaria válida.");

        RuleFor(x => x.Descripcion)
            .NotEmpty().WithMessage("La descripción del tratamiento es obligatoria.")
            .MaximumLength(255).WithMessage("La descripción no puede superar los 255 caracteres.");

        RuleFor(x => x.FechaInicio)
            .NotEmpty().WithMessage("La fecha de inicio es obligatoria.");

        RuleFor(x => x.FechaFin)
            .GreaterThanOrEqualTo(x => x.FechaInicio)
            .When(x => x.FechaFin.HasValue)
            .WithMessage("La fecha de fin no puede ser anterior a la fecha de inicio.");

        RuleFor(x => x.Estado)
            .NotEmpty().WithMessage("El estado del tratamiento es obligatorio.")
            .MaximumLength(50).WithMessage("El estado no puede superar los 50 caracteres.");

        RuleFor(x => x.Observacion)
            .MaximumLength(500).When(x => !string.IsNullOrEmpty(x.Observacion))
            .WithMessage("La observación no puede superar los 500 caracteres.");
    }
}