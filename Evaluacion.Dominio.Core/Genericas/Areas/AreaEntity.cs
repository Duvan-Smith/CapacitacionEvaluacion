using Evaluacion.Dominio.Core.Base;
using Evaluacion.Dominio.Core.Especificas.Empleados;
using Evaluacion.Dominio.Core.Especificas.Personas;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Evaluacion.Dominio.Core.Genericas.Areas
{
    public class AreaEntity : EntidadBase
    {
        [Required]
        [MaxLength(50)]
        public string NombreArea { get; set; }
        public IEnumerable<EmpleadoEntity> Empleado { get; set; }
        public PersonaEntity PersonaEntity { get; set; }
    }
}
