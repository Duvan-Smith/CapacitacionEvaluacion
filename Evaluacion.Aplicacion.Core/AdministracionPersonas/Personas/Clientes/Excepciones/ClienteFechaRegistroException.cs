using Evaluacion.Aplicacion.Core.Base.Excepciones;
using System;
using System.Runtime.Serialization;

namespace Evaluacion.Aplicacion.Core.AdministracionPersonas.Personas.Clientes.Excepciones
{
    [Serializable]
    internal class ClienteFechaRegistroException : EvaluacionException
    {
        private DateTimeOffset fechaRegistro;

        public ClienteFechaRegistroException()
        {
        }

        public ClienteFechaRegistroException(DateTimeOffset fechaRegistro)
        {
            this.fechaRegistro = fechaRegistro;
        }

        public ClienteFechaRegistroException(string message) : base(message)
        {
        }

        public ClienteFechaRegistroException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ClienteFechaRegistroException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}