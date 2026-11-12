using VetAndes.Entidades.Dtos;

namespace VetAndes.Negocio.Servicios;

public interface IPropietarioService
{
    Task<List<PropietarioDto>> ListarAsync();
    Task<PropietarioDto?> ObtenerPorIdAsync(int id);
    Task<PropietarioDto> CrearAsync(PropietarioCrearDto dto);
    Task<bool> ActualizarAsync(int id, PropietarioActualizarDto dto);
    Task<bool> EliminarAsync(int id);
}
