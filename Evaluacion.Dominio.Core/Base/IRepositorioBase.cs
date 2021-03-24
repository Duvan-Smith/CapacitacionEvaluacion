using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Evaluacion.Dominio.Core.Base
{
    public interface IRepositorioBase<T> where T : EntidadBase
    {
        public IUnitOfWork UnitOfWork { get; }
#pragma warning disable CS0693 // El parámetro de tipo tiene el mismo nombre que el parámetro de tipo de un tipo externo
        public Task<T> Insert<T>(T entidad) where T : EntidadBase;
#pragma warning restore CS0693 // El parámetro de tipo tiene el mismo nombre que el parámetro de tipo de un tipo externo
#pragma warning disable CS0693 // El parámetro de tipo tiene el mismo nombre que el parámetro de tipo de un tipo externo
        public bool Update<T>(T entidad) where T : EntidadBase;
#pragma warning restore CS0693 // El parámetro de tipo tiene el mismo nombre que el parámetro de tipo de un tipo externo
#pragma warning disable CS0693 // El parámetro de tipo tiene el mismo nombre que el parámetro de tipo de un tipo externo
        public bool Delete<T>(T id) where T : EntidadBase;
#pragma warning restore CS0693 // El parámetro de tipo tiene el mismo nombre que el parámetro de tipo de un tipo externo
#pragma warning disable CS0693 // El parámetro de tipo tiene el mismo nombre que el parámetro de tipo de un tipo externo
        public IEnumerable<T> SearchMatching<T>(Expression<Func<T, bool>> predicate) where T : EntidadBase;
#pragma warning restore CS0693 // El parámetro de tipo tiene el mismo nombre que el parámetro de tipo de un tipo externo
#pragma warning disable CS0693 // El parámetro de tipo tiene el mismo nombre que el parámetro de tipo de un tipo externo
        public T SearchMatchingOneResult<T>(Expression<Func<T, bool>> predicate) where T : EntidadBase;
#pragma warning restore CS0693 // El parámetro de tipo tiene el mismo nombre que el parámetro de tipo de un tipo externo
#pragma warning disable CS0693 // El parámetro de tipo tiene el mismo nombre que el parámetro de tipo de un tipo externo
        public IEnumerable<T> GetAll<T>() where T : EntidadBase;
#pragma warning restore CS0693 // El parámetro de tipo tiene el mismo nombre que el parámetro de tipo de un tipo externo
    }
}
