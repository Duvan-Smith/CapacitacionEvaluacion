using System;
using System.ComponentModel.DataAnnotations;

namespace Evaluacion.Aplicacion.Dto.Base
{
    public class BaseEntity : DataTransferObject
    {
        [Key]
        public Guid Id { get; set; }
    }
}