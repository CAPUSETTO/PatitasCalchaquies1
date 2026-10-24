using FluentValidation;
using PatitasCalchaquies.API.DTOs;

namespace PatitasCalchaquies.API.Validators;

public class UsuarioRolCreateValidator : AbstractValidator<UsuarioRolCrearDto>
{
    public UsuarioRolCreateValidator()
    {
        RuleFor(x => x.IdUsuario)
            .GreaterThan(0).WithMessage("Debe seleccionar un usuario válido.");

        RuleFor(x => x.IdRol)
            .GreaterThan(0).WithMessage("Debe seleccionar un rol válido.");
    }
}