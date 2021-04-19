using Evaluacion.Aplicacion.Core.AdministracionPersonas.Personas.FachadaPersonas;
using Evaluacion.Aplicacion.Dto.Especificas.Empleados;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Evaluacion.WebApi.Controllers.Fachadas.FachadaGenericas
{
    [ApiController]
    [Route("[controller]")]
    public class FachadaEmpleadoController : ControllerBase
    {
        private readonly ILogger<FachadaEmpleadoController> _logger;
        private readonly IFachadaPersonasService _fachadaPersonasService;
        public FachadaEmpleadoController(ILogger<FachadaEmpleadoController> logger, IFachadaPersonasService fachadaGenericasService)
        {
            _logger = logger;
            _fachadaPersonasService = fachadaGenericasService;
        }
        #region EmpleadoController
        [HttpPost(nameof(InsertEmpleado))]
        public async Task<EmpleadoResponseDto> InsertEmpleado(EmpleadoRequestDto requestDto) =>
            await _fachadaPersonasService.EmpleadoManagementInsert(requestDto).ConfigureAwait(false);

        [HttpPut(nameof(UpdateEmpleado))]
        public async Task<EmpleadoResponseDto> UpdateEmpleado(EmpleadoRequestDto requestDto) =>
            await _fachadaPersonasService.EmpleadoManagementUpdate(requestDto).ConfigureAwait(false);

        [HttpDelete(nameof(DeleteEmpleado))]
        public async Task<EmpleadoResponseDto> DeleteEmpleado(EmpleadoRequestDto requestDto) =>
            await _fachadaPersonasService.EmpleadoManagementDelete(requestDto).ConfigureAwait(false);

        [HttpPost(nameof(GetEmpleado))]
        public async Task<EmpleadoDto> GetEmpleado(EmpleadoRequestDto requestDto) =>
            await _fachadaPersonasService.EmpleadoManagementGet(requestDto).ConfigureAwait(false);

        [HttpGet(nameof(GetAllEmpleado))]
        public async Task<IEnumerable<EmpleadoDto>> GetAllEmpleado() =>
            await _fachadaPersonasService.EmpleadoManagementGetAll().ConfigureAwait(false);

        [HttpGet(nameof(ExportAllEmpleado))]
        public async Task<string> ExportAllEmpleado() =>
            await _fachadaPersonasService.EmpleadoManagementExportAll().ConfigureAwait(false);

        [HttpGet(nameof(ImportAllEmpleado))]
        public async Task<IEnumerable<EmpleadoDto>> ImportAllEmpleado() =>
            await _fachadaPersonasService.EmpleadoManagementImportAll().ConfigureAwait(false);
        #endregion
    }
}