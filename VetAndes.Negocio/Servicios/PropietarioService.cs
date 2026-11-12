using AutoMapper;
using VetAndes.Datos.Repositorios;
using VetAndes.Entidades;
using VetAndes.Entidades.Dtos;

namespace VetAndes.Negocio.Servicios;

public class PropietarioService : IPropietarioService
{
    private readonly IPropietarioRepository _repositorio;
    private readonly IMapper _mapper;

    public PropietarioService(IPropietarioRepository repositorio, IMapper mapper)
    {
        _repositorio = repositorio;
        _mapper = mapper;
    }

    public async Task<List<PropietarioDto>> ListarAsync()
    {
        var propietarios = await _repositorio.ObtenerTodosAsync();
        return _mapper.Map<List<PropietarioDto>>(propietarios);
    }

    public async Task<PropietarioDto?> ObtenerPorIdAsync(int id)
    {
        var propietario = await _repositorio.ObtenerPorIdAsync(id);
        if (propietario == null)
            return null;

        return _mapper.Map<PropietarioDto>(propietario);
    }

    public async Task<PropietarioDto> CrearAsync(PropietarioCrearDto dto)
    {
        if (await _repositorio.ExisteDocumentoAsync(dto.Documento))
            throw new ReglaNegocioException($"Ya existe un propietario con el documento '{dto.Documento}'.");

        var propietario = _mapper.Map<Propietario>(dto);
        await _repositorio.AgregarAsync(propietario);
        return _mapper.Map<PropietarioDto>(propietario);
    }

    public async Task<bool> ActualizarAsync(int id, PropietarioActualizarDto dto)
    {
        var actual = await _repositorio.ObtenerPorIdAsync(id);
        if (actual == null)
            return false;

        if (await _repositorio.ExisteDocumentoAsync(dto.Documento, id))
            throw new ReglaNegocioException($"Ya existe un propietario con el documento '{dto.Documento}'.");

        _mapper.Map(dto, actual);
        await _repositorio.GuardarCambiosAsync();
        return true;
    }

    public async Task<bool> EliminarAsync(int id)
    {
        var actual = await _repositorio.ObtenerPorIdAsync(id);
        if (actual == null)
            return false;

        if (await _repositorio.TienePacientesAsync(id))
            throw new ReglaNegocioException("No se puede eliminar un propietario que tiene pacientes registrados.");

        await _repositorio.EliminarAsync(actual);
        return true;
    }
}
