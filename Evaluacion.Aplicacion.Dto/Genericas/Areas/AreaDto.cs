using Evaluacion.Aplicacion.Dto.Base;
using System;
using System.ComponentModel.DataAnnotations;

namespace Evaluacion.Aplicacion.Dto.Genericas.Areas
{
    public class AreaDto : BaseEntity
    {
        [Required]
        [MaxLength(50)]
        public string NombreArea { get; set; }
        public Guid EmpleadoResponsableId { get; set; }
    }
}
