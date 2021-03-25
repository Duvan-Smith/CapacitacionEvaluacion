using Evaluacion.Aplicacion.Dto.Base;
using System.ComponentModel.DataAnnotations;

namespace Evaluacion.Aplicacion.Dto.Genericas.TipoDocumentos
{
    public class TipoDocumentoDto : BaseEntity
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
