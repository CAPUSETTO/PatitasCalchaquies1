using FluentValidation;
using PatitasCalchaquies.API.DTOs;

namespace PatitasCalchaquies.API.Validators;

public class TratamientoMedicamentoCreateValidator : AbstractValidator<TratamientoMedicamentoCrearDto>
{
    public TratamientoMedicamentoCreateValidator()
    {
        RuleFor(x => x.IdMedicamento)
            .GreaterThan(0).WithMessage("Debe seleccionar un medicamento válido.");

        RuleFor(x => x.IdTratamiento)
            .GreaterThan(0).WithMessage("Debe seleccionar un tratamiento válido.");

        RuleFor(x => x.Dosis)
            .NotEmpty().WithMessage("La dosis es obligatoria.")
            .MaximumLength(100).WithMessage("La dosis no puede superar los 100 caracteres.");

        RuleFor(x => x.Frecuencia)
            .NotEmpty().WithMessage("La frecuencia es obligatoria.")
            .MaximumLength(100).WithMessage("La frecuencia no puede superar los 100 caracteres.");

        RuleFor(x => x.Duracion)
            .NotEmpty().WithMessage("La duración es obligatoria.")
            .MaximumLength(100).WithMessage("La duración no puede superar los 100 caracteres.");

        RuleFor(x => x.Observacion)
            .MaximumLength(500).When(x => !string.IsNullOrEmpty(x.Observacion))
            .WithMessage("La observación no puede superar los 500 caracteres.");
    }
}