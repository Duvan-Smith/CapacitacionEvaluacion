using Evaluacion.Dominio.Core.Especificas.Empleados;
using Evaluacion.Infraestructura.Datos.Persistencia.Core.Base;

namespace Evaluacion.Infraestructura.Datos.Persistencia.Core.Especificas.Empleados
{
    public class EmpleadoRepositorio : RepositorioBase<EmpleadoEntity>, IEmpleadoRepositorio
    {
        public EmpleadoRepositorio(IContextDb unitOfWork) : base(unitOfWork)
        {
        }
    }
}