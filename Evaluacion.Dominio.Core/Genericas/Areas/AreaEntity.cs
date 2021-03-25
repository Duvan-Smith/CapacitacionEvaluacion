using Evaluacion.Dominio.Core.Base;
using Evaluacion.Dominio.Core.Especificas.Empleados;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Evaluacion.Dominio.Core.Genericas.Areas
{
    public class AreaEntity : EntidadBase
    {
        [Required]
        [MaxLength(50)]
        public string NombreArea { get; set; }
        public virtual IEnumerable<EmpleadoEntity> Empleado { get; set; }
        public Guid EmpleadoResponsableId { get; set; }
    }
}
