using FluentValidation;
using PatitasCalchaquies.API.DTOs;

namespace PatitasCalchaquies.API.Validators;

public class AnimalUpdateValidator : AbstractValidator<AnimalActualizarDto>
{
    public AnimalUpdateValidator()
    {
        RuleFor(x => x.IdAnimal)
            .GreaterThan(0).WithMessage("El ID del animal debe ser mayor a 0.");

        RuleFor(x => x.Nombre)
            .NotEmpty().WithMessage("El nombre del animal es obligatorio.")
            .MaximumLength(100).WithMessage("El nombre no puede superar los 100 caracteres.");

        RuleFor(x => x.CodigoIdentificacion)
            .NotEmpty().WithMessage("El código de identificación es obligatorio.")
            .MaximumLength(50).WithMessage("El código no puede superar los 50 caracteres.");

        RuleFor(x => x.IdEspecie)
            .GreaterThan(0).WithMessage("Debe seleccionar una especie válida.");
    }
}