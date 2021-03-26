using Evaluacion.Aplicacion.Core.Base.Excepciones;
using System;
using System.Runtime.Serialization;

namespace Evaluacion.Aplicacion.Core.AdministracionPersonas.Personas.Empleados.Excepciones
{
    [Serializable]
    internal class EmpleadoCodigoTipoDocumentoException : EvaluacionException
    {
        public EmpleadoCodigoTipoDocumentoException()
        {
        }

        public EmpleadoCodigoTipoDocumentoException(string message) : base(message)
        {
        }

        public EmpleadoCodigoTipoDocumentoException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected EmpleadoCodigoTipoDocumentoException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}