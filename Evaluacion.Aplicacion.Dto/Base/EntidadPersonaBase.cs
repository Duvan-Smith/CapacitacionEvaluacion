using System;
using System.ComponentModel.DataAnnotations;

namespace Evaluacion.Aplicacion.Dto.Base
{
    public class EntidadPersonaBase : DataTransferObject
    {
        [Key]
        public Guid Id { get; set; }
    }
}