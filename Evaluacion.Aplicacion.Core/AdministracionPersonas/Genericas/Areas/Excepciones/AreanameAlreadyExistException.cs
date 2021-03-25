using Evaluacion.Aplicacion.Core.Base.Excepciones;
using System;

namespace Evaluacion.Aplicacion.Core.AdministracionPersonas.Genericas.Areas.Excepciones
{
    [Serializable]
    internal class AreanameAlreadyExistException : EvaluacionException
    {
        public AreanameAlreadyExistException()
        {
        }

        public AreanameAlreadyExistException(string message) : base(message)
        {
        }

        public AreanameAlreadyExistException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}