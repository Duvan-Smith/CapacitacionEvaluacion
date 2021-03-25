using Evaluacion.Aplicacion.Dto.Base;
using Evaluacion.Aplicacion.Dto.Genericas.Areas;
using System.ComponentModel.DataAnnotations;

namespace Evaluacion.Aplicacion.Dto.Especificas.Empleados
{
    public class EmpleadoDto : BaseEntity
    {
        [Required]
        public double Salario { get; set; }
        public AreaDto AreaDto { get; set; }
    }
}
