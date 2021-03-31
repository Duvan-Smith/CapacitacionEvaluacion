using Evaluacion.Aplicacion.Core.Base.Excepciones;
using System;
using System.Runtime.Serialization;

namespace Evaluacion.Aplicacion.Core.AdministracionPersonas.Personas.Empleados.Excepciones
{
    [Serializable]
    internal class EmpleadoNoExistException : EvaluacionException
    {
        public EmpleadoNoExistException()
        {
        }

        public EmpleadoNoExistException(string message) : base(message)
        {
        }

        public EmpleadoNoExistException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected EmpleadoNoExistException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}