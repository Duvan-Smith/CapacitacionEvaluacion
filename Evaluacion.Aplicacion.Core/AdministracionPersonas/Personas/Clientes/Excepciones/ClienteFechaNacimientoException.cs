using Evaluacion.Aplicacion.Core.Base.Excepciones;
using System;
using System.Runtime.Serialization;

namespace Evaluacion.Aplicacion.Core.AdministracionPersonas.Personas.Clientes.Excepciones
{
    [Serializable]
    internal class ClienteFechaNacimientoException : EvaluacionException
    {
        private DateTimeOffset fechaNacimiento;

        public ClienteFechaNacimientoException()
        {
        }

        public ClienteFechaNacimientoException(DateTimeOffset fechaNacimiento)
        {
            this.fechaNacimiento = fechaNacimiento;
        }

        public ClienteFechaNacimientoException(string message) : base(message)
        {
        }

        public ClienteFechaNacimientoException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ClienteFechaNacimientoException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}