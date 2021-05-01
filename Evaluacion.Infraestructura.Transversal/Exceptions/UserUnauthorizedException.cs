using Evaluacion.Aplicacion.Core.Base.Excepciones;
using System;
using System.Runtime.Serialization;

namespace Evaluacion.Infraestructura.Transversal.Exceptions
{
    [Serializable]
    internal class UserUnauthorizedException : EvaluacionException
    {
        public UserUnauthorizedException()
        {
        }

        public UserUnauthorizedException(string message) : base(message)
        {
        }

        public UserUnauthorizedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected UserUnauthorizedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}