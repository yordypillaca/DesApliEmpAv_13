using Microsoft.AspNetCore.Mvc;
using VetAndes.Entidades.Dtos;
using VetAndes.Negocio;
using VetAndes.Negocio.Servicios;

namespace VetAndes.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PacientesController : ControllerBase
{
    private static readonly string[] Especies = ["Perro", "Gato", "Ave", "Conejo"];

    private readonly IPacienteService _pacientes;

    public PacientesController(IPacienteService pacientes) => _pacientes = pacientes;

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<PacienteDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<PacienteDto>>> Get(int? propietarioId, string? especie, string? buscar)
    {
        if (!string.IsNullOrWhiteSpace(especie)
            && !Especies.Contains(especie, StringComparer.OrdinalIgnoreCase))
        {
            return BadRequest(new ProblemDetails
            {
                Title = "Especie no válida",
                Detail = "La especie debe ser Perro, Gato, Ave o Conejo.",
                Status = StatusCodes.Status400BadRequest
            });
        }

        var datos = await _pacientes.ListarAsync(propietarioId, especie, buscar);
        return Ok(datos);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(PacienteDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PacienteDto>> GetById(int id)
    {
        var dato = await _pacientes.ObtenerPorIdAsync(id);
        if (dato == null)
            return NotFound();

        return Ok(dato);
    }

    [HttpPost]
    [ProducesResponseType(typeof(PacienteDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<PacienteDto>> Post(PacienteCrearDto dto)
    {
        try
        {
            var creado = await _pacientes.CrearAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = creado.Id }, creado);
        }
        catch (ReglaNegocioException ex)
        {
            return Conflicto(ex);
        }
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Put(int id, PacienteActualizarDto dto)
    {
        try
        {
            var actualizado = await _pacientes.ActualizarAsync(id, dto);
            if (!actualizado)
                return NotFound();

            return NoContent();
        }
        catch (ReglaNegocioException ex)
        {
            return Conflicto(ex);
        }
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var eliminado = await _pacientes.EliminarAsync(id);
        if (!eliminado)
            return NotFound();

        return NoContent();
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
