using VetAndes.Entidades;

namespace VetAndes.Datos.Repositorios;

public interface IPropietarioRepository
{
    Task<List<Propietario>> ObtenerTodosAsync();
    Task<Propietario?> ObtenerPorIdAsync(int id);
    Task<bool> ExisteDocumentoAsync(string documento, int? exceptoId = null);
    Task<bool> TienePacientesAsync(int id);
    Task AgregarAsync(Propietario propietario);
    Task EliminarAsync(Propietario propietario);
    Task GuardarCambiosAsync();
}
