using System.ComponentModel.DataAnnotations;

namespace VetAndes.Entidades.Dtos;

[AttributeUsage(AttributeTargets.Property)]
public sealed class FechaObligatoriaAttribute : ValidationAttribute
{
    public FechaObligatoriaAttribute()
    {
        ErrorMessage = "La fecha de nacimiento es obligatoria y no puede ser posterior a hoy.";
    }

    public override bool IsValid(object? value)
    {
        if (value is not DateOnly fecha || fecha == default)
            return false;

        return fecha <= DateOnly.FromDateTime(DateTime.Today);
    }
}
