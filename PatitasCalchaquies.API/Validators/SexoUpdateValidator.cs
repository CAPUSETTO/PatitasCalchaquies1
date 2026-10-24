using FluentValidation;
using PatitasCalchaquies.API.DTOs;

namespace PatitasCalchaquies.API.Validators;

public class SexoUpdateValidator : AbstractValidator<SexoActualizarDto>
{
    public SexoUpdateValidator()
    {
        RuleFor(x => x.IdSexo)
            .GreaterThan(0).WithMessage("El ID del sexo debe ser mayor a 0.");

        RuleFor(x => x.Nombre)
            .NotEmpty().WithMessage("El nombre del sexo es obligatorio.")
            .MaximumLength(20).WithMessage("El nombre no puede superar los 20 caracteres.");
    }
}