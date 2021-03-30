using Evaluacion.Aplicacion.Dto.Base;
using System.Threading.Tasks;

namespace Evaluacion.Aplicacion.Core.IntegracionPersonas
{
    public interface IIntegracionPersonaService
    {
        Task<string> Export<TRequest>(string path, TRequest request) where TRequest : DataTransferObject;
        Task<TResponse> Import<TResponse, TRequest>(string path, TRequest request) where TResponse : DataTransferObject;
    }
}
