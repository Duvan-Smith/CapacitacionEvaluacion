using System;
using System.Net;

namespace Evaluacion.Aplicacion.Dto.Base
{
    [Serializable]
    public class DataTransferObject
    {
        public bool Aceptado { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public string StatusDescription { get; set; }
    }
}