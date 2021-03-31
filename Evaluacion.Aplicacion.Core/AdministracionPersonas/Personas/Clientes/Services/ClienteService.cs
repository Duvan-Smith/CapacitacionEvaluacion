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
            var entity = ValidationEntity(requestDto);
            return Task.FromResult(_clienteRepositorio.Delete(entity));
        }
        public Task<ClienteDto> Get(ClienteRequestDto requestDto)
        {
            ValidationDto(requestDto);
            var entity = ValidationEntity(requestDto);
            return Task.FromResult(_mapper.Map<ClienteDto>(entity));
        }
        public Task<IEnumerable<ClienteDto>> GetAll()
        {
            var listentity = _clienteRepositorio
                .GetAll<ClienteEntity>();

            return Task.FromResult(_mapper.Map<IEnumerable<ClienteDto>>(listentity));
        }
        public async Task<Guid> Insert(ClienteRequestDto requestDto)
        {
            ValidationDto(requestDto);
            ValidationCliente(requestDto);

            var listentity = _clienteRepositorio
                .GetAll<ClienteEntity>();

            ValidationParameterDB(requestDto, listentity);

            var response = await _clienteRepositorio.Insert(_mapper.Map<ClienteEntity>(requestDto)).ConfigureAwait(false);

            return response.Id;

        }
        public Task<bool> Update(ClienteRequestDto requestDto)
        {
            ValidationDto(requestDto);
            var entity = ValidationEntity(requestDto);

            if (!string.IsNullOrEmpty(requestDto.Nombre))
                entity.Nombre = requestDto.Nombre;

            if (!string.IsNullOrEmpty(requestDto.Apellido))
                entity.Apellido = requestDto.Apellido;

            if (requestDto.FechaNacimiento != default)
                entity.FechaNacimiento = requestDto.FechaNacimiento;

            if (requestDto.FechaRegistro != default)
                entity.FechaRegistro = requestDto.FechaRegistro;

            if (requestDto.NumeroTelefono != default)
                entity.NumeroTelefono = requestDto.NumeroTelefono;

            if (!string.IsNullOrEmpty(requestDto.CorreoElectronico))
                entity.CorreoElectronico = requestDto.CorreoElectronico;

            if (requestDto.TipoDocumentoId != default)
                entity.TipoDocumentoId = requestDto.TipoDocumentoId;

            if (!string.IsNullOrEmpty(requestDto.CodigoTipoDocumento))
                entity.CodigoTipoDocumento = requestDto.CodigoTipoDocumento;

            if (requestDto.TipoPersona != default)
                entity.TipoPersona = (Dominio.Core.Especificas.Personas.TipoPersona)requestDto.TipoPersona;

            var listentity = _clienteRepositorio
                .GetAll<ClienteEntity>();

            ValidationParameterDB(_mapper.Map<ClienteRequestDto>(entity), listentity);

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
            var clienteDto = await _integracionPersonaService.ImportJson<IEnumerable<ClienteRequestDto>>("ExportAllCliente").ConfigureAwait(false);
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
        private void ValidationParameterDB(ClienteRequestDto requestDto, IEnumerable<ClienteEntity> listEntity)
        {
            if (requestDto.TipoPersona == default)
                throw new ClienteTipoPersonaNullException(requestDto.TipoPersona);

            var usernameExist = listEntity.Where(x => x.Nombre == requestDto.Nombre && x.Id != requestDto.Id).Any();
            if (usernameExist)
                throw new ClientenameAlreadyExistException(requestDto.Nombre);

            var codeExist = listEntity.Where(x => x.CodigoTipoDocumento == requestDto.CodigoTipoDocumento &&
                                                x.TipoDocumentoId == requestDto.TipoDocumentoId &&
                                                x.Id != requestDto.Id);
            if (codeExist.Any())
                throw new ClienteCodigoTipoDocumentoException(requestDto.CodigoTipoDocumento);

            if (requestDto.FechaNacimiento == default)
                throw new ClienteFechaNacimientoException(requestDto.FechaNacimiento);
            if (requestDto.FechaRegistro == default)
                throw new ClienteFechaRegistroException(requestDto.FechaRegistro);
        }
        private ClienteEntity ValidationEntity(ClienteRequestDto requestDto)
        {
            var entity = _clienteRepositorio.SearchMatchingOneResult<ClienteEntity>(x => x.Id == requestDto.Id);
            if (entity == null || entity == default)
                throw new ClienteNoExistException(requestDto.Nombre);
            return entity;
        }
        private static void ValidationCliente(ClienteRequestDto requestDto)
        {
            #region Empleado_Cliente
            if (requestDto.TipoDocumentoId == Guid.Parse("A89DAA40-149F-439A-8A08-7842E09D7376"))
                throw new ClienteTipoDocumentoException(requestDto.TipoDocumentoId.ToString());
            #endregion
        }

    }
}
