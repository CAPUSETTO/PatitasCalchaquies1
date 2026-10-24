using FluentValidation;
using PatitasCalchaquies.API.DTOs;

namespace PatitasCalchaquies.API.Validators;

public class EspecieUpdateValidator : AbstractValidator<EspecieActualizarDto>
{
    public EspecieUpdateValidator()
    {
        RuleFor(x => x.IdEspecie)
            .GreaterThan(0).WithMessage("El ID de la especie debe ser mayor a 0.");

        RuleFor(x => x.Nombre)
            .NotEmpty().WithMessage("El nombre de la especie es obligatorio.")
            .MaximumLength(50).WithMessage("El nombre no puede superar los 50 caracteres.");
    }
}