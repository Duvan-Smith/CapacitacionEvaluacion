using Evaluacion.Aplicacion.Dto.Base;
using Evaluacion.Aplicacion.Dto.Especificas.Clientes;
using Evaluacion.Aplicacion.Dto.Especificas.Empleados;
using Evaluacion.Aplicacion.Dto.Especificas.Proveedores;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Evaluacion.Aplicacion.Dto.Genericas.TipoDocumentos
{
    public class TipoDocumentoDto : BaseEntity
    {
        [Required]
        [MaxLength(50)]
        public string NombreTipoDocumento { get; set; }
        public IEnumerable<ClienteDto> Cliente { get; set; }
        public IEnumerable<EmpleadoDto> Empleado { get; set; }
        public IEnumerable<ProveedorDto> Proveedor { get; set; }
    }
}
