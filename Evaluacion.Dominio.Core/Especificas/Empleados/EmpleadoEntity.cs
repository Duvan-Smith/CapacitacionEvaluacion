using Evaluacion.Dominio.Core.Base;
using Evaluacion.Dominio.Core.Especificas.Personas;
using System;
using System.Collections.Generic;

namespace Evaluacion.Dominio.Core.Especificas.Empleados
{
    public class EmpleadoEntity : EntidadBase
    {
        public IEnumerable<PersonaEntity> Persona { get; set; }
        public Guid AreaEntityId { get; set; }
    }
}
