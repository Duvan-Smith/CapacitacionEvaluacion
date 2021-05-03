using AutoMapper;
using Evaluacion.Aplicacion.Core.AdministracionPersonas.Personas.Proveedores.Excepciones;
using Evaluacion.Aplicacion.Core.IntegracionPersonas;
using Evaluacion.Aplicacion.Dto.Especificas.Proveedores;
using Evaluacion.Dominio.Core.Especificas.Proveedores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Evaluacion.Aplicacion.Core.AdministracionPersonas.Personas.Proveedores.Services
{
    internal class ProveedorService : IProveedorService
    {
        private readonly IProveedorRepositorio _proveedorRepositorio;
        private readonly IMapper _mapper;
        private readonly IIntegracionPersonaService _integracionPersonaService;

        public ProveedorService(IProveedorRepositorio proveedorRepositorio, IMapper mapper, IIntegracionPersonaService integracionPersonaService)
        {
            _mapper = mapper;
            _proveedorRepositorio = proveedorRepositorio;
            _integracionPersonaService = integracionPersonaService;
        }
        public Task<bool> Delete(ProveedorRequestDto requestDto)
        {
            ValidationDto(requestDto);
            var entity = ValidationEntity(requestDto);
            return _proveedorRepositorio.Delete(entity);
        }
        public Task<ProveedorDto> Get(ProveedorRequestDto requestDto)
        {
            ValidationDto(requestDto);
            var entity = ValidationEntity(requestDto);
            return Task.FromResult(_mapper.Map<ProveedorDto>(entity));
        }
        public Task<IEnumerable<ProveedorDto>> GetAll()
        {
            var listentity = _proveedorRepositorio
                .GetAll<ProveedorEntity>();

            return Task.FromResult(_mapper.Map<IEnumerable<ProveedorDto>>(listentity));
        }
        public async Task<Guid> Insert(ProveedorRequestDto requestDto)
        {
            ValidationDto(requestDto);

            var listentity = _proveedorRepositorio
                .GetAll<ProveedorEntity>();

            ValidationParameterDB(requestDto, listentity);

            var response = await _proveedorRepositorio.Insert(_mapper.Map<ProveedorEntity>(requestDto)).ConfigureAwait(false);

            return response.Id;
        }
        public Task<bool> Update(ProveedorRequestDto requestDto)
        {
            ValidationDto(requestDto);
            var entity = ValidationEntity(requestDto);

            #region Validation If
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
            #endregion

            var listentity = _proveedorRepositorio
                .GetAll<ProveedorEntity>();

            ValidationParameterDB(_mapper.Map<ProveedorRequestDto>(entity), listentity);

            return _proveedorRepositorio.Update(entity);
        }
        public async Task<string> ExportAll()
        {
            var listentity = _proveedorRepositorio
                .GetAll<ProveedorEntity>();

            return await _integracionPersonaService.ExportJson("ExportAllProveedor", _mapper.Map<IEnumerable<ProveedorRequestDto>>(listentity)).ConfigureAwait(false);
        }
        public async Task<IEnumerable<ProveedorRequestDto>> ImportAll()
        {
            var proveedorDto = await _integracionPersonaService.ImportJson<IEnumerable<ProveedorRequestDto>>("ExportAllProveedor").ConfigureAwait(false);
            foreach (ProveedorRequestDto element in proveedorDto)
                await Update(element).ConfigureAwait(false);
            return proveedorDto;
        }
        private static void ValidationDto(ProveedorRequestDto requestDto)
        {
            if (requestDto == null)
                throw new ProveedorRequestDtoNullException();
        }
        private static void ValidationParameterDB(ProveedorRequestDto requestDto, IEnumerable<ProveedorEntity> listEntity)
        {
            if (requestDto.TipoPersona == default)
                throw new ProveedorTipoPersonaNullException(requestDto.TipoPersona);

            var usernameExist = listEntity.Where(x => x.Nombre == requestDto.Nombre && x.Id != requestDto.Id).Any();
            if (usernameExist)
                throw new ProveedornameAlreadyExistException(requestDto.Nombre);

            var codeExist = listEntity.Where(x => x.CodigoTipoDocumento == requestDto.CodigoTipoDocumento &&
                                                x.TipoDocumentoId == requestDto.TipoDocumentoId &&
                                                x.Id != requestDto.Id);
            if (codeExist.Any())
                throw new ProveedorCodigoTipoDocumentoException(codeExist.First().CodigoTipoDocumento);

            if (requestDto.FechaNacimiento == default)
                throw new ProveedorFechaNacimientoException(requestDto.FechaNacimiento);
            if (requestDto.FechaRegistro == default)
                throw new ProveedorFechaRegistroException(requestDto.FechaRegistro);
        }
        private ProveedorEntity ValidationEntity(ProveedorRequestDto requestDto)
        {
            var entity = _proveedorRepositorio.SearchMatchingOneResult<ProveedorEntity>(x => x.Id == requestDto.Id);
            if (entity == null || entity == default)
                throw new ProveedorNoExistException(requestDto.Nombre);
            return entity;
        }
    }
}
