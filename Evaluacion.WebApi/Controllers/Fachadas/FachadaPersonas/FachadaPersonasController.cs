using Evaluacion.Aplicacion.Core.AdministracionPersonas.Personas.FachadaPersonas;
using Evaluacion.Aplicacion.Dto.Especificas.Clientes;
using Evaluacion.Aplicacion.Dto.Especificas.Empleados;
using Evaluacion.Aplicacion.Dto.Especificas.Proveedores;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Evaluacion.WebApi.Controllers.Fachadas.FachadaGenericas
{
    [ApiController]
    [Route("[controller]")]
    public class FachadaPersonasController : ControllerBase
    {
        private readonly ILogger<FachadaPersonasController> _logger;
        private readonly IFachadaPersonasService _fachadaPersonasService;
        public FachadaPersonasController(ILogger<FachadaPersonasController> logger, IFachadaPersonasService fachadaGenericasService)
        {
            _logger = logger;
            _fachadaPersonasService = fachadaGenericasService;
        }
        #region ClienteController
        [HttpPost(nameof(InsertCliente))]
        public async Task<ClienteResponseDto> InsertCliente(ClienteRequestDto requestDto) =>
            await _fachadaPersonasService.ClienteManagementInsert(requestDto).ConfigureAwait(false);

        [HttpPost(nameof(UpdateCliente))]
        public async Task<ClienteResponseDto> UpdateCliente(ClienteRequestDto requestDto) =>
            await _fachadaPersonasService.ClienteManagementUpdate(requestDto).ConfigureAwait(false);

        [HttpPost(nameof(DeleteCliente))]
        public async Task<ClienteResponseDto> DeleteCliente(ClienteRequestDto requestDto) =>
            await _fachadaPersonasService.ClienteManagementDelete(requestDto).ConfigureAwait(false);

        [HttpPost(nameof(GetCliente))]
        public async Task<ClienteDto> GetCliente(ClienteRequestDto requestDto) =>
            await _fachadaPersonasService.ClienteManagementGet(requestDto).ConfigureAwait(false);

        [HttpGet(nameof(GetAllCliente))]
        public async Task<IEnumerable<ClienteDto>> GetAllCliente() =>
            await _fachadaPersonasService.ClienteManagementGetAll().ConfigureAwait(false);
        #endregion
        #region EmpleadoController
        [HttpPost(nameof(InsertEmpleado))]
        public async Task<EmpleadoResponseDto> InsertEmpleado(EmpleadoRequestDto requestDto) =>
            await _fachadaPersonasService.EmpleadoManagementInsert(requestDto).ConfigureAwait(false);

        [HttpPost(nameof(UpdateEmpleado))]
        public async Task<EmpleadoResponseDto> UpdateEmpleado(EmpleadoRequestDto requestDto) =>
            await _fachadaPersonasService.EmpleadoManagementUpdate(requestDto).ConfigureAwait(false);

        [HttpPost(nameof(DeleteEmpleado))]
        public async Task<EmpleadoResponseDto> DeleteEmpleado(EmpleadoRequestDto requestDto) =>
            await _fachadaPersonasService.EmpleadoManagementDelete(requestDto).ConfigureAwait(false);

        [HttpPost(nameof(GetEmpleado))]
        public async Task<EmpleadoDto> GetEmpleado(EmpleadoRequestDto requestDto) =>
            await _fachadaPersonasService.EmpleadoManagementGet(requestDto).ConfigureAwait(false);

        [HttpGet(nameof(GetAllEmpleado))]
        public async Task<IEnumerable<EmpleadoDto>> GetAllEmpleado() =>
            await _fachadaPersonasService.EmpleadoManagementGetAll().ConfigureAwait(false);
        #endregion
        #region ProveedorController
        [HttpPost(nameof(InsertProveedor))]
        public async Task<ProveedorResponseDto> InsertProveedor(ProveedorRequestDto requestDto) =>
            await _fachadaPersonasService.ProveedorManagementInsert(requestDto).ConfigureAwait(false);

        [HttpPost(nameof(UpdateProveedor))]
        public async Task<ProveedorResponseDto> UpdateProveedor(ProveedorRequestDto requestDto) =>
            await _fachadaPersonasService.ProveedorManagementUpdate(requestDto).ConfigureAwait(false);

        [HttpPost(nameof(DeleteProveedor))]
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
        #endregion
    }
}