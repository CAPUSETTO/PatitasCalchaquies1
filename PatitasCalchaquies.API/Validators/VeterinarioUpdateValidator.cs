using FluentValidation;
using PatitasCalchaquies.API.DTOs;

namespace PatitasCalchaquies.API.Validators;

public class VeterinarioUpdateValidator : AbstractValidator<VeterinarioActualizarDto>
{
    public VeterinarioUpdateValidator()
    {
        RuleFor(x => x.IdVeterinario)
            .GreaterThan(0).WithMessage("El ID del veterinario debe ser mayor a 0.");

        RuleFor(x => x.Matricula)
            .NotEmpty().WithMessage("La matrícula es obligatoria.")
            .MaximumLength(50).WithMessage("La matrícula no puede superar los 50 caracteres.");

        RuleFor(x => x.Nombre)
            .NotEmpty().WithMessage("El nombre es obligatorio.")
            .MaximumLength(50).WithMessage("El nombre no puede superar los 50 caracteres.");

        RuleFor(x => x.Apellido)
            .NotEmpty().WithMessage("El apellido es obligatorio.")
            .MaximumLength(50).WithMessage("El apellido no puede superar los 50 caracteres.");

        RuleFor(x => x.Telefono)
            .NotEmpty().WithMessage("El teléfono es obligatorio.")
            .Matches(@"^\+?[\d\s\-()]{6,20}$").WithMessage("El teléfono debe tener un formato válido.");
    }
}
