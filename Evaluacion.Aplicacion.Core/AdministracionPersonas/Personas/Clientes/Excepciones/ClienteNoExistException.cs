using Evaluacion.Aplicacion.Core.Base.Excepciones;
using System;
using System.Runtime.Serialization;

namespace Evaluacion.Aplicacion.Core.AdministracionPersonas.Personas.Clientes.Excepciones
{
    [Serializable]
    internal class ClienteNoExistException : EvaluacionException
    {
        public ClienteNoExistException()
        {
        }

        public ClienteNoExistException(string message) : base(message)
        {
        }

        public ClienteNoExistException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ClienteNoExistException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}