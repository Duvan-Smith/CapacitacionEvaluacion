﻿using AutoMapper;
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
            var entity = ValidationEntity(requestDto);
            return Task.FromResult(_proveedorRepositorio.Delete(entity));
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
            if (!listentity.Any())
                throw new ProveedorNoExistException();
            return Task.FromResult(_mapper.Map<IEnumerable<ProveedorDto>>(listentity));
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

            ValidationParameterInsert(_mapper.Map<ProveedorRequestDto>(entity));

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
                .SearchMatching<ProveedorEntity>(x => x.CodigoTipoDocumento == requestDto.CodigoTipoDocumento && x.TipoDocumentoId == requestDto.TipoDocumentoId && x.Id != requestDto.Id);
            if (idExist.Any())
                throw new ProveedorCodigoTipoDocumentoException(idExist.First().CodigoTipoDocumento);

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
