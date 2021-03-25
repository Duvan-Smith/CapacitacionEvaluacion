using AutoMapper;
using Evaluacion.Aplicacion.Dto.Especificas.Proveedores;
using Evaluacion.Dominio.Core.Especificas.Proveedores;

namespace Evaluacion.Aplicacion.Core.AdministracionPersonas.Personas.Proveedores.Mapping
{
    public class ProveedorProfile : Profile
    {
        public ProveedorProfile()
        {
            CreateMap<ProveedorEntity, ProveedorDto>().ReverseMap();
            CreateMap<ProveedorEntity, ProveedorRequestDto>().ReverseMap();
            CreateMap<ProveedorEntity, ProveedorResponseDto>().ReverseMap();
        }
    }
}
