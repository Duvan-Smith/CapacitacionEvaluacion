using Evaluacion.Aplicacion.Dto.Base;
using Evaluacion.Aplicacion.Dto.Especificas.Empleados;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Evaluacion.Aplicacion.Dto.Genericas.Areas
{
    public class AreaDto : EntidadPersonaBase
    {
        [Required]
        [MaxLength(50)]
        public string NombreArea { get; set; }
        public IEnumerable<EmpleadoDto> Empleado { get; set; }
        public Guid EmpleadoResponsableId { get; set; }
    }
}
