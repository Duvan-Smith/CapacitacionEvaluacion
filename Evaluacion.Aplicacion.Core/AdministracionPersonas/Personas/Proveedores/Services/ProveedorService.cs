using AutoMapper;
using Evaluacion.Aplicacion.Core.AdministracionPersonas.Personas.Proveedores.Excepciones;
using Evaluacion.Aplicacion.Dto.Especificas.Proveedores;
using Evaluacion.Dominio.Core.Especificas.Proveedores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Evaluacion.Aplicacion.Core.AdministracionPersonas.Personas.Proveedores.Services
{
    public class ProveedorService : IProveedorService
    {
        private readonly IProveedorRepositorio _proveedorRepositorio;
        private readonly IMapper _mapper;

        public ProveedorService(IProveedorRepositorio proveedorRepositorio, IMapper mapper)
        {
            _mapper = mapper;
            _proveedorRepositorio = proveedorRepositorio;
        }
        public Task<bool> Delete(ProveedorRequestDto requestDto)
        {
            ValidationDto(requestDto);
            var entity = _mapper.Map<ProveedorEntity>(requestDto);
            return Task.FromResult(_proveedorRepositorio.Delete(entity));
        }

        public Task<ProveedorDto> Get(ProveedorRequestDto requestDto)
        {
            ValidationDto(requestDto);
            var user = _proveedorRepositorio
                .SearchMatching<ProveedorEntity>(x => x.Id == requestDto.Id);
            return Task.FromResult(_mapper.Map<ProveedorDto>(user.FirstOrDefault()));
        }

        public Task<IEnumerable<ProveedorDto>> GetAll()
        {
            var area = _proveedorRepositorio
                .GetAll<ProveedorEntity>();
            return Task.FromResult(_mapper.Map<IEnumerable<ProveedorDto>>(area));
        }

        public async Task<Guid> Insert(ProveedorRequestDto requestDto)
        {
            ValidationDto(requestDto);
            ValidationParameterInsert(requestDto);

            var response = await _proveedorRepositorio.Insert(_mapper.Map<ProveedorEntity>(requestDto)).ConfigureAwait(false);

            return response.Id;
        }

        public Task<bool> Update(ProveedorRequestDto requestDto)
        {
            ValidationDto(requestDto);
            var entity = _proveedorRepositorio.SearchMatchingOneResult<ProveedorEntity>(x => x.Id == requestDto.Id);
            entity.Nombre = requestDto.Nombre;
            entity.Apellido = requestDto.Apellido;
            entity.FechaNacimiento = requestDto.FechaNacimiento;
            entity.FechaRegistro = requestDto.FechaRegistro;
            entity.NumeroTelefono = requestDto.NumeroTelefono;
            entity.CorreoElectronico = requestDto.CorreoElectronico;
            entity.CodigoTipoDocumento = requestDto.CodigoTipoDocumento;
            //TODO: Agregar demas datos a actualizar
            return Task.FromResult(_proveedorRepositorio.Update(entity));
        }
        private static void ValidationDto(ProveedorRequestDto requestDto)
        {
            if (requestDto == null)
                throw new ProveedorRequestDtoNullException();
        }
        private void ValidationParameterInsert(ProveedorRequestDto requestDto)
        {
            if (requestDto.TipoPersona == default)
                throw new ProveedorTipoPersonaNullException(requestDto.TipoPersona);
            var usernameExist = _proveedorRepositorio
                            .SearchMatching<ProveedorEntity>(x => x.Nombre == requestDto.Nombre)
                            .Any();
            if (usernameExist)
                throw new ProveedornameAlreadyExistException(requestDto.Nombre);

            var idExist = _proveedorRepositorio
                .SearchMatching<ProveedorEntity>(x => x.CodigoTipoDocumento == requestDto.CodigoTipoDocumento && x.TipoDocumentoId == requestDto.TipoDocumentoId);
            if (idExist.Any())
                throw new ProveedorCodigoTipoDocumentoException(idExist.First().CodigoTipoDocumento.ToString());

            if (requestDto.FechaNacimiento == default)
                throw new ProveedorFechaNacimientoException(requestDto.FechaNacimiento);
            if (requestDto.FechaRegistro == default)
                throw new ProveedorFechaRegistroException(requestDto.FechaRegistro);
        }
    }
}
