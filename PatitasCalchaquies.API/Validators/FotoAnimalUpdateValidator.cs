using FluentValidation;
using PatitasCalchaquies.API.DTOs;

namespace PatitasCalchaquies.API.Validators;

public class FotoAnimalUpdateValidator : AbstractValidator<FotoAnimalActualizarDto>
{
    public FotoAnimalUpdateValidator()
    {
        RuleFor(x => x.IdFotoAnimal)
            .GreaterThan(0).WithMessage("El ID de la foto debe ser mayor a 0.");

        RuleFor(x => x.IdAnimal)
            .GreaterThan(0).WithMessage("Debe seleccionar un animal válido.");

        RuleFor(x => x.Ruta)
            .NotEmpty().WithMessage("La ruta del archivo de la foto es obligatoria.")
            .MaximumLength(255).WithMessage("La ruta no puede superar los 255 caracteres.");

        RuleFor(x => x.Fecha)
            .NotEmpty().WithMessage("La fecha es obligatoria.");
    }
}