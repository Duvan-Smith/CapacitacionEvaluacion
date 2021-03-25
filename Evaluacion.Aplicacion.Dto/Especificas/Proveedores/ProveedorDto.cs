using Evaluacion.Aplicacion.Dto.Base;
using Evaluacion.Aplicacion.Dto.Especificas.Personas;
using System.Collections.Generic;

namespace Evaluacion.Aplicacion.Dto.Especificas.Proveedores
{
    public class ProveedorDto : BaseEntity
    {
        public IEnumerable<PersonaDto> PersonaProveedor { get; set; }
    }
}
