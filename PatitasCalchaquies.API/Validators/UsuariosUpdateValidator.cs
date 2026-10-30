using FluentValidation;
using PatitasCalchaquies.API.DTOs;

namespace PatitasCalchaquies.API.Validators;

public class UsuarioUpdateValidator : AbstractValidator<UsuarioActualizarDto>
{
    public UsuarioUpdateValidator()
    {
        RuleFor(x => x.IdUsuario)
            .GreaterThan(0).WithMessage("El ID del usuario debe ser mayor a 0.");

        RuleFor(x => x.User)
            .NotEmpty().WithMessage("El nombre de usuario es obligatorio.")
            .MaximumLength(50).WithMessage("El nombre de usuario no puede superar los 50 caracteres.");

        RuleFor(x => x.PasswordHash)
            .MaximumLength(255).WithMessage("El hash de la contraseña no puede superar los 255 caracteres.")
            .When(x => !string.IsNullOrEmpty(x.PasswordHash));
    }
}
