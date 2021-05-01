using Evaluacion.Aplicacion.Core.AdministracionPersonas.Personas.FachadaPersonas;
using Evaluacion.Aplicacion.Dto.Especificas.Clientes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Evaluacion.WebApi.Controllers.Fachadas.FachadaGenericas
{
    [ApiController]
    [Route("[controller]")]
    public class ClienteController : ControllerBase
    {
        private readonly ILogger<ClienteController> _logger;
        private readonly IFachadaPersonasService _fachadaPersonasService;
        public ClienteController(ILogger<ClienteController> logger, IFachadaPersonasService fachadaGenericasService)
        {
            _logger = logger;
            _fachadaPersonasService = fachadaGenericasService;
        }
        #region ClienteController
        [HttpPost(nameof(InsertCliente))]
        public async Task<ClienteResponseDto> InsertCliente(ClienteRequestDto requestDto) =>
            await _fachadaPersonasService.ClienteManagementInsert(requestDto).ConfigureAwait(false);

        [HttpPut(nameof(UpdateCliente))]
        public async Task<ClienteResponseDto> UpdateCliente(ClienteRequestDto requestDto) =>
            await _fachadaPersonasService.ClienteManagementUpdate(requestDto).ConfigureAwait(false);

        [HttpDelete(nameof(DeleteCliente))]
        public async Task<ClienteResponseDto> DeleteCliente(ClienteRequestDto requestDto) =>
            await _fachadaPersonasService.ClienteManagementDelete(requestDto).ConfigureAwait(false);

        [HttpPost(nameof(GetCliente))]
        public async Task<ClienteDto> GetCliente(ClienteRequestDto requestDto) =>
            await _fachadaPersonasService.ClienteManagementGet(requestDto).ConfigureAwait(false);

        [HttpGet(nameof(GetAllCliente))]
        public async Task<IEnumerable<ClienteDto>> GetAllCliente() =>
            await _fachadaPersonasService.ClienteManagementGetAll().ConfigureAwait(false);

        [HttpGet(nameof(ExportAllCliente))]
        public async Task<string> ExportAllCliente() =>
            await _fachadaPersonasService.ClienteManagementExportAll().ConfigureAwait(false);

        [HttpGet(nameof(ImportAllCliente))]
        public async Task<IEnumerable<ClienteDto>> ImportAllCliente() =>
            await _fachadaPersonasService.ClienteManagementImportAll().ConfigureAwait(false);
        #endregion
    }
}