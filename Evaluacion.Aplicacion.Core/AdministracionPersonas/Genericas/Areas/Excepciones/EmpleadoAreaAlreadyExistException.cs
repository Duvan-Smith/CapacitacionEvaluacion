using Evaluacion.Aplicacion.Core.Base.Excepciones;
using System;
using System.Runtime.Serialization;

namespace Evaluacion.Aplicacion.Core.AdministracionPersonas.Genericas.Areas.Excepciones
{
    [Serializable]
    internal class EmpleadoAreaAlreadyExistException : EvaluacionException
    {
        public EmpleadoAreaAlreadyExistException()
        {
        }

        public EmpleadoAreaAlreadyExistException(string message) : base(message)
        {
        }

        public EmpleadoAreaAlreadyExistException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected EmpleadoAreaAlreadyExistException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}