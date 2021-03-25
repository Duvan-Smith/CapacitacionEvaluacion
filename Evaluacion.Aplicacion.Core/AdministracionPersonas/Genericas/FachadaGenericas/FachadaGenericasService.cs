using Evaluacion.Aplicacion.Core.AdministracionPersonas.Genericas.Areas.Services;
using Evaluacion.Aplicacion.Core.AdministracionPersonas.Genericas.TipoDocumentos.Services;
using Evaluacion.Aplicacion.Dto.Genericas.Areas;
using Evaluacion.Aplicacion.Dto.Genericas.TipoDocumentos;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Evaluacion.Aplicacion.Core.AdministracionPersonas.Genericas.FachadaGenericas
{
    public class FachadaGenericasService : IFachadaGenericasService
    {
        private readonly IAreaService _areaService;
        private readonly ITipoDocumentoService _tipoDocumentoService;

        public FachadaGenericasService(IAreaService areaService, ITipoDocumentoService tipoDocumentoService)
        {
            _areaService = areaService;
            _tipoDocumentoService = tipoDocumentoService;
        }

        public async Task<AreaResponseDto> AreaManagementDelete(AreaRequestDto requestDto)
        {
            var result = await _areaService.DeleteArea(requestDto).ConfigureAwait(false);
            return new AreaResponseDto
            {
                Aceptado = result,
                StatusCode = result ? HttpStatusCode.OK : HttpStatusCode.Unauthorized,
                StatusDescription = result ? "Delete" : "No delete"
            };
        }

        public Task<AreaDto> AreaManagementGet(AreaRequestDto requestDto)
        {
            return _areaService.GetArea(requestDto);
        }

        public Task<IEnumerable<AreaDto>> AreaManagementGetAll()
        {
            return _areaService.GetAllArea();
        }

        public async Task<AreaResponseDto> AreaManagementInsert(AreaRequestDto requestDto)
        {
            var result = await _areaService.InsertArea(requestDto).ConfigureAwait(false) != default;
            return new AreaResponseDto
            {
                Aceptado = result,
                StatusCode = result ? HttpStatusCode.OK : HttpStatusCode.Unauthorized,
                StatusDescription = result ? "Insert" : "No insert"
            };
        }

        public async Task<AreaResponseDto> AreaManagementUpdate(AreaRequestDto requestDto)
        {
            var result = await _areaService.UpdateArea(requestDto).ConfigureAwait(false) != default;
            return new AreaResponseDto
            {
                Aceptado = result,
                StatusCode = result ? HttpStatusCode.OK : HttpStatusCode.Unauthorized,
                StatusDescription = result ? "Insert" : "No insert"
            };
        }

        public Task<TipoDocumentoResponseDto> TipoDocumentoManagementDelete(TipoDocumentoRequestDto requestDto)
        {
            throw new System.NotImplementedException();
        }

        public Task<TipoDocumentoDto> TipoDocumentoManagementGet(TipoDocumentoRequestDto requestDto)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<TipoDocumentoDto>> TipoDocumentoManagementGetAll()
        {
            throw new System.NotImplementedException();
        }

        public Task<TipoDocumentoResponseDto> TipoDocumentoManagementInsert(TipoDocumentoRequestDto requestDto)
        {
            throw new System.NotImplementedException();
        }

        public Task<TipoDocumentoResponseDto> TipoDocumentoManagementUpdate(TipoDocumentoRequestDto requestDto)
        {
            throw new System.NotImplementedException();
        }
    }
}
