using Evaluacion.Aplicacion.Core.Base.Excepciones;
using System;
using System.Runtime.Serialization;

namespace Evaluacion.Aplicacion.Core.AdministracionPersonas.Personas.Proveedores.Excepciones
{
    [Serializable]
    internal class ProveedorAlreadyExistException : EvaluacionException
    {
        public ProveedorAlreadyExistException()
        {
        }

        public ProveedorAlreadyExistException(string message) : base(message)
        {
        }

        public ProveedorAlreadyExistException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ProveedorAlreadyExistException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}