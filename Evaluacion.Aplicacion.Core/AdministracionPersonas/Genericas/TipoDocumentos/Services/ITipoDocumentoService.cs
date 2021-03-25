using Evaluacion.Aplicacion.Dto.Genericas.TipoDocumentos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Evaluacion.Aplicacion.Core.AdministracionPersonas.Genericas.TipoDocumentos.Services
{
    public interface ITipoDocumentoService
    {
        public Task<Guid> InsertTipoDocumento(TipoDocumentoRequestDto requestDto);
        public Task<bool> DeleteTipoDocumento(TipoDocumentoRequestDto requestDto);
        public Task<TipoDocumentoDto> GetTipoDocumento(TipoDocumentoRequestDto requestDto);
        public Task<IEnumerable<TipoDocumentoDto>> GetAllTipoDocumento();
        public Task<bool> UpdateTipoDocumento(TipoDocumentoRequestDto requestDto);
    }
}
