using System;
using System.Runtime.Serialization;

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
        protected EvaluacionException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
