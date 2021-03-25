using AutoMapper;
using Evaluacion.Aplicacion.Core.AdministracionPersonas.Genericas.TipoDocumentos.Excepciones;
using Evaluacion.Aplicacion.Dto.Genericas.TipoDocumentos;
using Evaluacion.Dominio.Core.Genericas.TipoDocumentos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Evaluacion.Aplicacion.Core.AdministracionPersonas.Genericas.TipoDocumentos.Services
{
    public class TipoDocumentoService : ITipoDocumentoService
    {
        private readonly ITipoDocumentoRepositorio _tipoDocumentoRepositorio;
        private readonly IMapper _mapper;
        public TipoDocumentoService(ITipoDocumentoRepositorio tipoDocumentoRepositorio, IMapper mapper)
        {
            _mapper = mapper;
            _tipoDocumentoRepositorio = tipoDocumentoRepositorio;
        }
        public Task<bool> Delete(TipoDocumentoRequestDto requestDto)
        {
            ValidationDto(requestDto);
            var entity = _mapper.Map<TipoDocumentoEntity>(requestDto);
            return Task.FromResult(_tipoDocumentoRepositorio.Delete(entity));
        }
        public Task<IEnumerable<TipoDocumentoDto>> GetAll()
        {
            var area = _tipoDocumentoRepositorio
                .GetAll<TipoDocumentoEntity>();
            return Task.FromResult(_mapper.Map<IEnumerable<TipoDocumentoDto>>(area));
        }
        public Task<TipoDocumentoDto> Get(TipoDocumentoRequestDto requestDto)
        {
            ValidationDto(requestDto);
            var user = _tipoDocumentoRepositorio
                .SearchMatching<TipoDocumentoEntity>(x => x.Id == requestDto.Id);
            return Task.FromResult(_mapper.Map<TipoDocumentoDto>(user.FirstOrDefault()));
        }
        public async Task<Guid> Insert(TipoDocumentoRequestDto requestDto)
        {
            ValidationDto(requestDto);
            var usernameExist = _tipoDocumentoRepositorio
                .SearchMatching<TipoDocumentoEntity>(x => x.NombreTipoDocumento == requestDto.NombreTipoDocumento)
                .Any();

            if (usernameExist)
                throw new TipoDocumentonameAlreadyExistException(requestDto.NombreTipoDocumento);

            var response = await _tipoDocumentoRepositorio.Insert(_mapper.Map<TipoDocumentoEntity>(requestDto)).ConfigureAwait(false);

            return response.Id;
        }
        public Task<bool> Update(TipoDocumentoRequestDto requestDto)
        {
            ValidationDto(requestDto);
            var entity = _tipoDocumentoRepositorio.SearchMatchingOneResult<TipoDocumentoEntity>(x => x.Id == requestDto.Id);
            entity.NombreTipoDocumento = requestDto.NombreTipoDocumento;

            return Task.FromResult(_tipoDocumentoRepositorio.Update(entity));
        }
        private static void ValidationDto(TipoDocumentoRequestDto requestDto)
        {
            if (requestDto == null)
                throw new TipoDocumentoRequestDtoNullException();
        }
    }
}
