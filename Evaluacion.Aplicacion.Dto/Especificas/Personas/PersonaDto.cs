using Evaluacion.Aplicacion.Dto.Base;
using Evaluacion.Aplicacion.Dto.Genericas.TipoDocumentos;
using System;
using System.ComponentModel.DataAnnotations;

namespace Evaluacion.Aplicacion.Dto.Especificas.Personas
{
    public class PersonaDto : BaseEntity
    {
        [Required]
        [MaxLength(50)]
        public string Nombre { get; set; }

        [Required]
        [MaxLength(50)]
        public string Apellido { get; set; }

        [Required]
        public DateTimeOffset FechaNacimiento { get; set; }

        [Required]
        public DateTimeOffset FechaRegistro { get; set; }

        [Required]
        public int NumeroTelefono { get; set; }

        [Required]
        [RegularExpression(@"^[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?$")]
        public string CorreoElectronico { get; set; }
        public TipoDocumentoDto TipoDocumento { get; set; }
        public Guid TipoDocumentoId { get; set; }

        [Required]
        [MinLength(8)]
        [MaxLength(50)]
        public string CodigoTipoDocumento { get; set; }
    }
}
