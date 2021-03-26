using Evaluacion.Aplicacion.Core.Base.Excepciones;
using System;
using System.Runtime.Serialization;

namespace Evaluacion.Aplicacion.Core.AdministracionPersonas.Personas.Proveedores.Excepciones
{
    [Serializable]
    internal class ProveedorRequestDtoNullException : EvaluacionException
    {
        public ProveedorRequestDtoNullException()
        {
        }

        public ProveedorRequestDtoNullException(string message) : base(message)
        {
        }

        public ProveedorRequestDtoNullException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ProveedorRequestDtoNullException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}