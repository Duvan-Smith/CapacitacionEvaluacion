using Evaluacion.Aplicacion.Core.AdministracionPersonas.Genericas.Areas.Services;
using Evaluacion.Aplicacion.Core.AdministracionPersonas.Genericas.TipoDocumentos.Services;
using Evaluacion.Aplicacion.Dto.Genericas.Areas;
using Evaluacion.Aplicacion.Dto.Genericas.TipoDocumentos;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Evaluacion.Aplicacion.Core.AdministracionPersonas.Genericas.FachadaGenericas
{
    internal class FachadaGenericasService : IFachadaGenericasService
    {
        private readonly IAreaService _areaService;
        private readonly ITipoDocumentoService _tipoDocumentoService;

        public FachadaGenericasService(IAreaService areaService, ITipoDocumentoService tipoDocumentoService)
        {
            _areaService = areaService;
            _tipoDocumentoService = tipoDocumentoService;
        }
        #region AreaServices
        public async Task<AreaResponseDto> AreaManagementDelete(AreaRequestDto requestDto)
        {
            var result = await _areaService.Delete(requestDto).ConfigureAwait(false);
            return new AreaResponseDto
            {
                Aceptado = result,
                StatusCode = result ? HttpStatusCode.OK : HttpStatusCode.Unauthorized,
                StatusDescription = result ? "Delete" : "No delete"
            };
        }

        public Task<AreaDto> AreaManagementGet(AreaRequestDto requestDto)
        {
            return _areaService.Get(requestDto);
        }

        public Task<IEnumerable<AreaDto>> AreaManagementGetAll()
        {
            return _areaService.GetAll();
        }

        public async Task<AreaResponseDto> AreaManagementInsert(AreaRequestDto requestDto)
        {
            var result = await _areaService.Insert(requestDto).ConfigureAwait(false) != default;
            return new AreaResponseDto
            {
                Aceptado = result,
                StatusCode = result ? HttpStatusCode.OK : HttpStatusCode.Unauthorized,
                StatusDescription = result ? "Insert" : "No insert"
            };
        }

        public async Task<AreaResponseDto> AreaManagementUpdate(AreaRequestDto requestDto)
        {
            var result = await _areaService.Update(requestDto).ConfigureAwait(false) != default;
            return new AreaResponseDto
            {
                Aceptado = result,
                StatusCode = result ? HttpStatusCode.OK : HttpStatusCode.Unauthorized,
                StatusDescription = result ? "Update" : "No Update"
            };
        }
        #endregion
        #region TipoDocumentoServices
        public async Task<TipoDocumentoResponseDto> TipoDocumentoManagementDelete(TipoDocumentoRequestDto requestDto)
        {
            var result = await _tipoDocumentoService.Delete(requestDto).ConfigureAwait(false);
            return new TipoDocumentoResponseDto
            {
                Aceptado = result,
                StatusCode = result ? HttpStatusCode.OK : HttpStatusCode.Unauthorized,
                StatusDescription = result ? "Delete" : "No delete"
            };
        }

        public Task<TipoDocumentoDto> TipoDocumentoManagementGet(TipoDocumentoRequestDto requestDto)
        {
            return _tipoDocumentoService.Get(requestDto);
        }

        public Task<IEnumerable<TipoDocumentoDto>> TipoDocumentoManagementGetAll()
        {
            return _tipoDocumentoService.GetAll();
        }

        public async Task<TipoDocumentoResponseDto> TipoDocumentoManagementInsert(TipoDocumentoRequestDto requestDto)
        {
            var result = await _tipoDocumentoService.Insert(requestDto).ConfigureAwait(false) != default;
            return new TipoDocumentoResponseDto
            {
                Aceptado = result,
                StatusCode = result ? HttpStatusCode.OK : HttpStatusCode.Unauthorized,
                StatusDescription = result ? "Insert" : "No insert"
            };
        }

        public async Task<TipoDocumentoResponseDto> TipoDocumentoManagementUpdate(TipoDocumentoRequestDto requestDto)
        {
            var result = await _tipoDocumentoService.Update(requestDto).ConfigureAwait(false) != default;
            return new TipoDocumentoResponseDto
            {
                Aceptado = result,
                StatusCode = result ? HttpStatusCode.OK : HttpStatusCode.Unauthorized,
                StatusDescription = result ? "Update" : "No Update"
            };
        }
        #endregion
    }
}
