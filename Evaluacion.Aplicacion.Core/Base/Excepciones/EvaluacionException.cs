using System;

namespace Evaluacion.Aplicacion.Core.Base.Excepciones
{
    [Serializable]
    public class EvaluacionException : Exception
    {
        public EvaluacionException()
        {
        }
        public EvaluacionException(string message) : base(message)
        {
        }
        public EvaluacionException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
