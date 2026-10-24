using FluentValidation;
using PatitasCalchaquies.API.DTOs;

namespace PatitasCalchaquies.API.Validators;

public class PredioCreateValidator : AbstractValidator<PredioCrearDto>
{
    public PredioCreateValidator()
    {
        RuleFor(x => x.Nombre)
            .NotEmpty().WithMessage("El nombre del predio es obligatorio.")
            .MaximumLength(100).WithMessage("El nombre no puede superar los 100 caracteres.");

        RuleFor(x => x.Direccion)
            .MaximumLength(150).When(x => !string.IsNullOrEmpty(x.Direccion))
            .WithMessage("La dirección no puede superar los 150 caracteres.");

        RuleFor(x => x.Descripcion)
            .MaximumLength(255).When(x => !string.IsNullOrEmpty(x.Descripcion))
            .WithMessage("La descripción no puede superar los 255 caracteres.");
    }
}