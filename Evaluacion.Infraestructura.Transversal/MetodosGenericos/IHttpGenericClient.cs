using Evaluacion.Aplicacion.Dto.Base;
using System.Threading.Tasks;

namespace Evaluacion.Infraestructura.Transversal.MetodosGenericos
{
    public interface IHttpGenericClient
    {
        Task<T> GetAll<T>(string path) where T : DataTransferObject;
        Task<T> Get<T>(string path, string request) where T : DataTransferObject;
        Task<TResponse> Post<TResponse, TRequest>(string path, TRequest request) where TResponse : DataTransferObject;
        Task<T> Put<T>(string path, T request) where T : DataTransferObject;
        Task<T> Patch<T>(string path, T request) where T : DataTransferObject;
    }
}
