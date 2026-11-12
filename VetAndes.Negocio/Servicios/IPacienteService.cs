using VetAndes.Entidades.Dtos;

namespace VetAndes.Negocio.Servicios;

public interface IPacienteService
{
    Task<List<PacienteDto>> ListarAsync(int? propietarioId, string? especie, string? buscar);
    Task<List<PacienteDto>?> ListarPorPropietarioAsync(int propietarioId);
    Task<PacienteDto?> ObtenerPorIdAsync(int id);
    Task<PacienteDto> CrearAsync(PacienteCrearDto dto);
    Task<bool> ActualizarAsync(int id, PacienteActualizarDto dto);
    Task<bool> EliminarAsync(int id);
}
