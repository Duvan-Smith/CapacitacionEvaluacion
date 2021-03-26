using Evaluacion.Aplicacion.Core.Base.Excepciones;
using System;
using System.Runtime.Serialization;

namespace Evaluacion.Aplicacion.Core.AdministracionPersonas.Personas.Clientes.Excepciones
{
    [Serializable]
    internal class ClienteCodigoTipoDocumentoException : EvaluacionException
    {
        public ClienteCodigoTipoDocumentoException()
        {
        }

        public ClienteCodigoTipoDocumentoException(string message) : base(message)
        {
        }

        public ClienteCodigoTipoDocumentoException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ClienteCodigoTipoDocumentoException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}