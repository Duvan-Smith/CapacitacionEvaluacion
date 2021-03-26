using Evaluacion.Aplicacion.Core.Base.Excepciones;
using System;
using System.Runtime.Serialization;

namespace Evaluacion.Aplicacion.Core.AdministracionPersonas.Personas.Clientes.Excepciones
{
    [Serializable]
    internal class ClienteRequestDtoNullException : EvaluacionException
    {
        public ClienteRequestDtoNullException()
        {
        }

        public ClienteRequestDtoNullException(string message) : base(message)
        {
        }

        public ClienteRequestDtoNullException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ClienteRequestDtoNullException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}