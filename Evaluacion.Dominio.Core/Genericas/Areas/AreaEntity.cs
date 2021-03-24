using Evaluacion.Dominio.Core.Base;
using Evaluacion.Dominio.Core.Especificas.Personas;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Evaluacion.Dominio.Core.Genericas.Areas
{
    public class AreaEntity : EntidadBase
    {
        [Required]
        [MaxLength(50)]
        public string NombreArea { get; set; }

        [ForeignKey("Id")]
        public PersonaEntity Persona { get; set; }
    }
}
