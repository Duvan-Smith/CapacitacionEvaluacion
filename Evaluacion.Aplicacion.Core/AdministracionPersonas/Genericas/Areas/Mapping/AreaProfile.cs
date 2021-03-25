using AutoMapper;
using Evaluacion.Aplicacion.Dto.Genericas.Areas;
using Evaluacion.Dominio.Core.Genericas.Areas;

namespace Evaluacion.Aplicacion.Core.AdministracionPersonas.Genericas.Areas.Mapping
{
    public class AreaProfile : Profile
    {
        public AreaProfile()
        {
            CreateMap<AreaEntity, AreaDto>().ReverseMap();
            CreateMap<AreaEntity, AreaRequestDto>().ReverseMap();
            CreateMap<AreaEntity, AreaResponseDto>().ReverseMap();
        }
    }
}
