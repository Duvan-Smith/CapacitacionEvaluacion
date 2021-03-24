using System;
using System.Runtime.Serialization;

namespace Evaluacion.Infraestructura.Transversal.Exceptions
{
    [Serializable]
    //TODO: Cambiar Exception por Exception base del proyecto
    internal class HttpClientNotDefinedException : Exception
    {
        public HttpClientNotDefinedException()
        {
        }

        public HttpClientNotDefinedException(string message) : base(message)
        {
        }

        public HttpClientNotDefinedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected HttpClientNotDefinedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}