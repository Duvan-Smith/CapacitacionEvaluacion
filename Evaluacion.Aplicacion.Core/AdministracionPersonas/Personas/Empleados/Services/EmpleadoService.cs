using AutoMapper;
using Evaluacion.Aplicacion.Core.AdministracionPersonas.Personas.Empleados.Excepciones;
using Evaluacion.Aplicacion.Dto.Especificas.Empleados;
using Evaluacion.Dominio.Core.Especificas.Empleados;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Evaluacion.Aplicacion.Core.AdministracionPersonas.Personas.Empleados.Services
{
    public class EmpleadoService : IEmpleadoService
    {
        private readonly IEmpleadoRepositorio _empleadoRepositorio;
        private readonly IMapper _mapper;

        public EmpleadoService(IEmpleadoRepositorio empleadoRepositorio, IMapper mapper)
        {
            _mapper = mapper;
            _empleadoRepositorio = empleadoRepositorio;
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
            var usernameExist = _empleadoRepositorio
                .SearchMatching<EmpleadoEntity>(x => x.Nombre == requestDto.Nombre)
                .Any();

            if (usernameExist)
                throw new EmpleadonameAlreadyExistException(requestDto.Nombre);

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
        private static void ValidationDto(EmpleadoRequestDto requestDto)
        {
            if (requestDto == null)
                throw new EmpleadoRequestDtoNullException();
        }
    }
}
