using Evaluacion.Aplicacion.Core.Base.Excepciones;
using System;
using System.Runtime.Serialization;

namespace Evaluacion.Aplicacion.Core.AdministracionPersonas.Personas.Empleados.Excepciones
{
    [Serializable]
    internal class EmpleadoidAlreadyExistException : EvaluacionException
    {
        public EmpleadoidAlreadyExistException()
        {
        }

        public EmpleadoidAlreadyExistException(string message) : base(message)
        {
        }

        public EmpleadoidAlreadyExistException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected EmpleadoidAlreadyExistException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}