using Evaluacion.Aplicacion.Dto.Especificas.Proveedores;
using Evaluacion.Infraestructura.Transversal.ClasesGenericas;
using Evaluacion.Infraestructura.Transversal.MetodosGenericos.Cofiguration;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Evaluacion.Infraestructura.Transversal.ClientesHttp.AdministracionPersonas.Personas.Proveedores
{
    public class ProveedorClienteHttp : HttpClientGeneric<ProveedorDto>, IProveedorClienteHttp
    {
        public ProveedorClienteHttp(HttpClient client, IOptions<HttpClientSettings> settings) : base(client, settings)
        {
        }

        protected override string Controller { get => "Proveedor"; }

        public async Task<IEnumerable<ProveedorDto>> GetAll() => await GetAll("GetAllProveedor").ConfigureAwait(false);
        public async Task<ProveedorDto> Post(ProveedorRequestDto proveedorDto) => await Post<ProveedorRequestDto>("InsertProveedor", proveedorDto).ConfigureAwait(false);
    }
}
