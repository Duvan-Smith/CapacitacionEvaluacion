using System;
using System.ComponentModel.DataAnnotations;

namespace Evaluacion.Dominio.Core.Base
{
    public abstract class EntidadBase
    {
        [Key]
        public Guid Id { get; set; }
    }
}
