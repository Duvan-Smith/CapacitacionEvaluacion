using Evaluacion.Aplicacion.Dto.Especificas.Proveedores;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Evaluacion.Infraestructura.Transversal.ClientesHttp.AdministracionPersonas.Personas.Proveedores
{
    public interface IProveedorClienteHttp
    {
        Task<IEnumerable<ProveedorDto>> GetAll();
    }
}
