using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VetAndes.Entidades.Dtos;
using VetAndes.Negocio;
using VetAndes.Negocio.Servicios;

namespace VetAndes.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PropietariosController : ControllerBase
{
    private readonly IPropietarioService _propietarios;
    private readonly IPacienteService _pacientes;

    public PropietariosController(IPropietarioService propietarios, IPacienteService pacientes)
    {
        _propietarios = propietarios;
        _pacientes = pacientes;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<PropietarioDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<IEnumerable<PropietarioDto>>> Get()
    {
        var datos = await _propietarios.ListarAsync();
        return Ok(datos);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(PropietarioDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PropietarioDto>> GetById(int id)
    {
        var dato = await _propietarios.ObtenerPorIdAsync(id);
        if (dato == null)
            return NotFound();

        return Ok(dato);
    }

    [HttpGet("{id:int}/pacientes")]
    [ProducesResponseType(typeof(IEnumerable<PacienteDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<PacienteDto>>> GetPacientes(int id)
    {
        var datos = await _pacientes.ListarPorPropietarioAsync(id);
        if (datos == null)
            return NotFound();

        return Ok(datos);
    }

    [Authorize(Roles = AuthService.RolRecepcionista)]
    [HttpPost]
    [ProducesResponseType(typeof(PropietarioDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<PropietarioDto>> Post(PropietarioCrearDto dto)
    {
        try
        {
            var creado = await _propietarios.CrearAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = creado.Id }, creado);
        }
        catch (ReglaNegocioException ex)
        {
            return Conflicto(ex);
        }
    }

    [Authorize(Roles = AuthService.RolRecepcionista)]
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Put(int id, PropietarioActualizarDto dto)
    {
        try
        {
            var actualizado = await _propietarios.ActualizarAsync(id, dto);
            if (!actualizado)
                return NotFound();

            return NoContent();
        }
        catch (ReglaNegocioException ex)
        {
            return Conflicto(ex);
        }
    }

    [Authorize(Roles = AuthService.RolVeterinario)]
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var eliminado = await _propietarios.EliminarAsync(id);
            if (!eliminado)
                return NotFound();

            return NoContent();
        }
        catch (ReglaNegocioException ex)
        {
            return Conflicto(ex);
        }
    }

    private ObjectResult Conflicto(ReglaNegocioException ex)
    {
        return Conflict(new ProblemDetails
        {
            Title = "Regla de negocio",
            Detail = ex.Message,
            Status = StatusCodes.Status409Conflict
        });
    }
}
