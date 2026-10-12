using FluentValidation;
using PatitasCalchaquies.API.DTOs;

namespace PatitasCalchaquies.API.Validators;

public class MedicamentoCreateValidator : AbstractValidator<MedicamentoCrearDto>
{
    public MedicamentoCreateValidator()
    {
        RuleFor(x => x.nombre)
            .NotEmpty().WithMessage("El nombre del medicamento es obligatorio.")
            .MaximumLength(50).WithMessage("El nombre no puede tener más de 50 caracteres.");
    }

}