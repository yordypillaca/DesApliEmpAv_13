using System;
using System.Collections.Generic;

namespace VetAndes.Entidades;

public partial class Paciente
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string Especie { get; set; } = null!;

    public string Raza { get; set; } = null!;

    public string Sexo { get; set; } = null!;

    public decimal PesoKg { get; set; }

    public DateOnly FechaNacimiento { get; set; }

    public int PropietarioId { get; set; }

    public virtual Propietario Propietario { get; set; } = null!;
}
