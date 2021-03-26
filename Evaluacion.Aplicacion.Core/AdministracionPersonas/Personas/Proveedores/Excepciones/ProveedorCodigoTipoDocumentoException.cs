using Evaluacion.Aplicacion.Core.Base.Excepciones;
using System;
using System.Runtime.Serialization;

namespace Evaluacion.Aplicacion.Core.AdministracionPersonas.Personas.Proveedores.Excepciones
{
    [Serializable]
    internal class ProveedorCodigoTipoDocumentoException : EvaluacionException
    {
        private object p;

        public ProveedorCodigoTipoDocumentoException()
        {
        }

        public ProveedorCodigoTipoDocumentoException(object p)
        {
            this.p = p;
        }

        public ProveedorCodigoTipoDocumentoException(string message) : base(message)
        {
        }

        public ProveedorCodigoTipoDocumentoException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ProveedorCodigoTipoDocumentoException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}