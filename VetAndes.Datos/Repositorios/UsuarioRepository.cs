using Microsoft.EntityFrameworkCore;
using VetAndes.Datos.Models;
using VetAndes.Entidades;

namespace VetAndes.Datos.Repositorios;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly VetAndesContext _context;

    public UsuarioRepository(VetAndesContext context) => _context = context;

    public async Task<Usuario?> BuscarPorCorreoAsync(string correo)
    {
        return await _context.Usuarios
            .FirstOrDefaultAsync(u => u.Correo == correo);
    }

    public async Task AgregarAsync(Usuario usuario)
    {
        await _context.Usuarios.AddAsync(usuario);
    }

    public async Task GuardarAsync()
    {
        await _context.SaveChangesAsync();
    }
}
