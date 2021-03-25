using Evaluacion.Dominio.Core.Base;
using Evaluacion.Dominio.Core.Especificas.Clientes;
using Evaluacion.Dominio.Core.Especificas.Empleados;
using Evaluacion.Dominio.Core.Especificas.Proveedores;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Evaluacion.Dominio.Core.Genericas.TipoDocumentos
{
    public class TipoDocumentoEntity : EntidadBase
    {
        [Required]
        [MaxLength(50)]
        public string NombreTipoDocumento { get; set; }
        public virtual IEnumerable<ClienteEntity> Cliente { get; set; }
        public virtual IEnumerable<EmpleadoEntity> Empleado { get; set; }
        public virtual IEnumerable<ProveedorEntity> Proveedor { get; set; }
    }
}
