using Evaluacion.Aplicacion.Dto.Base;
using System.Threading.Tasks;

namespace Evaluacion.Aplicacion.Core.IntegracionPersonas
{
    public interface IIntegracionPersonaService
    {
        Task<string> ExportJson<TRequest>(string path, TRequest request) where TRequest : DataTransferObject;
        Task<TResponse> ImportJson<TResponse>(string path) where TResponse : DataTransferObject;
    }
}
