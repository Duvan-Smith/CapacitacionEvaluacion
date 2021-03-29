using Evaluacion.Aplicacion.Core.Base.Excepciones;
using System;
using System.Runtime.Serialization;

namespace Evaluacion.Aplicacion.Core.AdministracionPersonas.Personas.Empleados.Excepciones
{
    [Serializable]
    internal class EmpleadoTipoPersonaNullException : EvaluacionException
    {
        private Dto.Especificas.Personas.TipoPersona tipoPersona;

        public EmpleadoTipoPersonaNullException()
        {
        }

        public EmpleadoTipoPersonaNullException(Dto.Especificas.Personas.TipoPersona tipoPersona)
        {
            this.tipoPersona = tipoPersona;
        }

        public EmpleadoTipoPersonaNullException(string message) : base(message)
        {
        }

        public EmpleadoTipoPersonaNullException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected EmpleadoTipoPersonaNullException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}