using VetAndes.Entidades.Dtos;

namespace VetAndes.Negocio.Servicios;

public interface IAuthService
{
    Task RegistrarAsync(RegistroDto dto);

    Task<TokenDto?> LoginAsync(LoginDto dto);
}
