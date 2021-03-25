using Evaluacion.Aplicacion.Dto.Base;
using Evaluacion.Aplicacion.Dto.Especificas.Personas;
using Evaluacion.Aplicacion.Dto.Genericas.Areas;
using System.Collections.Generic;

namespace Evaluacion.Aplicacion.Dto.Especificas.Empleados
{
    public class EmpleadoDto : BaseEntity
    {
        public IEnumerable<PersonaDto> PersonaEmpleado { get; set; }
        public AreaDto AreaDto { get; set; }
    }
}
