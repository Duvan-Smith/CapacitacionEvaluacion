using Evaluacion.Aplicacion.Core.Base.Excepciones;
using System;
using System.Runtime.Serialization;

namespace Evaluacion.Aplicacion.Core.AdministracionPersonas.Personas.Proveedores.Excepciones
{
    [Serializable]
    internal class ProveedorFechaRegistroException : EvaluacionException
    {
        private DateTimeOffset fechaRegistro;

        public ProveedorFechaRegistroException()
        {
        }

        public ProveedorFechaRegistroException(DateTimeOffset fechaRegistro)
        {
            this.fechaRegistro = fechaRegistro;
        }

        public ProveedorFechaRegistroException(string message) : base(message)
        {
        }

        public ProveedorFechaRegistroException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ProveedorFechaRegistroException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}