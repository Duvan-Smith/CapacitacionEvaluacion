using Evaluacion.Aplicacion.Core.Base.Excepciones;
using Evaluacion.Aplicacion.Dto.Especificas.Personas;
using System;
using System.Runtime.Serialization;

namespace Evaluacion.Aplicacion.Core.AdministracionPersonas.Personas.Proveedores.Excepciones
{
    [Serializable]
    internal class ProveedorTipoPersonaNullException : EvaluacionException
    {
        private TipoPersona tipoPersona;

        public ProveedorTipoPersonaNullException()
        {
        }

        public ProveedorTipoPersonaNullException(TipoPersona tipoPersona)
        {
            this.tipoPersona = tipoPersona;
        }

        public ProveedorTipoPersonaNullException(string message) : base(message)
        {
        }

        public ProveedorTipoPersonaNullException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ProveedorTipoPersonaNullException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}