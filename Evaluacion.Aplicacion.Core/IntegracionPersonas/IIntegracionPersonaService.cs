using Evaluacion.Aplicacion.Dto.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Evaluacion.Aplicacion.Core.IntegracionPersonas
{
    public interface IIntegracionPersonaService
    {
        Task<string> ExportJson<TRequest>(string path, TRequest request) where TRequest : IEnumerable<EntidadPersonaBase>;
        Task<TResponse> ImportJson<TResponse>(string path) where TResponse : IEnumerable<EntidadPersonaBase>;
    }
}
