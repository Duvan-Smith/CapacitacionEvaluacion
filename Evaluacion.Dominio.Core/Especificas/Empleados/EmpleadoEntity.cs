using Evaluacion.Dominio.Core.Base;
using Evaluacion.Dominio.Core.Especificas.Personas;
using Evaluacion.Dominio.Core.Genericas.Areas;
using System.ComponentModel.DataAnnotations.Schema;

namespace Evaluacion.Dominio.Core.Especificas.Empleados
{
    public class EmpleadoEntity : EntidadBase
    {
        [ForeignKey("Id")]
        public PersonaEntity Persona { get; set; }
        [ForeignKey("Id")]
        public AreaEntity Area { get; set; }
    }
}
