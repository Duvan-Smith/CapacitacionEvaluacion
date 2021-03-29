using Evaluacion.Aplicacion.Core.Base.Excepciones;
using Evaluacion.Aplicacion.Dto.Especificas.Personas;
using System;
using System.Runtime.Serialization;

namespace Evaluacion.Aplicacion.Core.AdministracionPersonas.Personas.Clientes.Excepciones
{
    [Serializable]
    internal class ClienteTipoPersonaNullException : EvaluacionException
    {
        private TipoPersona tipoPersona;

        public ClienteTipoPersonaNullException()
        {
        }

        public ClienteTipoPersonaNullException(TipoPersona tipoPersona)
        {
            this.tipoPersona = tipoPersona;
        }

        public ClienteTipoPersonaNullException(string message) : base(message)
        {
        }

        public ClienteTipoPersonaNullException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ClienteTipoPersonaNullException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}