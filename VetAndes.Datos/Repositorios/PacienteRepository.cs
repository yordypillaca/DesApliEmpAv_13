using Microsoft.EntityFrameworkCore;
using VetAndes.Datos.Models;
using VetAndes.Entidades;

namespace VetAndes.Datos.Repositorios;

public class PacienteRepository : IPacienteRepository
{
    private readonly VetAndesContext _context;

    public PacienteRepository(VetAndesContext context) => _context = context;

    public async Task<List<Paciente>> ObtenerAsync(int? propietarioId, string? especie, string? buscar)
    {
        var consulta = _context.Pacientes
            .AsNoTracking()
            .Include(p => p.Propietario)
            .AsQueryable();

        if (propietarioId != null)
            consulta = consulta.Where(p => p.PropietarioId == propietarioId);

        if (!string.IsNullOrWhiteSpace(especie))
            consulta = consulta.Where(p => p.Especie == especie);

        if (!string.IsNullOrWhiteSpace(buscar))
        {
            var texto = buscar.Trim();
            consulta = consulta.Where(p =>
                p.Nombre.Contains(texto) || p.Raza.Contains(texto));
        }

        return await consulta
            .OrderBy(p => p.Nombre)
            .ToListAsync();
    }

    public async Task<List<Paciente>> ObtenerPorPropietarioAsync(int propietarioId)
    {
        return await _context.Pacientes
            .AsNoTracking()
            .Include(p => p.Propietario)
            .Where(p => p.PropietarioId == propietarioId)
            .OrderBy(p => p.Nombre)
            .ToListAsync();
    }

    public async Task<Paciente?> ObtenerPorIdAsync(int id)
    {
        return await _context.Pacientes
            .Include(p => p.Propietario)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<bool> ExistePropietarioAsync(int propietarioId)
    {
        return await _context.Propietarios.AnyAsync(p => p.Id == propietarioId);
    }

    public async Task<bool> ExisteNombreAsync(int propietarioId, string nombre, int? exceptoId = null)
    {
        return await _context.Pacientes.AnyAsync(p =>
            p.PropietarioId == propietarioId
            && p.Nombre == nombre
            && (exceptoId == null || p.Id != exceptoId));
    }

    public async Task AgregarAsync(Paciente paciente)
    {
        await _context.Pacientes.AddAsync(paciente);
        await _context.SaveChangesAsync();
    }

    public async Task EliminarAsync(Paciente paciente)
    {
        _context.Pacientes.Remove(paciente);
        await _context.SaveChangesAsync();
    }

    public async Task GuardarCambiosAsync()
    {
        await _context.SaveChangesAsync();
    }
}
