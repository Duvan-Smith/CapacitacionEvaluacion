using Evaluacion.Aplicacion.Core.Base.Excepciones;
using System;
using System.Runtime.Serialization;

namespace Evaluacion.Aplicacion.Core.AdministracionPersonas.Personas.Proveedores.Excepciones
{
    [Serializable]
    internal class ProveedorNoExistException : EvaluacionException
    {
        public ProveedorNoExistException()
        {
        }

        public ProveedorNoExistException(string message) : base(message)
        {
        }

        public ProveedorNoExistException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ProveedorNoExistException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}