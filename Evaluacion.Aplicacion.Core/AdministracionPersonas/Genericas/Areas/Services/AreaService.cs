using AutoMapper;
using Evaluacion.Aplicacion.Core.AdministracionPersonas.Genericas.Areas.Excepciones;
using Evaluacion.Aplicacion.Dto.Genericas.Areas;
using Evaluacion.Dominio.Core.Especificas.Empleados;
using Evaluacion.Dominio.Core.Genericas.Areas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Evaluacion.Aplicacion.Core.AdministracionPersonas.Genericas.Areas.Services
{
    public class AreaService : IAreaService
    {
        private readonly IAreaRepositorio _areaRepositorio;
        private readonly IMapper _mapper;
        private readonly IEmpleadoRepositorio _empleadoRepositorio;

        public AreaService(IAreaRepositorio areaRepositorio, IMapper mapper, IEmpleadoRepositorio empleadoRepositorio)
        {
            _mapper = mapper;
            _areaRepositorio = areaRepositorio;
            _empleadoRepositorio = empleadoRepositorio;
        }
        public Task<bool> DeleteArea(AreaRequestDto requestDto)
        {
            ValidationDto(requestDto);

            var empleado = _empleadoRepositorio
                .SearchMatching<EmpleadoEntity>(x => x.AreaId == requestDto.Id)
                .Any();
            if (empleado)
                throw new EmpleadoAreaAlreadyExistException(requestDto.NombreArea);
            var entity = _mapper.Map<AreaEntity>(requestDto);
            return Task.FromResult(_areaRepositorio.Delete(entity));
        }

        public Task<IEnumerable<AreaDto>> GetAllArea()
        {
            var area = _areaRepositorio
                .GetAll<AreaEntity>();
            return Task.FromResult(_mapper.Map<IEnumerable<AreaDto>>(area));
        }

        public Task<AreaDto> GetArea(AreaRequestDto requestDto)
        {
            ValidationDto(requestDto);
            var area = _areaRepositorio
                .SearchMatching<AreaEntity>(x => x.Id == requestDto.Id);
            return Task.FromResult(_mapper.Map<AreaDto>(area.FirstOrDefault()));
        }

        public async Task<Guid> InsertArea(AreaRequestDto requestDto)
        {
            ValidationDto(requestDto);
            var usernameExist = _areaRepositorio
                .SearchMatching<AreaEntity>(x => x.NombreArea == requestDto.NombreArea)
                .Any();

            if (usernameExist)
                throw new AreanameAlreadyExistException(requestDto.NombreArea);

            var response = await _areaRepositorio.Insert(_mapper.Map<AreaEntity>(requestDto)).ConfigureAwait(false);

            return response.Id;
        }

        public Task<bool> UpdateArea(AreaRequestDto requestDto)
        {
            ValidationDto(requestDto);
            var entity = _areaRepositorio.SearchMatchingOneResult<AreaEntity>(x => x.Id == requestDto.Id);
            entity.NombreArea = requestDto.NombreArea;
            entity.EmpleadoResponsableId = requestDto.EmpleadoResponsableId;

            return Task.FromResult(_areaRepositorio.Update(entity));
        }

        private static void ValidationDto(AreaRequestDto requestDto)
        {
            if (requestDto == null)
                throw new AreaRequestDtoNullException();
        }
    }
}
