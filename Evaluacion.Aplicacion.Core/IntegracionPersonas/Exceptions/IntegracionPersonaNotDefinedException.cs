using Evaluacion.Aplicacion.Core.Base.Excepciones;
using System;
using System.Runtime.Serialization;

namespace Evaluacion.Aplicacion.Core.IntegracionPersonas.Exceptions
{
    [Serializable]
    internal class IntegracionPersonaNotDefinedException : EvaluacionException
    {
        public IntegracionPersonaNotDefinedException()
        {
        }

        public IntegracionPersonaNotDefinedException(string message) : base(message)
        {
        }

        public IntegracionPersonaNotDefinedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected IntegracionPersonaNotDefinedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}