using FluentValidation;
using PatitasCalchaquies.API.DTOs;

namespace PatitasCalchaquies.API.Validators;

public class AdopcionCreateValidator : AbstractValidator<AdopcionCrearDto>
{
    public AdopcionCreateValidator()
    {
        RuleFor(x => x.IdSolicitudAdopcion)
            .GreaterThan(0).WithMessage("Debe seleccionar una solicitud de adopción válida.");

        RuleFor(x => x.Fecha)
            .NotEmpty().WithMessage("La fecha de adopción es obligatoria.")
            .LessThanOrEqualTo(DateTime.Now.AddDays(1)).WithMessage("La fecha no puede ser futura.");

        RuleFor(x => x.Observacion)
            .MaximumLength(500).When(x => !string.IsNullOrEmpty(x.Observacion))
            .WithMessage("La observación no puede superar los 500 caracteres.");
    }
}