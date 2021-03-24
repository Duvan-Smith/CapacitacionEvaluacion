using Evaluacion.Dominio.Core.Base;
using System;

namespace Evaluacion.Infraestructura.Datos.Persistencia.Core.Base
{
    public interface IContextDb : IUnitOfWork, IDisposable
    {
    }
}
