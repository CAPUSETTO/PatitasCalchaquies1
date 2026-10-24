using FluentValidation;
using PatitasCalchaquies.API.DTOs;

namespace PatitasCalchaquies.API.Validators;

public class SexoCreateValidator : AbstractValidator<SexoCrearDto>
{
    public SexoCreateValidator()
    {
        RuleFor(x => x.Nombre)
            .NotEmpty().WithMessage("El nombre del sexo es obligatorio.")
            .MaximumLength(20).WithMessage("El nombre no puede superar los 20 caracteres.");
    }
}