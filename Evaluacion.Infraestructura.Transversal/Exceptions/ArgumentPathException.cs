using System;
using System.Runtime.Serialization;

namespace Evaluacion.Infraestructura.Transversal.Exceptions
{
    [Serializable]
    internal class ArgumentPathException : Exception
    {
        public ArgumentPathException()
        {
        }

        public ArgumentPathException(string message) : base(message)
        {
        }

        public ArgumentPathException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ArgumentPathException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}