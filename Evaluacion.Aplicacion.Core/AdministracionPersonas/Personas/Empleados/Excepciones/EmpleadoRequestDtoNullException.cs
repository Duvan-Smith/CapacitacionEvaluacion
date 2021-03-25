using Evaluacion.Aplicacion.Core.Base.Excepciones;
using System;

namespace Evaluacion.Aplicacion.Core.AdministracionPersonas.Personas.Empleados.Excepciones
{
    [Serializable]
    internal class EmpleadoRequestDtoNullException : EvaluacionException
    {
        public EmpleadoRequestDtoNullException()
        {
        }

        public EmpleadoRequestDtoNullException(string message) : base(message)
        {
        }

        public EmpleadoRequestDtoNullException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}