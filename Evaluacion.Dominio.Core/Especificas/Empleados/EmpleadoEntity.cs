using Evaluacion.Dominio.Core.Base;
using Evaluacion.Dominio.Core.Especificas.Personas;
using Evaluacion.Dominio.Core.Genericas.Areas;
using System.Collections.Generic;

namespace Evaluacion.Dominio.Core.Especificas.Empleados
{
    public class EmpleadoEntity : EntidadBase
    {
        public IEnumerable<PersonaEntity> PersonaEmpleado { get; set; }
        public AreaEntity AreaEntity { get; set; }
    }
}
