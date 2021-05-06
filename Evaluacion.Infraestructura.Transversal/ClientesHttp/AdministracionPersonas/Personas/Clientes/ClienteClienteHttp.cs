using Evaluacion.Aplicacion.Dto.Especificas.Clientes;
using Evaluacion.Infraestructura.Transversal.ClasesGenericas;
using Evaluacion.Infraestructura.Transversal.MetodosGenericos.Cofiguration;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Evaluacion.Infraestructura.Transversal.ClientesHttp.AdministracionPersonas.Personas.Clientes
{
    public class ClienteClienteHttp : HttpClientGeneric<ClienteDto>, IClienteClienteHttp
    {
        public ClienteClienteHttp(HttpClient client, IOptions<HttpClientSettings> settings) : base(client, settings)
        {
        }

        protected override string Controller { get => "Cliente"; }

        public async Task<IEnumerable<ClienteDto>> GetAll() => await GetAll("GetAllCliente").ConfigureAwait(false);
        public async Task<ClienteDto> Post(ClienteRequestDto clienteDto) => await Post<ClienteRequestDto>("InsertCliente", clienteDto).ConfigureAwait(false);
    }
}
