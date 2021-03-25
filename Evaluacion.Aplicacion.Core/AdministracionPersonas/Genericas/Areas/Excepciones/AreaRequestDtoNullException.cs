using Evaluacion.Aplicacion.Core.Base.Excepciones;
using System;

namespace Evaluacion.Aplicacion.Core.AdministracionPersonas.Genericas.Areas.Excepciones
{
    [Serializable]
    internal class AreaRequestDtoNullException : EvaluacionException
    {
        public AreaRequestDtoNullException()
        {
        }

        public AreaRequestDtoNullException(string message) : base(message)
        {
        }

        public AreaRequestDtoNullException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}