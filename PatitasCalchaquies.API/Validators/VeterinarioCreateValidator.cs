using FluentValidation;
using PatitasCalchaquies.API.DTOs;

namespace PatitasCalchaquies.API.Validators;

public class VeterinarioCreateValidator : AbstractValidator<VeterinarioCrearDto>
{
    public VeterinarioCreateValidator()
    {
        RuleFor(x => x.Matricula)
            .NotEmpty().WithMessage("La matrícula es obligatoria.")
            .MaximumLength(50).WithMessage("La matrícula no puede superar los 50 caracteres.");

        RuleFor(x => x.Nombre)
            .NotEmpty().WithMessage("El nombre es obligatorio.")
            .MaximumLength(50).WithMessage("El nombre no puede superar los 50 caracteres.");

        RuleFor(x => x.Apellido)
            .NotEmpty().WithMessage("El apellido es obligatorio.")
            .MaximumLength(50).WithMessage("El apellido no puede superar los 50 caracteres.");

        RuleFor(x => x.Telefono)
            .NotEmpty().WithMessage("El teléfono es obligatorio.")
            .Matches(@"^\+?\d{1,15}$").WithMessage("El teléfono debe contener solo dígitos numéricos y opcionalmente prefijo '+'.");
    }
}