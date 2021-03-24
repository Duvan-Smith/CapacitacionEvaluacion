using Evaluacion.Dominio.Core.Base;
using Evaluacion.Dominio.Core.Genericas.Areas;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Evaluacion.Dominio.Core.Especificas.Personas
{
    public class PersonaEntity : EntidadBase
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

        public IEnumerable<AreaEntity> Area { get; set; }
        public Guid EmpleadoEntityId { get; set; }
        public Guid ProveedorEntityId { get; set; }
        public Guid TipoDocumentoId { get; set; }
    }
}
