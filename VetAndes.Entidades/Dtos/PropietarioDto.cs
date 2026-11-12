namespace VetAndes.Entidades.Dtos;

public class PropietarioDto
{
    public int Id { get; set; }

    public string Documento { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public string Telefono { get; set; } = null!;

    public string Correo { get; set; } = null!;
}
