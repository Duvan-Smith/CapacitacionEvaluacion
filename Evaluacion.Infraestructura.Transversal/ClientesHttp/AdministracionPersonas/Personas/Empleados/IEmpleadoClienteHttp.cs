using Evaluacion.Aplicacion.Dto.Especificas.Empleados;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Evaluacion.Infraestructura.Transversal.ClientesHttp.AdministracionPersonas.Personas.Empleados
{
    public interface IEmpleadoClienteHttp
    {
        Task<IEnumerable<EmpleadoDto>> GetAll();
        Task<EmpleadoDto> Post(EmpleadoRequestDto areaDto);
    }
}
