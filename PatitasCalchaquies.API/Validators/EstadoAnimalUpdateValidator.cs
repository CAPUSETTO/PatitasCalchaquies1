using FluentValidation;
using PatitasCalchaquies.API.DTOs;

namespace PatitasCalchaquies.API.Validators;

public class EstadoAnimalUpdateValidator : AbstractValidator<EstadoAnimalActualizarDto>
{
    public EstadoAnimalUpdateValidator()
    {
        RuleFor(x => x.IdEstadoAnimal)
            .GreaterThan(0).WithMessage("El ID del estado debe ser mayor a 0.");

        RuleFor(x => x.Nombre)
            .NotEmpty().WithMessage("El nombre del estado es obligatorio.")
            .MaximumLength(50).WithMessage("El nombre no puede superar los 50 caracteres.");
    }
}