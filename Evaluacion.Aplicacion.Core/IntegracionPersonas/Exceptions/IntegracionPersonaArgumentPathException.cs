using Evaluacion.Aplicacion.Core.Base.Excepciones;
using System;
using System.Runtime.Serialization;

namespace Evaluacion.Aplicacion.Core.IntegracionPersonas.Exceptions
{
    [Serializable]
    internal class IntegracionPersonaArgumentPathException : EvaluacionException
    {
        public IntegracionPersonaArgumentPathException()
        {
        }

        public IntegracionPersonaArgumentPathException(string message) : base(message)
        {
        }

        public IntegracionPersonaArgumentPathException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected IntegracionPersonaArgumentPathException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}