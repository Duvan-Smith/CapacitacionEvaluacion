using Evaluacion.Aplicacion.Dto.Especificas.Clientes;
using Evaluacion.Aplicacion.Dto.Especificas.Empleados;
using Evaluacion.Aplicacion.Dto.Especificas.Proveedores;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Evaluacion.Aplicacion.Core.AdministracionPersonas.Personas.FachadaPersonas
{
    public interface IFachadaPersonasService
    {
        public Task<ClienteDto> ClienteManagementGet(ClienteRequestDto requestDto);
        public Task<ClienteResponseDto> ClienteManagementInsert(ClienteRequestDto requestDto);
        public Task<ClienteResponseDto> ClienteManagementDelete(ClienteRequestDto requestDto);
        public Task<ClienteResponseDto> ClienteManagementUpdate(ClienteRequestDto requestDto);
        public Task<IEnumerable<ClienteDto>> ClienteManagementGetAll();
        public Task<string> ClienteManagementExportAll();

        public Task<EmpleadoDto> EmpleadoManagementGet(EmpleadoRequestDto requestDto);
        public Task<EmpleadoResponseDto> EmpleadoManagementInsert(EmpleadoRequestDto requestDto);
        public Task<EmpleadoResponseDto> EmpleadoManagementDelete(EmpleadoRequestDto requestDto);
        public Task<EmpleadoResponseDto> EmpleadoManagementUpdate(EmpleadoRequestDto requestDto);
        public Task<IEnumerable<EmpleadoDto>> EmpleadoManagementGetAll();
        public Task<string> EmpleadoManagementExportAll();

        public Task<ProveedorDto> ProveedorManagementGet(ProveedorRequestDto requestDto);
        public Task<ProveedorResponseDto> ProveedorManagementInsert(ProveedorRequestDto requestDto);
        public Task<ProveedorResponseDto> ProveedorManagementDelete(ProveedorRequestDto requestDto);
        public Task<ProveedorResponseDto> ProveedorManagementUpdate(ProveedorRequestDto requestDto);
        public Task<IEnumerable<ProveedorDto>> ProveedorManagementGetAll();
        public Task<string> ProveedorManagementExportAll();
    }
}
