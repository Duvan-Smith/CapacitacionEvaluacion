using Evaluacion.Aplicacion.Dto.Base;
using System.Threading.Tasks;

namespace Evaluacion.Infraestructura.Transversal.MetodosGenericos
{
    public interface IHttpGenericClient
    {
        Task<T> GetAll<T>(string path) where T : DataTransferObject;
        Task<T> Get<T>(string path, string request) where T : DataTransferObject;
        Task<TResponse> Post<TResponse, TRequest>(string path, TRequest request) where TRequest : DataTransferObject;
        Task<TResponse> Put<TResponse, TRequest>(string path, TRequest request) where TRequest : DataTransferObject;
        Task<TResponse> Patch<TResponse, TRequest>(string path, TRequest request) where TRequest : DataTransferObject;
        Task<T> Delete<T>(string path) where T : DataTransferObject;
    }
}
