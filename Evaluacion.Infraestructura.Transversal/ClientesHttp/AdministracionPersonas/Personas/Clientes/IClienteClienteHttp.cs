using Evaluacion.Aplicacion.Dto.Especificas.Clientes;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Evaluacion.Infraestructura.Transversal.ClientesHttp.AdministracionPersonas.Personas.Clientes
{
    public interface IClienteClienteHttp
    {
        Task<IEnumerable<ClienteDto>> GetAll();
        Task<ClienteDto> Post(ClienteRequestDto areaDto);
    }
}
