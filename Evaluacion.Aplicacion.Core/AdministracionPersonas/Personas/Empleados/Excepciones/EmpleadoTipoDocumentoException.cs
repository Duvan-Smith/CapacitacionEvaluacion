using Evaluacion.Aplicacion.Core.Base.Excepciones;
using System;
using System.Runtime.Serialization;

namespace Evaluacion.Aplicacion.Core.AdministracionPersonas.Personas.Empleados.Excepciones
{
    [Serializable]
    internal class EmpleadoTipoDocumentoException : EvaluacionException
    {
        public EmpleadoTipoDocumentoException()
        {
        }

        public EmpleadoTipoDocumentoException(string message) : base(message)
        {
        }

        public EmpleadoTipoDocumentoException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected EmpleadoTipoDocumentoException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}