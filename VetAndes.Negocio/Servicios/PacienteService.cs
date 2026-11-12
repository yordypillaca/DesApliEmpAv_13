using AutoMapper;
using VetAndes.Datos.Repositorios;
using VetAndes.Entidades;
using VetAndes.Entidades.Dtos;

namespace VetAndes.Negocio.Servicios;

public class PacienteService : IPacienteService
{
    private readonly IPacienteRepository _repositorio;
    private readonly IMapper _mapper;

    public PacienteService(IPacienteRepository repositorio, IMapper mapper)
    {
        _repositorio = repositorio;
        _mapper = mapper;
    }

    public async Task<List<PacienteDto>> ListarAsync(int? propietarioId, string? especie, string? buscar)
    {
        var pacientes = await _repositorio.ObtenerAsync(propietarioId, especie, buscar);
        return _mapper.Map<List<PacienteDto>>(pacientes);
    }

    public async Task<List<PacienteDto>?> ListarPorPropietarioAsync(int propietarioId)
    {
        if (!await _repositorio.ExistePropietarioAsync(propietarioId))
            return null;

        var pacientes = await _repositorio.ObtenerPorPropietarioAsync(propietarioId);
        return _mapper.Map<List<PacienteDto>>(pacientes);
    }

    public async Task<PacienteDto?> ObtenerPorIdAsync(int id)
    {
        var paciente = await _repositorio.ObtenerPorIdAsync(id);
        if (paciente == null)
            return null;

        return _mapper.Map<PacienteDto>(paciente);
    }

    public async Task<PacienteDto> CrearAsync(PacienteCrearDto dto)
    {
        if (!await _repositorio.ExistePropietarioAsync(dto.PropietarioId))
            throw new ReglaNegocioException("El propietario indicado no existe.");

        if (await _repositorio.ExisteNombreAsync(dto.PropietarioId, dto.Nombre))
            throw new ReglaNegocioException($"Ya existe un paciente llamado '{dto.Nombre}' para este propietario.");

        var paciente = _mapper.Map<Paciente>(dto);
        await _repositorio.AgregarAsync(paciente);

        var guardado = await _repositorio.ObtenerPorIdAsync(paciente.Id);
        return _mapper.Map<PacienteDto>(guardado!);
    }

    public async Task<bool> ActualizarAsync(int id, PacienteActualizarDto dto)
    {
        var actual = await _repositorio.ObtenerPorIdAsync(id);
        if (actual == null)
            return false;

        if (!await _repositorio.ExistePropietarioAsync(dto.PropietarioId))
            throw new ReglaNegocioException("El propietario indicado no existe.");

        if (await _repositorio.ExisteNombreAsync(dto.PropietarioId, dto.Nombre, id))
            throw new ReglaNegocioException($"Ya existe un paciente llamado '{dto.Nombre}' para este propietario.");

        _mapper.Map(dto, actual);
        await _repositorio.GuardarCambiosAsync();
        return true;
    }

    public async Task<bool> EliminarAsync(int id)
    {
        var actual = await _repositorio.ObtenerPorIdAsync(id);
        if (actual == null)
            return false;

        await _repositorio.EliminarAsync(actual);
        return true;
    }
}
