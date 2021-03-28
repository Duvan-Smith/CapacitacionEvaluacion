using Evaluacion.Aplicacion.Core.Base.Excepciones;
using System;
using System.Runtime.Serialization;

namespace Evaluacion.Aplicacion.Core.AdministracionPersonas.Personas.Empleados.Excepciones
{
    [Serializable]
    internal class EmpleadoAreaIdNullException : EvaluacionException
    {
        private Guid areaId;

        public EmpleadoAreaIdNullException()
        {
        }

        public EmpleadoAreaIdNullException(Guid areaId)
        {
            this.areaId = areaId;
        }

        public EmpleadoAreaIdNullException(string message) : base(message)
        {
        }

        public EmpleadoAreaIdNullException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected EmpleadoAreaIdNullException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}