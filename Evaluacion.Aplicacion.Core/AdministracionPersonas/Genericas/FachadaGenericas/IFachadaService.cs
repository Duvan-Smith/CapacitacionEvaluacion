using Evaluacion.Aplicacion.Dto.Genericas.Areas;
using Evaluacion.Aplicacion.Dto.Genericas.TipoDocumentos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Evaluacion.Aplicacion.Core.AdministracionPersonas.Genericas.FachadaGenericas
{
    public interface IFachadaService
    {
        public Task<AreaDto> AreaManagementGet(AreaRequestDto requestDto);
        public Task<AreaDto> AreaManagementInsert(AreaRequestDto requestDto);
        public Task<AreaDto> AreaManagementDelete(AreaRequestDto requestDto);
        public Task<AreaDto> AreaManagementUpdate(AreaRequestDto requestDto);
        public Task<IEnumerable<AreaDto>> AreaManagementGetAll();

        public Task<TipoDocumentoDto> TipoDocumentoManagementGet(TipoDocumentoRequestDto requestDto);
        public Task<TipoDocumentoDto> TipoDocumentoManagementInsert(TipoDocumentoRequestDto requestDto);
        public Task<TipoDocumentoDto> TipoDocumentoManagementDelete(TipoDocumentoRequestDto requestDto);
        public Task<TipoDocumentoDto> TipoDocumentoManagementUpdate(TipoDocumentoRequestDto requestDto);
        public Task<IEnumerable<TipoDocumentoDto>> TipoDocumentoManagementGetAll();

    }
}
