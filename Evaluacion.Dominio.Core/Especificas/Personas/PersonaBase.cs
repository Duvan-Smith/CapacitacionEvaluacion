using Evaluacion.Dominio.Core.Base;
using Evaluacion.Dominio.Core.Genericas.TipoDocumentos;
using System;
using System.ComponentModel.DataAnnotations;

namespace Evaluacion.Dominio.Core.Especificas.Personas
{
    public enum TipoPersona
    {
        Natural = 1,
        Juridico = 2,
    }
    public abstract class PersonaBase : EntidadBase
    {
        public virtual TipoPersona TipoPersona { get; set; }
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
        public virtual TipoDocumentoEntity TipoDocumento { get; set; }
        public virtual Guid TipoDocumentoId { get; set; }

        [Required]
        [MinLength(8)]
        [MaxLength(50)]
        public string CodigoTipoDocumento { get; set; }
    }
}
