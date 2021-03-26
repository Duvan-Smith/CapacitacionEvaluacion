using Evaluacion.Aplicacion.Core.Base.Excepciones;
using System;
using System.Runtime.Serialization;

namespace Evaluacion.Aplicacion.Core.AdministracionPersonas.Personas.Clientes.Excepciones
{
    [Serializable]
    internal class ClientenameAlreadyExistException : EvaluacionException
    {
        public ClientenameAlreadyExistException()
        {
        }

        public ClientenameAlreadyExistException(string message) : base(message)
        {
        }

        public ClientenameAlreadyExistException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ClientenameAlreadyExistException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}