using Evaluacion.Dominio.Core.Base;
using Evaluacion.Dominio.Core.Especificas.Personas;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Evaluacion.Dominio.Core.Genericas.TipoDocumentos
{
    public class TipoDocumentoEntity : EntidadBase
    {
        [Required]
        [MaxLength(50)]
        public string NombreTipoDocumento { get; set; }

        [Required]
        [MinLength(8)]
        [MaxLength(50)]
        public string CodigoTipoDocumento { get; set; }
        public IEnumerable<PersonaEntity> Persona { get; set; }

    }
}
