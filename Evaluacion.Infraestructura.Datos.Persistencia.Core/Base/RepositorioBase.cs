using Evaluacion.Dominio.Core.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Evaluacion.Infraestructura.Datos.Persistencia.Core.Base
{
    public abstract class RepositorioBase<TGeneric> : IRepositorioBase<TGeneric> where TGeneric : EntidadBase
    {
        public IUnitOfWork UnitOfWork { get; }
        public RepositorioBase(IUnitOfWork unitOfWork) => UnitOfWork = unitOfWork;

        public Task<bool> Delete<T>(T entity) where T : EntidadBase
        {
            return Task.Run(() =>
                {
                    try
                    {
                        var entityToDelete = UnitOfWork.Set<T>().First(x => x == entity);
                        UnitOfWork.Set<T>().Remove(entityToDelete);
                        UnitOfWork.Commit();
                        return true;
                    }
                    catch
                    {
                        return false;
                    }
                }
            );
        }
        public IEnumerable<T> GetAll<T>() where T : EntidadBase => UnitOfWork.Set<T>().ToArray();
        public async Task<T> Insert<T>(T entidad) where T : EntidadBase
        {
            var response = await UnitOfWork.Set<T>().AddAsync(entidad).ConfigureAwait(false);
            UnitOfWork.Commit();
            return response.Entity;
        }
        public IEnumerable<T> SearchMatching<T>(Expression<Func<T, bool>> predicate) where T : EntidadBase =>
            UnitOfWork.Set<T>().Where(predicate);
        public T SearchMatchingOneResult<T>(Expression<Func<T, bool>> predicate) where T : EntidadBase =>
            UnitOfWork.Set<T>().First(predicate);
        public Task<bool> Update<T>(T entidad) where T : EntidadBase
        {
            return Task.Run(() =>
                {
                    try
                    {
                        var response = UnitOfWork.Set<T>().Update(entidad);
                        UnitOfWork.Commit();
                        return true;
                    }
                    catch
                    {
                        return false;
                    }
                }
            );
        }
    }
}
