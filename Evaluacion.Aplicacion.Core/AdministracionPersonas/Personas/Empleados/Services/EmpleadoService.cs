using AutoMapper;
using Evaluacion.Aplicacion.Core.AdministracionPersonas.Personas.Empleados.Excepciones;
using Evaluacion.Aplicacion.Core.IntegracionPersonas;
using Evaluacion.Aplicacion.Dto.Especificas.Empleados;
using Evaluacion.Dominio.Core.Especificas.Empleados;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Evaluacion.Aplicacion.Core.AdministracionPersonas.Personas.Empleados.Services
{
    public enum TipoPersona
    {
        Natural = 1,
        Juridico = 2,
    }
    public class EmpleadoService : IEmpleadoService
    {
        private readonly IEmpleadoRepositorio _empleadoRepositorio;
        private readonly IMapper _mapper;
        private readonly IIntegracionPersonaService _integracionPersonaService;

        public EmpleadoService(IEmpleadoRepositorio empleadoRepositorio, IMapper mapper, IIntegracionPersonaService integracionPersonaService)
        {
            _mapper = mapper;
            _empleadoRepositorio = empleadoRepositorio;
            _integracionPersonaService = integracionPersonaService;
        }
        public Task<bool> Delete(EmpleadoRequestDto requestDto)
        {
            ValidationDto(requestDto);
            var entity = _mapper.Map<EmpleadoEntity>(requestDto);
            return Task.FromResult(_empleadoRepositorio.Delete(entity));
        }
        public Task<EmpleadoDto> Get(EmpleadoRequestDto requestDto)
        {
            ValidationDto(requestDto);
            var user = _empleadoRepositorio
                .SearchMatching<EmpleadoEntity>(x => x.Id == requestDto.Id);
            return Task.FromResult(_mapper.Map<EmpleadoDto>(user.FirstOrDefault()));
        }
        public Task<IEnumerable<EmpleadoDto>> GetAll()
        {
            var area = _empleadoRepositorio
                .GetAll<EmpleadoEntity>();
            return Task.FromResult(_mapper.Map<IEnumerable<EmpleadoDto>>(area));
        }
        public async Task<Guid> Insert(EmpleadoRequestDto requestDto)
        {
            ValidationDto(requestDto);
            ValidationEmpleadoDto(requestDto);
            #region Empleado_Cliente
            if (requestDto.TipoDocumentoId == Guid.Parse("A89DAA40-149F-439A-8A08-7842E09D7376"))
                throw new EmpleadoTipoDocumentoException(requestDto.TipoDocumentoId.ToString());
            #endregion
            ValidationParameterInsert(requestDto);

            var response = await _empleadoRepositorio.Insert(_mapper.Map<EmpleadoEntity>(requestDto)).ConfigureAwait(false);

            return response.Id;
        }

        public Task<bool> Update(EmpleadoRequestDto requestDto)
        {
            ValidationDto(requestDto);
            var entity = _empleadoRepositorio.SearchMatchingOneResult<EmpleadoEntity>(x => x.Id == requestDto.Id);
            entity.Nombre = requestDto.Nombre;
            entity.Apellido = requestDto.Apellido;
            entity.FechaNacimiento = requestDto.FechaNacimiento;
            entity.FechaRegistro = requestDto.FechaRegistro;
            entity.NumeroTelefono = requestDto.NumeroTelefono;
            entity.CorreoElectronico = requestDto.CorreoElectronico;
            entity.CodigoTipoDocumento = requestDto.CodigoTipoDocumento;
            //TODO: Agregar demas datos a actualizar
            return Task.FromResult(_empleadoRepositorio.Update(entity));
        }
        public async Task<string> ExportAll()
        {
            var listentity = _empleadoRepositorio
                .GetAll<EmpleadoEntity>();

            return await _integracionPersonaService.ExportJson("ExportAllEmpleado", _mapper.Map<IEnumerable<EmpleadoDto>>(listentity)).ConfigureAwait(false);
        }
        private static void ValidationDto(EmpleadoRequestDto requestDto)
        {
            if (requestDto == null)
                throw new EmpleadoRequestDtoNullException();
        }
        private void ValidationEmpleadoDto(EmpleadoRequestDto requestDto)
        {
            if (nameof(TipoPersona.Juridico) == requestDto.TipoPersona.ToString())
                throw new EmpleadoErrorTipoPersonaException(requestDto.TipoPersona.ToString());
            var usercodeExist = _empleadoRepositorio
                            .SearchMatching<EmpleadoEntity>(x => x.CodigoEmpleado == requestDto.CodigoEmpleado)
                            .Any();
            if (usercodeExist)
                throw new EmpleadocodeAlreadyExistException(requestDto.CodigoEmpleado);
            if (requestDto.AreaId == default)
                throw new EmpleadoAreaIdNullException(requestDto.AreaId);
            var AreaIdExist = _empleadoRepositorio
                            .SearchMatching<EmpleadoEntity>(x => x.Id == requestDto.Id && x.AreaId == requestDto.AreaId)
                            .Any();
            if (AreaIdExist)
                throw new EmpleadoAreaIdAlreadyExistException(requestDto.AreaId);
        }
        private void ValidationParameterInsert(EmpleadoRequestDto requestDto)
        {
            if (requestDto.TipoPersona == default)
                throw new EmpleadoTipoPersonaNullException(requestDto.TipoPersona);

            var usernameExist = _empleadoRepositorio
                            .SearchMatching<EmpleadoEntity>(x => x.Nombre == requestDto.Nombre)
                            .Any();
            if (usernameExist)
                throw new EmpleadonameAlreadyExistException(requestDto.Nombre);

            var codeExist = _empleadoRepositorio
                .SearchMatching<EmpleadoEntity>(x => x.CodigoTipoDocumento == requestDto.CodigoTipoDocumento && x.TipoDocumentoId == requestDto.TipoDocumentoId)
                .Any();
            if (codeExist)
                throw new EmpleadoCodigoTipoDocumentoException(requestDto.CodigoTipoDocumento);

            if (requestDto.FechaNacimiento == default)
                throw new EmpleadoFechaNacimientoException(requestDto.FechaNacimiento);
            if (requestDto.FechaRegistro == default)
                throw new EmpleadoFechaRegistroException(requestDto.FechaRegistro);
        }
    }
}
