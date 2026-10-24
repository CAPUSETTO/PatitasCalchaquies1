using FluentValidation;
using PatitasCalchaquies.API.DTOs;

namespace PatitasCalchaquies.API.Validators;

public class SectorUpdateValidator : AbstractValidator<SectorActualizarDto>
{
    public SectorUpdateValidator()
    {
        RuleFor(x => x.IdSector)
            .GreaterThan(0).WithMessage("El ID del sector debe ser mayor a 0.");

        RuleFor(x => x.Nombre)
            .NotEmpty().WithMessage("El nombre del sector es obligatorio.")
            .MaximumLength(100).WithMessage("El nombre no puede superar los 100 caracteres.");

        RuleFor(x => x.IdPredio)
            .GreaterThan(0).WithMessage("Debe seleccionar un predio válido.");

        RuleFor(x => x.Capacidad)
            .GreaterThanOrEqualTo(0).When(x => x.Capacidad.HasValue)
            .WithMessage("La capacidad no puede ser negativa.");

        RuleFor(x => x.Descripcion)
            .MaximumLength(255).When(x => !string.IsNullOrEmpty(x.Descripcion))
            .WithMessage("La descripción no puede superar los 255 caracteres.");
    }
}