using Evaluacion.Aplicacion.Dto.Base;
using Evaluacion.Aplicacion.Dto.Especificas.Clientes;
using Evaluacion.Aplicacion.Dto.Especificas.Empleados;
using Evaluacion.Aplicacion.Dto.Especificas.Proveedores;
using Evaluacion.Aplicacion.Dto.Genericas.Areas;
using Evaluacion.Aplicacion.Dto.Genericas.TipoDocumentos;
using System;
using System.Collections.Generic;
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

        public IEnumerable<AreaDto> Area { get; set; }
        public EmpleadoDto EmpleadoDto { get; set; }
        public ProveedorDto ProveedorDto { get; set; }
        public TipoDocumentoDto TipoDocumentoDto { get; set; }
        public ClienteDto ClienteDto { get; set; }
    }
}
