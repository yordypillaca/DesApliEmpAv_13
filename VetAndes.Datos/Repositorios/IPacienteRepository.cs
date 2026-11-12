using VetAndes.Entidades;

namespace VetAndes.Datos.Repositorios;

public interface IPacienteRepository
{
    Task<List<Paciente>> ObtenerAsync(int? propietarioId, string? especie, string? buscar);
    Task<List<Paciente>> ObtenerPorPropietarioAsync(int propietarioId);
    Task<Paciente?> ObtenerPorIdAsync(int id);
    Task<bool> ExistePropietarioAsync(int propietarioId);
    Task<bool> ExisteNombreAsync(int propietarioId, string nombre, int? exceptoId = null);
    Task AgregarAsync(Paciente paciente);
    Task EliminarAsync(Paciente paciente);
    Task GuardarCambiosAsync();
}
