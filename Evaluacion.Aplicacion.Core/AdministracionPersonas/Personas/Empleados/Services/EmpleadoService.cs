using AutoMapper;
using Evaluacion.Aplicacion.Core.AdministracionPersonas.Personas.Empleados.Excepciones;
using Evaluacion.Aplicacion.Core.IntegracionPersonas;
using Evaluacion.Aplicacion.Dto.Especificas.Empleados;
using Evaluacion.Dominio.Core.Especificas.Empleados;
using Evaluacion.Dominio.Core.Genericas.TipoDocumentos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Evaluacion.Aplicacion.Core.AdministracionPersonas.Personas.Empleados.Services
{
    internal enum TipoPersona
    {
        Natural = 1,
        Juridico = 2,
    }
    internal class EmpleadoService : IEmpleadoService
    {
        private readonly IEmpleadoRepositorio _empleadoRepositorio;
        private readonly IMapper _mapper;
        private readonly IIntegracionPersonaService _integracionPersonaService;
        private readonly ITipoDocumentoRepositorio _tipoDocumentoRepositorio;

        public EmpleadoService(IEmpleadoRepositorio empleadoRepositorio, IMapper mapper, IIntegracionPersonaService integracionPersonaService, ITipoDocumentoRepositorio tipoDocumentoRepositorio)
        {
            _mapper = mapper;
            _empleadoRepositorio = empleadoRepositorio;
            _integracionPersonaService = integracionPersonaService;
            _tipoDocumentoRepositorio = tipoDocumentoRepositorio;
        }
        public Task<bool> Delete(EmpleadoRequestDto requestDto)
        {
            ValidationDto(requestDto);
            var entity = ValidationEntity(requestDto);
            return _empleadoRepositorio.Delete(entity);
        }
        public Task<EmpleadoDto> Get(EmpleadoRequestDto requestDto)
        {
            ValidationDto(requestDto);
            var entity = ValidationEntity(requestDto);
            return Task.FromResult(_mapper.Map<EmpleadoDto>(entity));
        }
        public Task<IEnumerable<EmpleadoDto>> GetAll()
        {
            var listentity = _empleadoRepositorio
                .GetAll<EmpleadoEntity>();

            return Task.FromResult(_mapper.Map<IEnumerable<EmpleadoDto>>(listentity));
        }
        public async Task<Guid> Insert(EmpleadoRequestDto requestDto)
        {
            ValidationDto(requestDto);

            var listentity = _empleadoRepositorio
                .GetAll<EmpleadoEntity>();

            ValidationEmpleadoDto(requestDto, listentity);
            ValidationEmpleadoAndCliente(requestDto);
            ValidationInsert(requestDto, listentity);

            ValidationParameterDB(requestDto, listentity);

            var response = await _empleadoRepositorio.Insert(_mapper.Map<EmpleadoEntity>(requestDto)).ConfigureAwait(false);

            return response.Id;
        }
        private void ValidationEmpleadoAndCliente(EmpleadoRequestDto requestDto)
        {
            var listdocumento = _tipoDocumentoRepositorio
                .SearchMatching<TipoDocumentoEntity>(x => x.Id == requestDto.TipoDocumentoId).FirstOrDefault();
            if (listdocumento.NombreTipoDocumento.ToLower() == "nit".ToLower())
                throw new EmpleadoTipoDocumentoException(requestDto.TipoDocumentoId.ToString());
        }
        public Task<bool> Update(EmpleadoRequestDto requestDto)
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

            if (requestDto.Salario != default)
                entity.Salario = requestDto.Salario;

            if (!string.IsNullOrEmpty(requestDto.CodigoEmpleado))
                entity.CodigoEmpleado = requestDto.CodigoEmpleado;

            if (requestDto.AreaId != default)
                entity.AreaId = requestDto.AreaId;
            #endregion

            var listentity = _empleadoRepositorio
                .GetAll<EmpleadoEntity>();

            ValidationEmpleadoDto(_mapper.Map<EmpleadoRequestDto>(entity), listentity);
            ValidationEmpleadoAndCliente(requestDto);
            ValidationParameterDB(_mapper.Map<EmpleadoRequestDto>(entity), listentity);

            return _empleadoRepositorio.Update(entity);
        }
        public async Task<string> ExportAll()
        {
            var listentity = _empleadoRepositorio
                .GetAll<EmpleadoEntity>();

            return await _integracionPersonaService.ExportJson("ExportAllEmpleado", _mapper.Map<IEnumerable<EmpleadoDto>>(listentity)).ConfigureAwait(false);
        }
        public async Task<IEnumerable<EmpleadoRequestDto>> ImportAll()
        {
            var empleadoDto = await _integracionPersonaService.ImportJson<IEnumerable<EmpleadoRequestDto>>("ExportAllEmpleado").ConfigureAwait(false);
            foreach (EmpleadoRequestDto element in empleadoDto)
                await Update(element).ConfigureAwait(false);
            return empleadoDto;
        }
        private static void ValidationDto(EmpleadoRequestDto requestDto)
        {
            if (requestDto == null)
                throw new EmpleadoRequestDtoNullException();
        }
        private static void ValidationEmpleadoDto(EmpleadoRequestDto requestDto, IEnumerable<EmpleadoEntity> listEntity)
        {
            if (nameof(TipoPersona.Juridico) == requestDto.TipoPersona.ToString())
                throw new EmpleadoErrorTipoPersonaException(requestDto.TipoPersona.ToString());

            var usercodeExist = listEntity.Where(x => x.CodigoEmpleado == requestDto.CodigoEmpleado && x.Id != requestDto.Id).Any();
            if (usercodeExist)
                throw new EmpleadocodeAlreadyExistException(requestDto.CodigoEmpleado);

            if (requestDto.AreaId == default)
                throw new EmpleadoAreaIdNullException(requestDto.AreaId);
        }
        private static void ValidationInsert(EmpleadoRequestDto requestDto, IEnumerable<EmpleadoEntity> listEntity)
        {
            var AreaIdExist = listEntity.Where(x => x.Id == requestDto.Id && x.AreaId == requestDto.AreaId).Any();
            if (AreaIdExist)
                throw new EmpleadoAreaIdAlreadyExistException(requestDto.AreaId);
        }
        private static void ValidationParameterDB(EmpleadoRequestDto requestDto, IEnumerable<EmpleadoEntity> listEntity)
        {
            if (requestDto.TipoPersona == default)
                throw new EmpleadoTipoPersonaNullException(requestDto.TipoPersona);

            var usernameExist = listEntity.Where(x => x.Nombre == requestDto.Nombre && x.Id != requestDto.Id).Any();
            if (usernameExist)
                throw new EmpleadonameAlreadyExistException(requestDto.Nombre);

            var codeExist = listEntity.Where(x => x.CodigoTipoDocumento == requestDto.CodigoTipoDocumento &&
                                                x.TipoDocumentoId == requestDto.TipoDocumentoId &&
                                                x.Id != requestDto.Id);
            if (codeExist.Any())
                throw new EmpleadoCodigoTipoDocumentoException(requestDto.CodigoTipoDocumento);

            if (requestDto.FechaNacimiento == default)
                throw new EmpleadoFechaNacimientoException(requestDto.FechaNacimiento);
            if (requestDto.FechaRegistro == default)
                throw new EmpleadoFechaRegistroException(requestDto.FechaRegistro);
        }
        private EmpleadoEntity ValidationEntity(EmpleadoRequestDto requestDto)
        {
            var entity = _empleadoRepositorio.SearchMatchingOneResult<EmpleadoEntity>(x => x.Id == requestDto.Id);
            if (entity == null || entity == default)
                throw new EmpleadoNoExistException(requestDto.Nombre);
            return entity;
        }
    }
}
