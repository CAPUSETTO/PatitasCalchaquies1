using FluentValidation;
using PatitasCalchaquies.API.DTOs;

namespace PatitasCalchaquies.API.Validators;

public class RazaCreateValidator : AbstractValidator<RazaCrearDto>
{
    public RazaCreateValidator()
    {
        RuleFor(x => x.IdEspecie)
            .GreaterThan(0).WithMessage("Debe seleccionar una especie válida.");

        RuleFor(x => x.Nombre)
            .NotEmpty().WithMessage("El nombre de la raza es obligatorio.")
            .MaximumLength(50).WithMessage("El nombre no puede tener más de 50 caracteres.");
    }
}