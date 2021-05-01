using Evaluacion.Aplicacion.Core.AdministracionPersonas.Genericas.FachadaGenericas;
using Evaluacion.Aplicacion.Dto.Genericas.Areas;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Evaluacion.WebApi.Controllers.Fachadas.FachadaGenericas
{
    [ApiController]
    [Route("[controller]")]
    public class AreaController : ControllerBase
    {
        private readonly ILogger<AreaController> _logger;
        private readonly IFachadaGenericasService _fachadaGenericasService;
        public AreaController(ILogger<AreaController> logger, IFachadaGenericasService fachadaGenericasService)
        {
            _logger = logger;
            _fachadaGenericasService = fachadaGenericasService;
        }
        #region AreaController
        [HttpPost(nameof(InsertArea))]
        public async Task<AreaResponseDto> InsertArea(AreaRequestDto requestDto) =>
            await _fachadaGenericasService.AreaManagementInsert(requestDto).ConfigureAwait(false);

        [HttpPut(nameof(UpdateArea))]
        public async Task<AreaResponseDto> UpdateArea(AreaRequestDto requestDto) =>
            await _fachadaGenericasService.AreaManagementUpdate(requestDto).ConfigureAwait(false);

        [HttpDelete(nameof(DeleteArea))]
        public async Task<AreaResponseDto> DeleteArea(AreaRequestDto requestDto) =>
            await _fachadaGenericasService.AreaManagementDelete(requestDto).ConfigureAwait(false);

        [HttpPost(nameof(GetArea))]
        public async Task<AreaDto> GetArea(AreaRequestDto requestDto) =>
            await _fachadaGenericasService.AreaManagementGet(requestDto).ConfigureAwait(false);

        [HttpGet(nameof(GetAllArea))]
        public async Task<IEnumerable<AreaDto>> GetAllArea() =>
            await _fachadaGenericasService.AreaManagementGetAll().ConfigureAwait(false);
        #endregion
    }
}