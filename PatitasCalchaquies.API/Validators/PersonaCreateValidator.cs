using FluentValidation;
using PatitasCalchaquies.API.DTOs;

namespace PatitasCalchaquies.API.Validators;

public class PersonaCreateValidator : AbstractValidator<PersonaCrearDto>
{
    public PersonaCreateValidator()
    {
        RuleFor(x=>x.Apellido)
            .NotEmpty().WithMessage("La Apellido no puede estar vacio")
            .MaximumLength(50).WithMessage("El apellido no puede tener mas de 50 caracteres");
        RuleFor(x=>x.Nombre)
            .NotEmpty().WithMessage("El nombre no puede estar vacio")
            .MaximumLength(50).WithMessage("El nombre no puede tener mas de 50 caracteres");
        RuleFor(x => x.Dni)
            .NotEmpty()
            .WithMessage("El DNI es obligatorio.")
            .Matches(@"^\d{7,8}$")
            .WithMessage("El DNI debe contener entre 7 y 8 dígitos.");
        RuleFor(x => x.Telefono)
            .NotEmpty()
            .WithMessage("El Número de telefono es obligatorio.")
            .Matches(@"^\+?\d{1,15}$")
            .WithMessage("El teléfono debe contener solo dígitos y puede incluir un prefijo '+'.");
        RuleFor(x => x.Email)
            .EmailAddress().When(x => !string.IsNullOrEmpty(x.Email))
            .WithMessage("El email no tiene un formato válido.");
        RuleFor(x => x.Email)
            .EmailAddress().When(x => !string.IsNullOrEmpty(x.Email))
            .WithMessage("El email no tiene un formato válido.");
        RuleFor(x => x.Direccion)
            .NotEmpty()
            .WithMessage("La dirección es obligatoria.")
            .MaximumLength(100).WithMessage("La dirección no puede tener mas de 100 caracteres");
    }
}