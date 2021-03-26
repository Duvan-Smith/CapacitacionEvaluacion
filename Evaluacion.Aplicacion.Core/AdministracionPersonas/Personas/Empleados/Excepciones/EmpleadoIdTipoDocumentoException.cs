using Evaluacion.Aplicacion.Core.Base.Excepciones;
using System;
using System.Runtime.Serialization;

namespace Evaluacion.Aplicacion.Core.AdministracionPersonas.Personas.Empleados.Excepciones
{
    [Serializable]
    internal class EmpleadoIdTipoDocumentoException : EvaluacionException
    {
        public EmpleadoIdTipoDocumentoException()
        {
        }

        public EmpleadoIdTipoDocumentoException(string message) : base(message)
        {
        }

        public EmpleadoIdTipoDocumentoException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected EmpleadoIdTipoDocumentoException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}