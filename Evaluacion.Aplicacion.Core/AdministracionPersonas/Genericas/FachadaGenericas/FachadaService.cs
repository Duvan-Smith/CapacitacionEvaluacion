using Evaluacion.Aplicacion.Core.AdministracionPersonas.Genericas.Areas.Services;
using Evaluacion.Aplicacion.Core.AdministracionPersonas.Genericas.TipoDocumentos.Services;
using Evaluacion.Aplicacion.Dto.Base;
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

        public Task<Tresponse> ManagementDelete<Tresponse, Trequest>(Trequest requestDto) where Tresponse : DataTransferObject
        {
            throw new System.NotImplementedException();
        }

        public Task<Tresponse> ManagementGet<Tresponse, Trequest>(Trequest requestDto) where Tresponse : DataTransferObject
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<Tresponse>> ManagementGetAll<Tresponse>() where Tresponse : DataTransferObject
        {
            throw new System.NotImplementedException();
        }

        public Task<Tresponse> ManagementInsert<Tresponse, Trequest>(Trequest requestDto) where Tresponse : DataTransferObject
        {
            throw new System.NotImplementedException();
        }

        public Task<Tresponse> ManagementUpdate<Tresponse, Trequest>(Trequest requestDto) where Tresponse : DataTransferObject
        {
            throw new System.NotImplementedException();
        }
    }
}
