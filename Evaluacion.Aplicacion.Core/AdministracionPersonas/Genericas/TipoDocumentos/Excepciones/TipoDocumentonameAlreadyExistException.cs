using Evaluacion.Aplicacion.Core.Base.Excepciones;
using System;

namespace Evaluacion.Aplicacion.Core.AdministracionPersonas.Genericas.TipoDocumentos.Excepciones
{
    [Serializable]
    internal class TipoDocumentonameAlreadyExistException : EvaluacionException
    {
        public TipoDocumentonameAlreadyExistException()
        {
        }

        public TipoDocumentonameAlreadyExistException(string message) : base(message)
        {
        }

        public TipoDocumentonameAlreadyExistException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}