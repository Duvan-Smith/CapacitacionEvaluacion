using AutoMapper;
using Evaluacion.Aplicacion.Dto.Especificas.Clientes;
using Evaluacion.Dominio.Core.Especificas.Clientes;

namespace Evaluacion.Aplicacion.Core.AdministracionPersonas.Personas.Clientes.Mapping
{
    public class ClienteProfile : Profile
    {
        public ClienteProfile()
        {
            CreateMap<ClienteEntity, ClienteDto>().ReverseMap();
            CreateMap<ClienteEntity, ClienteRequestDto>().ReverseMap();
            CreateMap<ClienteEntity, ClienteResponseDto>().ReverseMap();
        }
    }
}
