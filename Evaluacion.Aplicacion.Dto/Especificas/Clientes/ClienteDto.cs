using Evaluacion.Aplicacion.Dto.Base;
using Evaluacion.Aplicacion.Dto.Especificas.Personas;
using System.Collections.Generic;

namespace Evaluacion.Aplicacion.Dto.Especificas.Clientes
{
    public class ClienteDto : BaseEntity
    {
        public IEnumerable<PersonaDto> PersonaCliente { get; set; }
    }
}
