using FluentValidation;
using PatitasCalchaquies.API.DTOs;

namespace PatitasCalchaquies.API.Validators;

public class VacunacionCreateValidator : AbstractValidator<VacunacionCrearDto>
{
    public VacunacionCreateValidator()
    {
        RuleFor(x => x.IdAnimal)
            .GreaterThan(0).WithMessage("Debe seleccionar un animal válido.");

        RuleFor(x => x.IdVacuna)
            .GreaterThan(0).WithMessage("Debe seleccionar una vacuna válida.");

        RuleFor(x => x.Fecha)
            .NotEmpty().WithMessage("La fecha de vacunación es obligatoria.")
            .LessThanOrEqualTo(DateTime.Now.AddDays(1)).WithMessage("La fecha no puede ser futura.");

        RuleFor(x => x.ProximaFecha)
            .GreaterThanOrEqualTo(x => x.Fecha)
            .When(x => x.ProximaFecha.HasValue)
            .WithMessage("La próxima fecha de vacunación no puede ser anterior a la fecha de aplicación.");

        RuleFor(x => x.Observacion)
            .MaximumLength(500).When(x => !string.IsNullOrEmpty(x.Observacion))
            .WithMessage("La observación no puede superar los 500 caracteres.");
    }
}