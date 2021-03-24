using Evaluacion.Dominio.Core.Base;
using Evaluacion.Dominio.Core.Especificas.Personas;
using Evaluacion.Dominio.Core.Genericas.Areas;
using System.ComponentModel.DataAnnotations.Schema;

namespace Evaluacion.Dominio.Core.Especificas.Empleados
{
    public class Empleado : EntidadBase
    {
        [ForeignKey("Id")]
        public Persona Persona { get; set; }
        [ForeignKey("Id")]
        public Area Area { get; set; }
    }
}
