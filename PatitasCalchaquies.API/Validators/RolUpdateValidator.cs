using FluentValidation;
using PatitasCalchaquies.API.DTOs;

namespace PatitasCalchaquies.API.Validators;

public class RolUpdateValidator : AbstractValidator<RolActualizarDto>
{
    public RolUpdateValidator()
    {
        RuleFor(x => x.IdRol)
            .GreaterThan(0).WithMessage("El ID del rol debe ser mayor a 0.");

        RuleFor(x => x.Nombre)
            .NotEmpty().WithMessage("El nombre del rol es obligatorio.")
            .MaximumLength(50).WithMessage("El nombre no puede superar los 50 caracteres.");

        RuleFor(x => x.Descripcion)
            .MaximumLength(255).When(x => !string.IsNullOrEmpty(x.Descripcion))
            .WithMessage("La descripción no puede superar los 255 caracteres.");
    }
}