using Evaluacion.Aplicacion.Core.AdministracionPersonas.Genericas.Areas.Services;
using Evaluacion.Aplicacion.Core.AdministracionPersonas.Genericas.TipoDocumentos.Services;
using Evaluacion.Aplicacion.Dto.Genericas.Areas;
using Evaluacion.Aplicacion.Dto.Genericas.TipoDocumentos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Evaluacion.Aplicacion.Core.AdministracionPersonas.Genericas.FachadaGenericas
{
    public class FachadaService : IFachadaService
    {
        private readonly IAreaService _areaService;
        private readonly ITipoDocumentoService _tipoDocumentoService;

        public FachadaService(IAreaService areaService, ITipoDocumentoService tipoDocumentoService)
        {
            _areaService = areaService;
            _tipoDocumentoService = tipoDocumentoService;
        }

        public Task<AreaDto> AreaManagementDelete(AreaRequestDto requestDto)
        {
            throw new System.NotImplementedException();
        }

        public Task<AreaDto> AreaManagementGet(AreaRequestDto requestDto)
        {
            return _areaService.GetAreaByArea(requestDto);
        }

        public Task<IEnumerable<AreaDto>> AreaManagementGetAll()
        {
            throw new System.NotImplementedException();
        }

        public Task<AreaDto> AreaManagementInsert(AreaRequestDto requestDto)
        {
            throw new System.NotImplementedException();
        }

        public Task<AreaDto> AreaManagementUpdate(AreaRequestDto requestDto)
        {
            throw new System.NotImplementedException();
        }

        public Task<TipoDocumentoDto> TipoDocumentoManagementDelete(TipoDocumentoRequestDto requestDto)
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

        public Task<TipoDocumentoDto> TipoDocumentoManagementInsert(TipoDocumentoRequestDto requestDto)
        {
            throw new System.NotImplementedException();
        }

        public Task<TipoDocumentoDto> TipoDocumentoManagementUpdate(TipoDocumentoRequestDto requestDto)
        {
            throw new System.NotImplementedException();
        }
    }
}
