using Evaluacion.Aplicacion.Core.Base.Excepciones;
using System;
using System.Runtime.Serialization;

namespace Evaluacion.Aplicacion.Core.AdministracionPersonas.Genericas.TipoDocumentos.Excepciones
{
    [Serializable]
    internal class TipoDocumentoNoExistException : EvaluacionException
    {
        public TipoDocumentoNoExistException()
        {
        }

        public TipoDocumentoNoExistException(string message) : base(message)
        {
        }

        public TipoDocumentoNoExistException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected TipoDocumentoNoExistException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}