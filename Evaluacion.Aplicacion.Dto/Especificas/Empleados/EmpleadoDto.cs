using Evaluacion.Aplicacion.Dto.Especificas.Personas;
using Evaluacion.Aplicacion.Dto.Genericas.Areas;
using System;
using System.ComponentModel.DataAnnotations;

namespace Evaluacion.Aplicacion.Dto.Especificas.Empleados
{
    public class EmpleadoDto : PersonaDto
    {
        [Required]
        public double Salario { get; set; }
        public AreaDto Area { get; set; }
        public Guid AreaId { get; set; }
        public override TipoPersona TipoPersona => TipoPersona.Natural;
    }
}
