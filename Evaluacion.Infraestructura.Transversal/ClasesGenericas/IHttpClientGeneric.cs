using Evaluacion.Aplicacion.Dto.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Evaluacion.Infraestructura.Transversal.ClasesGenericas
{
    public interface IHttpClientGeneric<T> where T : DataTransferObject
    {
        Task<IEnumerable<T>> Get(string action);

        Task<T> Post(T request);

        Task<T> Put(T request);

        Task<T> Patch(T request);

        public Task<T> Delete();
    }
}
