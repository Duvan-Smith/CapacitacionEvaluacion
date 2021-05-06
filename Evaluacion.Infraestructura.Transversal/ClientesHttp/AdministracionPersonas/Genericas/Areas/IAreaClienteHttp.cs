using Evaluacion.Aplicacion.Dto.Genericas.Areas;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Evaluacion.Infraestructura.Transversal.ClientesHttp.AdministracionPersonas.Genericas.Areas
{
    public interface IAreaClienteHttp
    {
        Task<IEnumerable<AreaDto>> GetAll();
        Task<AreaDto> Post(AreaRequestDto areaDto);
        Task<AreaDto> Put(AreaRequestDto areaDto);
        Task<AreaDto> GetId(AreaRequestDto areaDto);
    }
}