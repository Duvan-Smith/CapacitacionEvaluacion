using Evaluacion.Dominio.Core.Genericas.Areas;
using Evaluacion.Infraestructura.Datos.Persistencia.Core.Base;

namespace Evaluacion.Infraestructura.Datos.Persistencia.Core.Genericas.Areas
{
    public class AreaRepositorio : RepositorioBase<AreaEntity>, IAreaRepositorio
    {
        public AreaRepositorio(IContextDb unitOfWork) : base(unitOfWork)
        {
        }
    }
}
