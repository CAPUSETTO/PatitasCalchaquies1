using FluentValidation;
using PatitasCalchaquies.API.DTOs;

namespace PatitasCalchaquies.API.Validators;

public class VacunaCreateValidator : AbstractValidator<VacunaCrearDto>
{
    public VacunaCreateValidator()
    {
        RuleFor(x => x.nombre)
            .NotEmpty().WithMessage("El nombre de la vacuna es obligatorio.")
            .MaximumLength(50).WithMessage("El nombre no puede tener más de 50 caracteres.");
    }
}