using Evaluacion.Dominio.Core.Base;
using Evaluacion.Dominio.Core.Especificas.Personas;
using System.Collections.Generic;

namespace Evaluacion.Dominio.Core.Especificas.Proveedores
{
    public class ProveedorEntity : EntidadBase
    {
        public IEnumerable<PersonaEntity> Persona { get; set; }
    }
}
