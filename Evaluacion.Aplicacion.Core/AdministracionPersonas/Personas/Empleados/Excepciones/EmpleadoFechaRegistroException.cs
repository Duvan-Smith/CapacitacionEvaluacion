using Evaluacion.Aplicacion.Core.Base.Excepciones;
using System;
using System.Runtime.Serialization;

namespace Evaluacion.Aplicacion.Core.AdministracionPersonas.Personas.Empleados.Excepciones
{
    [Serializable]
    internal class EmpleadoFechaRegistroException : EvaluacionException
    {
#pragma warning disable IDE0052 // Quitar miembros privados no leídos
        private readonly DateTimeOffset fechaRegistro;
#pragma warning restore IDE0052 // Quitar miembros privados no leídos
        public EmpleadoFechaRegistroException()
        {
        }

        public EmpleadoFechaRegistroException(DateTimeOffset fechaRegistro)
        {
            this.fechaRegistro = fechaRegistro;
        }

        public EmpleadoFechaRegistroException(string message) : base(message)
        {
        }

        public EmpleadoFechaRegistroException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected EmpleadoFechaRegistroException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}