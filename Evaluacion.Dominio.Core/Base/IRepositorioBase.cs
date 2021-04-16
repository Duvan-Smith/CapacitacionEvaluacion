using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Evaluacion.Dominio.Core.Base
{
#pragma warning disable CS0693 // El parámetro de tipo tiene el mismo nombre que el parámetro de tipo de un tipo externo
    public interface IRepositorioBase<T> where T : EntidadBase
    {
        public IUnitOfWork UnitOfWork { get; }
        public Task<T> Insert<T>(T entidad) where T : EntidadBase;
        public Task<bool> Update<T>(T entidad) where T : EntidadBase;
        public Task<bool> Delete<T>(T id) where T : EntidadBase;
        public IEnumerable<T> SearchMatching<T>(Expression<Func<T, bool>> predicate) where T : EntidadBase;
        public T SearchMatchingOneResult<T>(Expression<Func<T, bool>> predicate) where T : EntidadBase;
        public IEnumerable<T> GetAll<T>() where T : EntidadBase;
    }
#pragma warning restore CS0693 // El parámetro de tipo tiene el mismo nombre que el parámetro de tipo de un tipo externo
}
