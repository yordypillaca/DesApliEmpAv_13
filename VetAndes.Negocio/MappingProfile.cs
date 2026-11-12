using AutoMapper;
using VetAndes.Entidades;
using VetAndes.Entidades.Dtos;

namespace VetAndes.Negocio;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Propietario, PropietarioDto>();
        CreateMap<PropietarioCrearDto, Propietario>();
        CreateMap<PropietarioActualizarDto, Propietario>();

        CreateMap<Paciente, PacienteDto>()
            .ForMember(d => d.PropietarioNombre, o => o.MapFrom(s => s.Propietario.Nombre));

        CreateMap<PacienteCrearDto, Paciente>();
        CreateMap<PacienteActualizarDto, Paciente>();
    }
}
