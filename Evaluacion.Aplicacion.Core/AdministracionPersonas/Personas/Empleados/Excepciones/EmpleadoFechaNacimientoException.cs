using Evaluacion.Aplicacion.Core.Base.Excepciones;
using System;
using System.Runtime.Serialization;

namespace Evaluacion.Aplicacion.Core.AdministracionPersonas.Personas.Empleados.Excepciones
{
    [Serializable]
    internal class EmpleadoFechaNacimientoException : EvaluacionException
    {
        private DateTimeOffset fechaNacimiento;

        public EmpleadoFechaNacimientoException()
        {
        }

        public EmpleadoFechaNacimientoException(DateTimeOffset fechaNacimiento)
        {
            this.fechaNacimiento = fechaNacimiento;
        }

        public EmpleadoFechaNacimientoException(string message) : base(message)
        {
        }

        public EmpleadoFechaNacimientoException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected EmpleadoFechaNacimientoException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}