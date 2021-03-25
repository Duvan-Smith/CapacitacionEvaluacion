using Evaluacion.Aplicacion.Core.AdministracionPersonas.Personas.Clientes.Services;
using Evaluacion.Aplicacion.Core.AdministracionPersonas.Personas.Empleados.Services;
using Evaluacion.Aplicacion.Core.AdministracionPersonas.Personas.Proveedores.Services;
using Evaluacion.Aplicacion.Dto.Especificas.Clientes;
using Evaluacion.Aplicacion.Dto.Especificas.Empleados;
using Evaluacion.Aplicacion.Dto.Especificas.Proveedores;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Evaluacion.Aplicacion.Core.AdministracionPersonas.Personas.FachadaPersonas
{
    public class FachadaPersonasService : IFachadaPersonasService
    {
        private readonly IClienteService _clienteService;
        private readonly IEmpleadoService _empleadoService;
        private readonly IProveedorService _proveedorService;

        public FachadaPersonasService(IClienteService clienteService, IEmpleadoService empleadoService, IProveedorService proveedorService)
        {
            _clienteService = clienteService;
            _empleadoService = empleadoService;
            _proveedorService = proveedorService;
        }
        #region ClienteServices
        public async Task<ClienteResponseDto> ClienteManagementDelete(ClienteRequestDto requestDto)
        {
            var result = await _clienteService.Delete(requestDto).ConfigureAwait(false);
            return new ClienteResponseDto
            {
                Aceptado = result,
                StatusCode = result ? HttpStatusCode.OK : HttpStatusCode.Unauthorized,
                StatusDescription = result ? "Delete" : "No delete"
            };
        }

        public Task<ClienteDto> ClienteManagementGet(ClienteRequestDto requestDto)
        {
            return _clienteService.Get(requestDto);
        }

        public Task<IEnumerable<ClienteDto>> ClienteManagementGetAll()
        {
            return _clienteService.GetAll();
        }

        public async Task<ClienteResponseDto> ClienteManagementInsert(ClienteRequestDto requestDto)
        {
            var result = await _clienteService.Insert(requestDto).ConfigureAwait(false) != default;
            return new ClienteResponseDto
            {
                Aceptado = result,
                StatusCode = result ? HttpStatusCode.OK : HttpStatusCode.Unauthorized,
                StatusDescription = result ? "Insert" : "No insert"
            };
        }

        public async Task<ClienteResponseDto> ClienteManagementUpdate(ClienteRequestDto requestDto)
        {
            var result = await _clienteService.Update(requestDto).ConfigureAwait(false) != default;
            return new ClienteResponseDto
            {
                Aceptado = result,
                StatusCode = result ? HttpStatusCode.OK : HttpStatusCode.Unauthorized,
                StatusDescription = result ? "Update" : "No Update"
            };
        }
        #endregion
        #region EmpleadoServices
        public async Task<EmpleadoResponseDto> EmpleadoManagementDelete(EmpleadoRequestDto requestDto)
        {
            var result = await _empleadoService.Delete(requestDto).ConfigureAwait(false);
            return new EmpleadoResponseDto
            {
                Aceptado = result,
                StatusCode = result ? HttpStatusCode.OK : HttpStatusCode.Unauthorized,
                StatusDescription = result ? "Delete" : "No delete"
            };
        }

        public Task<EmpleadoDto> EmpleadoManagementGet(EmpleadoRequestDto requestDto)
        {
            return _empleadoService.Get(requestDto);
        }

        public Task<IEnumerable<EmpleadoDto>> EmpleadoManagementGetAll()
        {
            return _empleadoService.GetAll();
        }

        public async Task<EmpleadoResponseDto> EmpleadoManagementInsert(EmpleadoRequestDto requestDto)
        {
            var result = await _empleadoService.Insert(requestDto).ConfigureAwait(false) != default;
            return new EmpleadoResponseDto
            {
                Aceptado = result,
                StatusCode = result ? HttpStatusCode.OK : HttpStatusCode.Unauthorized,
                StatusDescription = result ? "Insert" : "No insert"
            };
        }

        public async Task<EmpleadoResponseDto> EmpleadoManagementUpdate(EmpleadoRequestDto requestDto)
        {
            var result = await _empleadoService.Update(requestDto).ConfigureAwait(false) != default;
            return new EmpleadoResponseDto
            {
                Aceptado = result,
                StatusCode = result ? HttpStatusCode.OK : HttpStatusCode.Unauthorized,
                StatusDescription = result ? "Update" : "No Update"
            };
        }
        #endregion
        #region ProveedorServices
        public async Task<ProveedorResponseDto> ProveedorManagementDelete(ProveedorRequestDto requestDto)
        {
            var result = await _proveedorService.Delete(requestDto).ConfigureAwait(false);
            return new ProveedorResponseDto
            {
                Aceptado = result,
                StatusCode = result ? HttpStatusCode.OK : HttpStatusCode.Unauthorized,
                StatusDescription = result ? "Delete" : "No delete"
            };
        }

        public Task<ProveedorDto> ProveedorManagementGet(ProveedorRequestDto requestDto)
        {
            return _proveedorService.Get(requestDto);
        }

        public Task<IEnumerable<ProveedorDto>> ProveedorManagementGetAll()
        {
            return _proveedorService.GetAll();
        }

        public async Task<ProveedorResponseDto> ProveedorManagementInsert(ProveedorRequestDto requestDto)
        {
            var result = await _proveedorService.Insert(requestDto).ConfigureAwait(false) != default;
            return new ProveedorResponseDto
            {
                Aceptado = result,
                StatusCode = result ? HttpStatusCode.OK : HttpStatusCode.Unauthorized,
                StatusDescription = result ? "Insert" : "No insert"
            };
        }

        public async Task<ProveedorResponseDto> ProveedorManagementUpdate(ProveedorRequestDto requestDto)
        {
            var result = await _proveedorService.Update(requestDto).ConfigureAwait(false) != default;
            return new ProveedorResponseDto
            {
                Aceptado = result,
                StatusCode = result ? HttpStatusCode.OK : HttpStatusCode.Unauthorized,
                StatusDescription = result ? "Update" : "No Update"
            };
        }
        #endregion
    }
}
