using Evaluacion.Aplicacion.Dto.Genericas.Areas;
using Evaluacion.Infraestructura.Transversal.ClasesGenericas;
using Evaluacion.Infraestructura.Transversal.MetodosGenericos.Cofiguration;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Evaluacion.Infraestructura.Transversal.ClientesHttp.AdministracionPersonas.Genericas.Areas
{
    public class AreaClienteHttp : HttpClientGeneric<AreaDto>, IAreaClienteHttp
    {
        public AreaClienteHttp(HttpClient client, IOptions<HttpClientSettings> settings) : base(client, settings)
        {
        }

        protected override string Controller { get => "Area"; }

        public async Task<IEnumerable<AreaDto>> GetAll() => await Get("GetAllArea").ConfigureAwait(false);
    }
}
