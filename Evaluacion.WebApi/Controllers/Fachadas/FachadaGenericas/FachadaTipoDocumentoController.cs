using Evaluacion.Aplicacion.Core.AdministracionPersonas.Genericas.FachadaGenericas;
using Evaluacion.Aplicacion.Dto.Genericas.TipoDocumentos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Evaluacion.WebApi.Controllers.Fachadas.FachadaGenericas
{
    [ApiController]
    [Route("[controller]")]
    public class FachadaTipoDocumentoController : ControllerBase
    {
        private readonly ILogger<FachadaTipoDocumentoController> _logger;
        private readonly IFachadaGenericasService _fachadaGenericasService;
        public FachadaTipoDocumentoController(ILogger<FachadaTipoDocumentoController> logger, IFachadaGenericasService fachadaGenericasService)
        {
            _logger = logger;
            _fachadaGenericasService = fachadaGenericasService;
        }
        #region TipoDocumentoController
        [HttpPost(nameof(InsertTipoDocumento))]
        public async Task<TipoDocumentoResponseDto> InsertTipoDocumento(TipoDocumentoRequestDto requestDto) =>
            await _fachadaGenericasService.TipoDocumentoManagementInsert(requestDto).ConfigureAwait(false);

        [HttpPut(nameof(UpdateTipoDocumento))]
        public async Task<TipoDocumentoResponseDto> UpdateTipoDocumento(TipoDocumentoRequestDto requestDto) =>
            await _fachadaGenericasService.TipoDocumentoManagementUpdate(requestDto).ConfigureAwait(false);

        [HttpDelete(nameof(DeleteTipoDocumento))]
        public async Task<TipoDocumentoResponseDto> DeleteTipoDocumento(TipoDocumentoRequestDto requestDto) =>
            await _fachadaGenericasService.TipoDocumentoManagementDelete(requestDto).ConfigureAwait(false);

        [HttpPost(nameof(GetTipoDocumento))]
        public async Task<TipoDocumentoDto> GetTipoDocumento(TipoDocumentoRequestDto requestDto) =>
            await _fachadaGenericasService.TipoDocumentoManagementGet(requestDto).ConfigureAwait(false);

        [HttpGet(nameof(GetAllTipoDocumento))]
        public async Task<IEnumerable<TipoDocumentoDto>> GetAllTipoDocumento() =>
            await _fachadaGenericasService.TipoDocumentoManagementGetAll().ConfigureAwait(false);
        #endregion
    }
}