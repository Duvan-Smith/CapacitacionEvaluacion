using Evaluacion.Aplicacion.Dto.Especificas.Personas;
using Evaluacion.Aplicacion.Dto.Genericas.Areas;
using System.ComponentModel.DataAnnotations;

namespace Evaluacion.Aplicacion.Dto.Especificas.Empleados
{
    public class EmpleadoDto : PersonaDto
    {
        [Required]
        public double Salario { get; set; }
        public AreaDto AreaDto { get; set; }
    }
}
