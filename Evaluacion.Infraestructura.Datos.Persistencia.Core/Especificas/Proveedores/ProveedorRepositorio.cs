using Evaluacion.Dominio.Core.Especificas.Proveedores;
using Evaluacion.Infraestructura.Datos.Persistencia.Core.Base;

namespace Evaluacion.Infraestructura.Datos.Persistencia.Core.Especificas.Proveedores
{
    public class ProveedorRepositorio : RepositorioBase<ProveedorEntity>, IProveedorRepositorio
    {
        public ProveedorRepositorio(IContextDb unitOfWork) : base(unitOfWork)
        {
        }
    }
}
