using FluentValidation;
using PatitasCalchaquies.API.DTOs;

namespace PatitasCalchaquies.API.Validadores;

public class RazaActualizarValidator : AbstractValidator<RazaActualizarDto>
{
    public RazaActualizarValidator()
    {
        RuleFor(x => x.IdRaza)
            .GreaterThan(0).WithMessage("El ID de la raza debe ser mayor a 0.");

        RuleFor(x => x.IdEspecie)
            .GreaterThan(0).WithMessage("Debe seleccionar una especie válida.");

        RuleFor(x => x.Nombre)
            .NotEmpty().WithMessage("El nombre de la raza es obligatorio.")
            .MaximumLength(50).WithMessage("El nombre de la raza no puede superar los 50 caracteres.");
    }
}