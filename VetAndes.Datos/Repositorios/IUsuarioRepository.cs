using VetAndes.Entidades;

namespace VetAndes.Datos.Repositorios;

public interface IUsuarioRepository
{
    Task<Usuario?> BuscarPorCorreoAsync(string correo);

    Task AgregarAsync(Usuario usuario);

    Task GuardarAsync();
}
