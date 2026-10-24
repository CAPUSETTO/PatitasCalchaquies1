using FluentValidation;
using PatitasCalchaquies.API.DTOs;

namespace PatitasCalchaquies.API.Validadores;

public class DevolucionAdopcionUpdateValidator : AbstractValidator<DevolucionAdopcionActualizarDto>
{
    public DevolucionAdopcionUpdateValidator()
    {
        RuleFor(x => x.IdDevolucionAdopcion)
            .GreaterThan(0).WithMessage("El ID de la devolución debe ser mayor a 0.");

        RuleFor(x => x.IdAdopcion)
            .GreaterThan(0).WithMessage("Debe seleccionar una adopción válida.");

        RuleFor(x => x.Fecha)
            .NotEmpty().WithMessage("La fecha de devolución es obligatoria.");

        RuleFor(x => x.Motivo)
            .NotEmpty().WithMessage("El motivo es obligatorio.")
            .MaximumLength(255).WithMessage("El motivo no puede superar los 255 caracteres.");

        RuleFor(x => x.Observacion)
            .MaximumLength(500).When(x => !string.IsNullOrEmpty(x.Observacion))
            .WithMessage("La observación no puede superar los 500 caracteres.");
    }
}