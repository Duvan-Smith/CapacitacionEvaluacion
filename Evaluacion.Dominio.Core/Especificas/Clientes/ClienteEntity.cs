using Evaluacion.Dominio.Core.Base;
using Evaluacion.Dominio.Core.Especificas.Personas;
using System.Collections.Generic;

namespace Evaluacion.Dominio.Core.Especificas.Clientes
{
    public class ClienteEntity : EntidadBase
    {
        public IEnumerable<PersonaEntity> PersonaCliente { get; set; }
    }
}
