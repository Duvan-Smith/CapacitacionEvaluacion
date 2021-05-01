using Evaluacion.Aplicacion.Dto.Genericas.TipoDocumentos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Evaluacion.Infraestructura.Transversal.ClientesHttp.AdministracionPersonas.Genericas.TipoDocumentos
{
    public interface ITipoDocumentoClienteHttp
    {
        Task<IEnumerable<TipoDocumentoDto>> GetAll();
    }
}