using Evaluacion.Aplicacion.Core.Base.Excepciones;
using System;
using System.Runtime.Serialization;

namespace Evaluacion.Aplicacion.Core.AdministracionPersonas.Personas.Proveedores.Excepciones
{
    [Serializable]
    internal class ProveedornameAlreadyExistException : EvaluacionException
    {
        public ProveedornameAlreadyExistException()
        {
        }

        public ProveedornameAlreadyExistException(string message) : base(message)
        {
        }

        public ProveedornameAlreadyExistException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ProveedornameAlreadyExistException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}