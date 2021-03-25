using Evaluacion.Aplicacion.Core.Base.Excepciones;
using System;

namespace Evaluacion.Aplicacion.Core.AdministracionPersonas.Genericas.TipoDocumentos.Excepciones
{
    [Serializable]
    internal class TipoDocumentoRequestDtoNullException : EvaluacionException
    {
        public TipoDocumentoRequestDtoNullException()
        {
        }

        public TipoDocumentoRequestDtoNullException(string message) : base(message)
        {
        }

        public TipoDocumentoRequestDtoNullException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}