using Evaluacion.Aplicacion.Core.Base.Excepciones;
using System;

namespace Evaluacion.Aplicacion.Core.AdministracionPersonas.Personas.Empleados.Excepciones
{
    [Serializable]
    internal class EmpleadonameAlreadyExistException : EvaluacionException
    {
        public EmpleadonameAlreadyExistException()
        {
        }

        public EmpleadonameAlreadyExistException(string message) : base(message)
        {
        }

        public EmpleadonameAlreadyExistException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}