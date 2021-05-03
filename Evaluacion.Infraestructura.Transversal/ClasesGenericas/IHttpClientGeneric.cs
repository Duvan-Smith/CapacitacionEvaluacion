using Evaluacion.Aplicacion.Dto.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Evaluacion.Infraestructura.Transversal.ClasesGenericas
{
    public interface IHttpClientGeneric<TRequest> where TRequest : DataTransferObject
    {
        Task<IEnumerable<TRequest>> Get(string action);

        Task<TResponse> Post<TResponse>(string path, TRequest request);

        Task<TRequest> Put(TRequest request);

        Task<TRequest> Patch(TRequest request);

        public Task<TRequest> Delete();
    }
}
