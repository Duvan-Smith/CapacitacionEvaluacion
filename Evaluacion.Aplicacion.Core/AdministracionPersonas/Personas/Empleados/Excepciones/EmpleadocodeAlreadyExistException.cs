using Evaluacion.Aplicacion.Core.Base.Excepciones;
using System;
using System.Runtime.Serialization;

namespace Evaluacion.Aplicacion.Core.AdministracionPersonas.Personas.Empleados.Excepciones
{
    [Serializable]
    internal class EmpleadocodeAlreadyExistException : EvaluacionException
    {
        private double codigoEmpleado;

        public EmpleadocodeAlreadyExistException()
        {
        }

        public EmpleadocodeAlreadyExistException(double codigoEmpleado)
        {
            this.codigoEmpleado = codigoEmpleado;
        }

        public EmpleadocodeAlreadyExistException(string message) : base(message)
        {
        }

        public EmpleadocodeAlreadyExistException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected EmpleadocodeAlreadyExistException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}