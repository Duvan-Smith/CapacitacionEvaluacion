using Evaluacion.Aplicacion.Core.Base.Excepciones;
using System;
using System.Runtime.Serialization;

namespace Evaluacion.Aplicacion.Core.AdministracionPersonas.Genericas.Areas.Excepciones
{
    [Serializable]
    internal class AreaEmpleadoResponsableIdNullException : EvaluacionException
    {
        public AreaEmpleadoResponsableIdNullException()
        {
        }

        public AreaEmpleadoResponsableIdNullException(string message) : base(message)
        {
        }

        public AreaEmpleadoResponsableIdNullException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected AreaEmpleadoResponsableIdNullException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}