using Evaluacion.Aplicacion.Dto.Genericas.Areas;
using Evaluacion.Aplicacion.Dto.Genericas.TipoDocumentos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Evaluacion.Aplicacion.Core.AdministracionPersonas.Genericas.FachadaGenericas
{
    public interface IFachadaGenericasService
    {
        public Task<AreaDto> AreaManagementGet(AreaRequestDto requestDto);
        public Task<AreaResponseDto> AreaManagementInsert(AreaRequestDto requestDto);
        public Task<AreaResponseDto> AreaManagementDelete(AreaRequestDto requestDto);
        public Task<AreaResponseDto> AreaManagementUpdate(AreaRequestDto requestDto);
        public Task<IEnumerable<AreaDto>> AreaManagementGetAll();

        public Task<TipoDocumentoDto> TipoDocumentoManagementGet(TipoDocumentoRequestDto requestDto);
        public Task<TipoDocumentoResponseDto> TipoDocumentoManagementInsert(TipoDocumentoRequestDto requestDto);
        public Task<TipoDocumentoResponseDto> TipoDocumentoManagementDelete(TipoDocumentoRequestDto requestDto);
        public Task<TipoDocumentoResponseDto> TipoDocumentoManagementUpdate(TipoDocumentoRequestDto requestDto);
        public Task<IEnumerable<TipoDocumentoDto>> TipoDocumentoManagementGetAll();
    }
}
