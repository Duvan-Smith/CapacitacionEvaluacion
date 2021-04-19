using Evaluacion.Aplicacion.Core.AdministracionPersonas.Personas.FachadaPersonas;
using Evaluacion.Aplicacion.Dto.Especificas.Proveedores;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Evaluacion.WebApi.Controllers.Fachadas.FachadaGenericas
{
    [ApiController]
    [Route("[controller]")]
    public class FachadaProveedorController : ControllerBase
    {
        private readonly ILogger<FachadaProveedorController> _logger;
        private readonly IFachadaPersonasService _fachadaPersonasService;
        public FachadaProveedorController(ILogger<FachadaProveedorController> logger, IFachadaPersonasService fachadaGenericasService)
        {
            _logger = logger;
            _fachadaPersonasService = fachadaGenericasService;
        }
        #region ProveedorController
        [HttpPost(nameof(InsertProveedor))]
        public async Task<ProveedorResponseDto> InsertProveedor(ProveedorRequestDto requestDto) =>
            await _fachadaPersonasService.ProveedorManagementInsert(requestDto).ConfigureAwait(false);

        [HttpPut(nameof(UpdateProveedor))]
        public async Task<ProveedorResponseDto> UpdateProveedor(ProveedorRequestDto requestDto) =>
            await _fachadaPersonasService.ProveedorManagementUpdate(requestDto).ConfigureAwait(false);

        [HttpDelete(nameof(DeleteProveedor))]
        public async Task<ProveedorResponseDto> DeleteProveedor(ProveedorRequestDto requestDto) =>
            await _fachadaPersonasService.ProveedorManagementDelete(requestDto).ConfigureAwait(false);

        [HttpPost(nameof(GetProveedor))]
        public async Task<ProveedorDto> GetProveedor(ProveedorRequestDto requestDto) =>
            await _fachadaPersonasService.ProveedorManagementGet(requestDto).ConfigureAwait(false);

        [HttpGet(nameof(GetAllProveedor))]
        public async Task<IEnumerable<ProveedorDto>> GetAllProveedor() =>
            await _fachadaPersonasService.ProveedorManagementGetAll().ConfigureAwait(false);

        [HttpGet(nameof(ExportAllProveedor))]
        public async Task<string> ExportAllProveedor() =>
            await _fachadaPersonasService.ProveedorManagementExportAll().ConfigureAwait(false);

        [HttpGet(nameof(ImportAllProveedor))]
        public async Task<IEnumerable<ProveedorDto>> ImportAllProveedor() =>
            await _fachadaPersonasService.ProveedorManagementImportAll().ConfigureAwait(false);
        #endregion
    }
}