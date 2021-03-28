using Evaluacion.Aplicacion.Core.Base.Excepciones;
using System;
using System.Runtime.Serialization;

namespace Evaluacion.Aplicacion.Core.AdministracionPersonas.Genericas.Areas.Excepciones
{
    [Serializable]
    internal class AreaNoExistException : EvaluacionException
    {
        public AreaNoExistException()
        {
        }

        public AreaNoExistException(string message) : base(message)
        {
        }

        public AreaNoExistException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected AreaNoExistException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}