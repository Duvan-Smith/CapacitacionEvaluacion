using AutoMapper;
using Evaluacion.Aplicacion.Core.AdministracionPersonas.Personas.Clientes.Excepciones;
using Evaluacion.Aplicacion.Core.IntegracionPersonas;
using Evaluacion.Aplicacion.Dto.Especificas.Clientes;
using Evaluacion.Dominio.Core.Especificas.Clientes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Evaluacion.Aplicacion.Core.AdministracionPersonas.Personas.Clientes.Services
{
    public class ClienteService : IClienteService
    {
        private readonly IClienteRepositorio _clienteRepositorio;
        private readonly IMapper _mapper;
        private readonly IIntegracionPersonaService _integracionPersonaService;

        public ClienteService(IClienteRepositorio clienteRepositorio, IMapper mapper, IIntegracionPersonaService integracionPersonaService)
        {
            _mapper = mapper;
            _clienteRepositorio = clienteRepositorio;
            _integracionPersonaService = integracionPersonaService;
        }
        public Task<bool> Delete(ClienteRequestDto requestDto)
        {
            ValidationDto(requestDto);
            var entity = _mapper.Map<ClienteEntity>(requestDto);
            return Task.FromResult(_clienteRepositorio.Delete(entity));
        }
        public Task<ClienteDto> Get(ClienteRequestDto requestDto)
        {
            ValidationDto(requestDto);
            var user = _clienteRepositorio
                .SearchMatching<ClienteEntity>(x => x.Id == requestDto.Id);
            return Task.FromResult(_mapper.Map<ClienteDto>(user.FirstOrDefault()));
        }
        public Task<IEnumerable<ClienteDto>> GetAll()
        {
            var area = _clienteRepositorio
                .GetAll<ClienteEntity>();
            return Task.FromResult(_mapper.Map<IEnumerable<ClienteDto>>(area));
        }
        public async Task<Guid> Insert(ClienteRequestDto requestDto)
        {
            ValidationDto(requestDto);
            #region Empleado_Cliente
            if (requestDto.TipoDocumentoId == Guid.Parse("A89DAA40-149F-439A-8A08-7842E09D7376"))
                throw new ClienteTipoDocumentoException(requestDto.TipoDocumentoId.ToString());
            #endregion
            ValidationParameterInsert(requestDto);

            var response = await _clienteRepositorio.Insert(_mapper.Map<ClienteEntity>(requestDto)).ConfigureAwait(false);

            return response.Id;

        }
        public Task<bool> Update(ClienteRequestDto requestDto)
        {
            ValidationDto(requestDto);
            var entity = _clienteRepositorio.SearchMatchingOneResult<ClienteEntity>(x => x.Id == requestDto.Id);
            entity.Nombre = requestDto.Nombre;
            entity.Apellido = requestDto.Apellido;
            entity.FechaNacimiento = requestDto.FechaNacimiento;
            entity.FechaRegistro = requestDto.FechaRegistro;
            entity.NumeroTelefono = requestDto.NumeroTelefono;
            entity.CorreoElectronico = requestDto.CorreoElectronico;
            entity.CodigoTipoDocumento = requestDto.CodigoTipoDocumento;
            //TODO: Agregar demas datos a actualizar
            return Task.FromResult(_clienteRepositorio.Update(entity));
        }
        public async Task<string> ExportAll()
        {
            var listentity = _clienteRepositorio
                .GetAll<ClienteEntity>();

            return await _integracionPersonaService.ExportJson("ExportAllCliente", _mapper.Map<IEnumerable<ClienteDto>>(listentity)).ConfigureAwait(false);
        }
        public async Task<IEnumerable<ClienteRequestDto>> ImportAll()
        {
            var clienteDto = await _integracionPersonaService.ImportJson<IEnumerable<ClienteRequestDto>>("ExportAllEmpleado").ConfigureAwait(false);
            foreach (ClienteRequestDto element in clienteDto)
            {
                await Update(element).ConfigureAwait(false);
            }
            return clienteDto;
        }
        private static void ValidationDto(ClienteRequestDto requestDto)
        {
            if (requestDto == null)
                throw new ClienteRequestDtoNullException();
        }
        private void ValidationParameterInsert(ClienteRequestDto requestDto)
        {
            if (requestDto.TipoPersona == default)
                throw new ClienteTipoPersonaNullException(requestDto.TipoPersona);

            var usernameExist = _clienteRepositorio
                            .SearchMatching<ClienteEntity>(x => x.Nombre == requestDto.Nombre)
                            .Any();
            if (usernameExist)
                throw new ClientenameAlreadyExistException(requestDto.Nombre);

            var codeExist = _clienteRepositorio
                .SearchMatching<ClienteEntity>(x => x.CodigoTipoDocumento == requestDto.CodigoTipoDocumento && x.TipoDocumentoId == requestDto.TipoDocumentoId);
            if (codeExist.Any())
                throw new ClienteCodigoTipoDocumentoException(requestDto.CodigoTipoDocumento);

            if (requestDto.FechaNacimiento == default)
                throw new ClienteFechaNacimientoException(requestDto.FechaNacimiento);
            if (requestDto.FechaRegistro == default)
                throw new ClienteFechaRegistroException(requestDto.FechaRegistro);
        }
    }
}
