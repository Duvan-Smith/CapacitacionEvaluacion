using Evaluacion.Aplicacion.Core.Base.Excepciones;
using System;
using System.Runtime.Serialization;

namespace Evaluacion.Infraestructura.Transversal.Exceptions
{
    [Serializable]
    internal class HttpClientNotDefinedException : EvaluacionException
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