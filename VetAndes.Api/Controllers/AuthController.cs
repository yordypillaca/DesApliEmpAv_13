using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VetAndes.Entidades.Dtos;
using VetAndes.Negocio;
using VetAndes.Negocio.Servicios;

namespace VetAndes.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _auth;

    public AuthController(IAuthService auth) => _auth = auth;

    [AllowAnonymous]
    [HttpPost("registro")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Registro(RegistroDto dto)
    {
        try
        {
            await _auth.RegistrarAsync(dto);
            return StatusCode(StatusCodes.Status201Created);
        }
        catch (ReglaNegocioException ex)
        {
            return Conflict(new ProblemDetails
            {
                Title = "Regla de negocio",
                Detail = ex.Message,
                Status = StatusCodes.Status409Conflict
            });
        }
    }

    [AllowAnonymous]
    [HttpPost("login")]
    [ProducesResponseType(typeof(TokenDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<TokenDto>> Login(LoginDto dto)
    {
        var token = await _auth.LoginAsync(dto);
        if (token == null)
            return Unauthorized();

        return Ok(token);
    }

    [Authorize]
    [HttpGet("perfil")]
    [ProducesResponseType(typeof(PerfilDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<PerfilDto> Perfil()
    {
        var correo = User.FindFirstValue(JwtRegisteredClaimNames.Email)
            ?? User.FindFirstValue(ClaimTypes.Email);
        var rol = User.FindFirstValue("rol")
            ?? User.FindFirstValue(ClaimTypes.Role);

        return Ok(new PerfilDto
        {
            Correo = correo ?? string.Empty,
            Rol = rol ?? string.Empty
        });
    }
}
