using Evaluacion.Aplicacion.Core.Base.Excepciones;
using System;
using System.Runtime.Serialization;

namespace Evaluacion.Aplicacion.Core.AdministracionPersonas.Personas.Clientes.Excepciones
{
    [Serializable]
    internal class ClienteTipoDocumentoException : EvaluacionException
    {
        public ClienteTipoDocumentoException()
        {
        }

        public ClienteTipoDocumentoException(string message) : base(message)
        {
        }

        public ClienteTipoDocumentoException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ClienteTipoDocumentoException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}