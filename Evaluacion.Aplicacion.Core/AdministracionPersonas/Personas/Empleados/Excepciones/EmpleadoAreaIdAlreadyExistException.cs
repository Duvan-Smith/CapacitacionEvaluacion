using Evaluacion.Aplicacion.Core.Base.Excepciones;
using System;
using System.Runtime.Serialization;

namespace Evaluacion.Aplicacion.Core.AdministracionPersonas.Personas.Empleados.Excepciones
{
    [Serializable]
    internal class EmpleadoAreaIdAlreadyExistException : EvaluacionException
    {
        private Guid areaId;

        public EmpleadoAreaIdAlreadyExistException()
        {
        }

        public EmpleadoAreaIdAlreadyExistException(Guid areaId)
        {
            this.areaId = areaId;
        }

        public EmpleadoAreaIdAlreadyExistException(string message) : base(message)
        {
        }

        public EmpleadoAreaIdAlreadyExistException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected EmpleadoAreaIdAlreadyExistException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}