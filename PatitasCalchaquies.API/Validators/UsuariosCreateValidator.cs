using FluentValidation;
using PatitasCalchaquies.API.DTOs;

namespace PatitasCalchaquies.API.Validators;

public class UsuarioCreateValidator : AbstractValidator<UsuarioCrearDto>
{
    public UsuarioCreateValidator()
    {
        RuleFor(x => x.User)
            .NotEmpty().WithMessage("El nombre de usuario es obligatorio.")
            .MaximumLength(50).WithMessage("El nombre de usuario no puede superar los 50 caracteres.");

        RuleFor(x => x.PasswordHash)
            .NotEmpty().WithMessage("La contraseña es obligatoria.")
            .MaximumLength(255).WithMessage("El hash de la contraseña no puede superar los 255 caracteres.");

        RuleFor(x => x.FechaCreacion)
            .NotEmpty().WithMessage("La fecha de creación es obligatoria.");
    }
}