using Evaluacion.Aplicacion.Dto.Genericas.TipoDocumentos;
using Evaluacion.Infraestructura.Transversal.ClasesGenericas;
using Evaluacion.Infraestructura.Transversal.MetodosGenericos.Cofiguration;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Evaluacion.Infraestructura.Transversal.ClientesHttp.AdministracionPersonas.Genericas.TipoDocumentos
{
    public class TipoDocumentoClienteHttp : HttpClientGeneric<TipoDocumentoDto>, ITipoDocumentoClienteHttp
    {
        public TipoDocumentoClienteHttp(HttpClient client, IOptions<HttpClientSettings> settings) : base(client, settings)
        {
        }

        protected override string Controller { get => "TipoDocumento"; }

        public async Task<IEnumerable<TipoDocumentoDto>> GetAll() => await GetAll("GetAllTipoDocumento").ConfigureAwait(false);
        public async Task<TipoDocumentoDto> Post(TipoDocumentoRequestDto tipoDocumentoDto) => await Post<TipoDocumentoRequestDto>("InsertTipoDocumento", tipoDocumentoDto).ConfigureAwait(false);
    }
}
