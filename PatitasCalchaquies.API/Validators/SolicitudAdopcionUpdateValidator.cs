using FluentValidation;
using PatitasCalchaquies.API.DTOs;

namespace PatitasCalchaquies.API.Validators;

public class SolicitudAdopcionUpdateValidator : AbstractValidator<SolicitudAdopcionActualizarDto>
{
    public SolicitudAdopcionUpdateValidator()
    {
        RuleFor(x => x.IdSolicitudAdopcion)
            .GreaterThan(0).WithMessage("El ID de la solicitud debe ser mayor a 0.");

        RuleFor(x => x.IdPersona)
            .GreaterThan(0).WithMessage("Debe seleccionar una persona válida.");

        RuleFor(x => x.IdAnimal)
            .GreaterThan(0).WithMessage("Debe seleccionar un animal válido.");

        RuleFor(x => x.Fecha)
            .NotEmpty().WithMessage("La fecha de solicitud es obligatoria.");

        RuleFor(x => x.Estado)
            .NotEmpty().WithMessage("El estado de la solicitud es obligatorio.")
            .MaximumLength(50).WithMessage("El estado no puede superar los 50 caracteres.");

        RuleFor(x => x.Observacion)
            .MaximumLength(500).When(x => !string.IsNullOrEmpty(x.Observacion))
            .WithMessage("La observación no puede superar los 500 caracteres.");
    }
}