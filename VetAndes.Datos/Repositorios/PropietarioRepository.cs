using Microsoft.EntityFrameworkCore;
using VetAndes.Datos.Models;
using VetAndes.Entidades;

namespace VetAndes.Datos.Repositorios;

public class PropietarioRepository : IPropietarioRepository
{
    private readonly VetAndesContext _context;

    public PropietarioRepository(VetAndesContext context) => _context = context;

    public async Task<List<Propietario>> ObtenerTodosAsync()
    {
        return await _context.Propietarios
            .AsNoTracking()
            .OrderBy(p => p.Nombre)
            .ToListAsync();
    }

    public async Task<Propietario?> ObtenerPorIdAsync(int id)
    {
        return await _context.Propietarios
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<bool> ExisteDocumentoAsync(string documento, int? exceptoId = null)
    {
        return await _context.Propietarios.AnyAsync(p =>
            p.Documento == documento && (exceptoId == null || p.Id != exceptoId));
    }

    public async Task<bool> TienePacientesAsync(int id)
    {
        return await _context.Pacientes.AnyAsync(p => p.PropietarioId == id);
    }

    public async Task AgregarAsync(Propietario propietario)
    {
        await _context.Propietarios.AddAsync(propietario);
        await _context.SaveChangesAsync();
    }

    public async Task EliminarAsync(Propietario propietario)
    {
        _context.Propietarios.Remove(propietario);
        await _context.SaveChangesAsync();
    }

    public async Task GuardarCambiosAsync()
    {
        await _context.SaveChangesAsync();
    }
}
