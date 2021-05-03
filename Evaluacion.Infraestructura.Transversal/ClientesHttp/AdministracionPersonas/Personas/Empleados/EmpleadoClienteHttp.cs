using Evaluacion.Aplicacion.Dto.Especificas.Empleados;
using Evaluacion.Infraestructura.Transversal.ClasesGenericas;
using Evaluacion.Infraestructura.Transversal.MetodosGenericos.Cofiguration;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Evaluacion.Infraestructura.Transversal.ClientesHttp.AdministracionPersonas.Personas.Empleados
{
    public class EmpleadoClienteHttp : HttpClientGeneric<EmpleadoDto>, IEmpleadoClienteHttp
    {
        public EmpleadoClienteHttp(HttpClient client, IOptions<HttpClientSettings> settings) : base(client, settings)
        {
        }

        protected override string Controller { get => "Empleado"; }

        public async Task<IEnumerable<EmpleadoDto>> GetAll() => await Get("GetAllEmpleado").ConfigureAwait(false);
        public async Task<EmpleadoDto> Post(EmpleadoRequestDto empleadoDto) => await Post<EmpleadoRequestDto>("InsertEmpleado", empleadoDto).ConfigureAwait(false);
    }
}
