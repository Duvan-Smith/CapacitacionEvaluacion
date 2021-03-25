using AutoMapper;
using Evaluacion.Aplicacion.Dto.Especificas.Empleados;
using Evaluacion.Dominio.Core.Especificas.Empleados;

namespace Evaluacion.Aplicacion.Core.AdministracionPersonas.Personas.Empleados.Mapping
{
    public class EmpleadoProfile : Profile
    {
        public EmpleadoProfile()
        {
            CreateMap<EmpleadoEntity, EmpleadoDto>().ReverseMap();
            CreateMap<EmpleadoEntity, EmpleadoRequestDto>().ReverseMap();
            CreateMap<EmpleadoEntity, EmpleadoResponseDto>().ReverseMap();
        }
    }
}
