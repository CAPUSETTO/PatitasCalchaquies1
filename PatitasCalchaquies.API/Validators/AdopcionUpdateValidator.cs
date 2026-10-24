using FluentValidation;
using PatitasCalchaquies.API.DTOs;

namespace PatitasCalchaquies.API.Validators;

public class AdopcionUpdateValidator : AbstractValidator<AdopcionActualizarDto>
{
    public AdopcionUpdateValidator()
    {
        RuleFor(x => x.IdAdopcion)
            .GreaterThan(0).WithMessage("El ID de la adopción debe ser mayor a 0.");

        RuleFor(x => x.IdSolicitudAdopcion)
            .GreaterThan(0).WithMessage("Debe seleccionar una solicitud de adopción válida.");

        RuleFor(x => x.Fecha)
            .NotEmpty().WithMessage("La fecha de adopción es obligatoria.");

        RuleFor(x => x.Observacion)
            .MaximumLength(500).When(x => !string.IsNullOrEmpty(x.Observacion))
            .WithMessage("La observación no puede superar los 500 caracteres.");
    }
}