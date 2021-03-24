using Evaluacion.Dominio.Core.Base;
using System.ComponentModel.DataAnnotations;

namespace Evaluacion.Dominio.Core.Genericas.TipoDocumentos
{
    public class TipoDocumento : EntidadBase
    {
        [Required]
        [MaxLength(50)]
        public string NombreTipoDocumento { get; set; }

        [Required]
        [MinLength(8)]
        [MaxLength(50)]
        public string CodigoTipoDocumento { get; set; }
    }
}
