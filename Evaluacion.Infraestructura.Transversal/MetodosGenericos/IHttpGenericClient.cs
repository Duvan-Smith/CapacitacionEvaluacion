using System.Threading.Tasks;

namespace Evaluacion.Infraestructura.Transversal.MetodosGenericos
{
    public interface IHttpGenericClient
    {
        //TODO: crear Dto generico y cambiar el class por ese DTo generico "DataTransferObject"
        Task<T> GetAll<T>(string path) where T : class;
        Task<T> Get<T>(string path, string request) where T : class;
        Task<TResponse> Post<TResponse, TRequest>(string path, TRequest request) where TResponse : class;
        Task<T> Put<T>(string path, T request) where T : class;
        Task<T> Patch<T>(string path, T request) where T : class;
    }
}
