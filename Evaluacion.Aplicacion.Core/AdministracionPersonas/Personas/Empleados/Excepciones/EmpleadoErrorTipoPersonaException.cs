using Evaluacion.Aplicacion.Core.Base.Excepciones;
using System;
using System.Runtime.Serialization;

namespace Evaluacion.Aplicacion.Core.AdministracionPersonas.Personas.Empleados.Excepciones
{
    [Serializable]
    internal class EmpleadoErrorTipoPersonaException : EvaluacionException
    {
        public EmpleadoErrorTipoPersonaException()
        {
        }

        public EmpleadoErrorTipoPersonaException(string message) : base(message)
        {
        }

        public EmpleadoErrorTipoPersonaException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected EmpleadoErrorTipoPersonaException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}