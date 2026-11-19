using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using VetAndes.Datos.Repositorios;
using VetAndes.Entidades;
using VetAndes.Entidades.Dtos;

namespace VetAndes.Negocio.Servicios;

public class AuthService : IAuthService
{
    public const string RolRecepcionista = "Recepcionista";
    public const string RolVeterinario = "Veterinario";

    private readonly IUsuarioRepository _usuarios;
    private readonly JwtConfig _jwt;

    public AuthService(IUsuarioRepository usuarios, JwtConfig jwt)
    {
        _usuarios = usuarios;
        _jwt = jwt;
    }

    public async Task RegistrarAsync(RegistroDto dto)
    {
        var correo = dto.Correo.Trim();

        if (await _usuarios.BuscarPorCorreoAsync(correo) != null)
            throw new ReglaNegocioException($"Ya existe un usuario con el correo '{correo}'.");

        var usuario = new Usuario
        {
            Correo = correo,
            ClaveHash = BCrypt.Net.BCrypt.HashPassword(dto.Clave),
            Rol = RolRecepcionista
        };

        await _usuarios.AgregarAsync(usuario);
        await _usuarios.GuardarAsync();
    }

    public async Task<TokenDto?> LoginAsync(LoginDto dto)
    {
        var usuario = await _usuarios.BuscarPorCorreoAsync(dto.Correo.Trim());

        if (usuario == null || !BCrypt.Net.BCrypt.Verify(dto.Clave, usuario.ClaveHash))
            return null;

        return EmitirToken(usuario);
    }

    private TokenDto EmitirToken(Usuario usuario)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, usuario.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, usuario.Correo),
            new Claim("rol", usuario.Rol)
        };

        var clave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Clave));
        var expira = DateTime.UtcNow.AddMinutes(_jwt.Minutos);

        var token = new JwtSecurityToken(
            issuer: _jwt.Emisor,
            audience: _jwt.Audiencia,
            claims: claims,
            expires: expira,
            signingCredentials: new SigningCredentials(clave, SecurityAlgorithms.HmacSha256));

        return new TokenDto
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            Expira = expira
        };
    }
}
