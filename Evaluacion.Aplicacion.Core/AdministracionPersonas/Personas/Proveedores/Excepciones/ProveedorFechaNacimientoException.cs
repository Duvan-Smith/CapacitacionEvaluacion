using Evaluacion.Aplicacion.Core.Base.Excepciones;
using System;
using System.Runtime.Serialization;

namespace Evaluacion.Aplicacion.Core.AdministracionPersonas.Personas.Proveedores.Excepciones
{
    [Serializable]
    internal class ProveedorFechaNacimientoException : EvaluacionException
    {
        private DateTimeOffset fechaNacimiento;

        public ProveedorFechaNacimientoException()
        {
        }

        public ProveedorFechaNacimientoException(DateTimeOffset fechaNacimiento)
        {
            this.fechaNacimiento = fechaNacimiento;
        }

        public ProveedorFechaNacimientoException(string message) : base(message)
        {
        }

        public ProveedorFechaNacimientoException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ProveedorFechaNacimientoException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}