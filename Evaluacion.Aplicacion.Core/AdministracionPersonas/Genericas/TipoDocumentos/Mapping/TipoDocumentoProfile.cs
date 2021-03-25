using AutoMapper;
using Evaluacion.Aplicacion.Dto.Genericas.TipoDocumentos;
using Evaluacion.Dominio.Core.Genericas.TipoDocumentos;

namespace Evaluacion.Aplicacion.Core.AdministracionPersonas.Genericas.TipoDocumentos.Mapping
{
    public class TipoDocumentoProfile : Profile
    {
        public TipoDocumentoProfile()
        {
            CreateMap<TipoDocumentoEntity, TipoDocumentoDto>().ReverseMap();
            CreateMap<TipoDocumentoEntity, TipoDocumentoRequestDto>().ReverseMap();
            CreateMap<TipoDocumentoEntity, TipoDocumentoResponseDto>().ReverseMap();
        }
    }
}
