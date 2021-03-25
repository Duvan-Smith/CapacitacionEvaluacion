using Evaluacion.Dominio.Core.Especificas.Personas;
using Evaluacion.Dominio.Core.Genericas.Areas;
using System;
using System.ComponentModel.DataAnnotations;

namespace Evaluacion.Dominio.Core.Especificas.Empleados
{
    public class EmpleadoEntity : PersonaBase
    {
        [Required]
        public double Salario { get; set; }
        public virtual AreaEntity Area { get; set; }
        public virtual Guid AreaId { get; set; }
        public override TipoPersona TipoPersona => TipoPersona.Natural;
    }
}
