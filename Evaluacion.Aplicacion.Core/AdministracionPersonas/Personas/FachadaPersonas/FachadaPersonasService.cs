using Evaluacion.Aplicacion.Core.AdministracionPersonas.Personas.Clientes.Services;
using Evaluacion.Aplicacion.Core.AdministracionPersonas.Personas.Empleados.Services;
using Evaluacion.Aplicacion.Core.AdministracionPersonas.Personas.Proveedores.Services;
using Evaluacion.Aplicacion.Dto.Especificas.Clientes;
using Evaluacion.Aplicacion.Dto.Especificas.Empleados;
using Evaluacion.Aplicacion.Dto.Especificas.Proveedores;
using System.Collections.Generic;
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
        public Task<ClienteResponseDto> ClienteManagementDelete(ClienteRequestDto requestDto)
        {
            throw new System.NotImplementedException();
        }

        public Task<ClienteDto> ClienteManagementGet(ClienteRequestDto requestDto)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<ClienteDto>> ClienteManagementGetAll()
        {
            throw new System.NotImplementedException();
        }

        public Task<ClienteResponseDto> ClienteManagementInsert(ClienteRequestDto requestDto)
        {
            throw new System.NotImplementedException();
        }

        public Task<ClienteResponseDto> ClienteManagementUpdate(ClienteRequestDto requestDto)
        {
            throw new System.NotImplementedException();
        }

        public Task<EmpleadoResponseDto> EmpleadoManagementDelete(EmpleadoRequestDto requestDto)
        {
            throw new System.NotImplementedException();
        }

        public Task<EmpleadoDto> EmpleadoManagementGet(EmpleadoRequestDto requestDto)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<EmpleadoDto>> EmpleadoManagementGetAll()
        {
            throw new System.NotImplementedException();
        }

        public Task<EmpleadoResponseDto> EmpleadoManagementInsert(EmpleadoRequestDto requestDto)
        {
            throw new System.NotImplementedException();
        }

        public Task<EmpleadoResponseDto> EmpleadoManagementUpdate(EmpleadoRequestDto requestDto)
        {
            throw new System.NotImplementedException();
        }

        public Task<ProveedorResponseDto> ProveedorManagementDelete(ProveedorRequestDto requestDto)
        {
            throw new System.NotImplementedException();
        }

        public Task<ProveedorDto> ProveedorManagementGet(ProveedorRequestDto requestDto)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<ProveedorDto>> ProveedorManagementGetAll()
        {
            throw new System.NotImplementedException();
        }

        public Task<ProveedorResponseDto> ProveedorManagementInsert(ProveedorRequestDto requestDto)
        {
            throw new System.NotImplementedException();
        }

        public Task<ProveedorResponseDto> ProveedorManagementUpdate(ProveedorRequestDto requestDto)
        {
            throw new System.NotImplementedException();
        }
    }
}
