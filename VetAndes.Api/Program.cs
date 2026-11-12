using VetAndes.Datos.Models;
using VetAndes.Datos.Repositorios;
using VetAndes.Negocio;
using VetAndes.Negocio.Servicios;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.local.json", optional: true, reloadOnChange: true);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

var cadena = builder.Configuration.GetConnectionString("VetAndes");
if (string.IsNullOrWhiteSpace(cadena))
    throw new InvalidOperationException("Defina la cadena de conexión 'VetAndes'.");

builder.Services.AddDbContext<VetAndesContext>(options =>
    options.UseSqlServer(cadena));

builder.Services.AddScoped<IPropietarioRepository, PropietarioRepository>();
builder.Services.AddScoped<IPacienteRepository, PacienteRepository>();
builder.Services.AddScoped<IPropietarioService, PropietarioService>();
builder.Services.AddScoped<IPacienteService, PacienteService>();
builder.Services.AddAutoMapper(cfg => cfg.AddProfile<MappingProfile>());

var app = builder.Build();

app.MapOpenApi();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/openapi/v1.json", "VetAndes");
    options.RoutePrefix = "swagger";
});

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
