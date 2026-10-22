using FluentValidation;
using FluentValidation.Validators;
using PatitasCalchaquies.API.DTOs;

namespace PatitasCalchaquies.API.Validators;

public class AnimalCreateValidator : AbstractValidator<AnimalCrearDto>
{
    public AnimalCreateValidator()
    {
        RuleFor(x => x.Nombre)
            .NotEmpty().WithMessage("El nombre del animal es obligatorio.")
            .MaximumLength(100).WithMessage("El nombre no puede superar los 100 caracteres.");

        RuleFor(x => x.CodigoIdentificacion)
            .NotEmpty().WithMessage("El código de identificación es obligatorio.")
            .MaximumLength(50).WithMessage("El código de identificación no puede superar los 50 caracteres.");

        RuleFor(x => x.IdEspecie)
            .GreaterThan(0).WithMessage("Debe seleccionar una especie válida.");

        RuleFor(x => x.FechaIngreso)
            .NotEmpty().WithMessage("La fecha de ingreso es obligatoria.")
            .LessThanOrEqualTo(DateTime.Now.AddDays(1)).WithMessage("La fecha de ingreso no puede ser futura.");
    }
}