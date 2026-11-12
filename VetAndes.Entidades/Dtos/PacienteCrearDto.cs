using System.ComponentModel.DataAnnotations;

namespace VetAndes.Entidades.Dtos;

public class PacienteCrearDto
{
    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(80, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 80 caracteres.")]
    public string Nombre { get; set; } = null!;

    [Required(ErrorMessage = "La especie es obligatoria.")]
    [RegularExpression("^(Perro|Gato|Ave|Conejo)$", ErrorMessage = "La especie debe ser Perro, Gato, Ave o Conejo.")]
    public string Especie { get; set; } = null!;

    [Required(ErrorMessage = "La raza es obligatoria.")]
    [StringLength(60, MinimumLength = 2, ErrorMessage = "La raza debe tener entre 2 y 60 caracteres.")]
    public string Raza { get; set; } = null!;

    [Required(ErrorMessage = "El sexo es obligatorio.")]
    [RegularExpression("^(Macho|Hembra)$", ErrorMessage = "El sexo debe ser Macho o Hembra.")]
    public string Sexo { get; set; } = null!;

    [Range(typeof(decimal), "0.01", "200", ErrorMessage = "El peso debe estar entre 0.01 y 200 kg.")]
    public decimal PesoKg { get; set; }

    [FechaObligatoria]
    public DateOnly FechaNacimiento { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "El propietario es obligatorio.")]
    public int PropietarioId { get; set; }
}
