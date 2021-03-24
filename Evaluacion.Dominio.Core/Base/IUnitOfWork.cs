using Microsoft.EntityFrameworkCore;

namespace Evaluacion.Dominio.Core.Base
{
    public interface IUnitOfWork
    {
        public int Commit();
        public void Rollback();
        public DbSet<T> Set<T>() where T : EntidadBase;
        public void SetAttach<T>(T item) where T : EntidadBase;
        public void SetModified<T>(T item) where T : EntidadBase;
        public void SetDeatached<T>(T item) where T : EntidadBase;
    }
}
