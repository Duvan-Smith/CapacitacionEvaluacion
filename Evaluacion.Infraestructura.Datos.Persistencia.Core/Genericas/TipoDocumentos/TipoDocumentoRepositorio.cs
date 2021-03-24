using Evaluacion.Dominio.Core.Genericas.TipoDocumentos;
using Evaluacion.Infraestructura.Datos.Persistencia.Core.Base;

namespace Evaluacion.Infraestructura.Datos.Persistencia.Core.Genericas.TipoDocumentos
{
    public class TipoDocumentoRepositorio : RepositorioBase<TipoDocumentoEntity>, ITipoDocumentoRepositorio
    {
        public TipoDocumentoRepositorio(IContextDb unitOfWork) : base(unitOfWork)
        {
        }
    }
}