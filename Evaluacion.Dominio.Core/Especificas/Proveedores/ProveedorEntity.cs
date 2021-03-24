using Evaluacion.Dominio.Core.Base;
using Evaluacion.Dominio.Core.Especificas.Personas;
using System.ComponentModel.DataAnnotations.Schema;

namespace Evaluacion.Dominio.Core.Especificas.Proveedores
{
    public class ProveedorEntity : EntidadBase
    {
        [ForeignKey("Id")]
        public PersonaEntity Persona { get; set; }
    }
}
