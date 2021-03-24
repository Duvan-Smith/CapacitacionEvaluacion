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
        private readonly IUnitOfWork _unitOfWork;
#pragma warning disable RCS1085 // Use auto-implemented property.
        public IUnitOfWork UnitOfWork => _unitOfWork;
#pragma warning restore RCS1085 // Use auto-implemented property.
#pragma warning disable RCS1160 // Abstract type should not have public constructors.
        public RepositorioBase(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;
#pragma warning restore RCS1160 // Abstract type should not have public constructors.

        public bool Delete<T>(T entity) where T : EntidadBase
        {
            try
            {
                var entityToDelete = _unitOfWork.Set<T>().First(x => x == entity);
                _unitOfWork.Set<T>().Remove(entityToDelete);
                _unitOfWork.Commit();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public IEnumerable<T> GetAll<T>() where T : EntidadBase => _unitOfWork.Set<T>().ToArray();
        public async Task<T> Insert<T>(T entidad) where T : EntidadBase
        {
            var response = await _unitOfWork.Set<T>().AddAsync(entidad).ConfigureAwait(false);
            _unitOfWork.Commit();
            return response.Entity;
        }
        public IEnumerable<T> SearchMatching<T>(Expression<Func<T, bool>> predicate) where T : EntidadBase =>
            _unitOfWork.Set<T>().Where(predicate);
        public T SearchMatchingOneResult<T>(Expression<Func<T, bool>> predicate) where T : EntidadBase =>
            _unitOfWork.Set<T>().First(predicate);
        public bool Update<T>(T entidad) where T : EntidadBase
        {
            try
            {
                var response = _unitOfWork.Set<T>().Update(entidad);
                _unitOfWork.Commit();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
