namespace VetAndes.Negocio;

public class JwtConfig
{
    public string Clave { get; set; } = null!;

    public string Emisor { get; set; } = null!;

    public string Audiencia { get; set; } = null!;

    public int Minutos { get; set; }
}
