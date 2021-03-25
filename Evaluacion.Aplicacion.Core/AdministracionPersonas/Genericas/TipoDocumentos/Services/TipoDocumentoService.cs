using Evaluacion.Aplicacion.Dto.Genericas.TipoDocumentos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Evaluacion.Aplicacion.Core.AdministracionPersonas.Genericas.TipoDocumentos.Services
{
    public class TipoDocumentoService : ITipoDocumentoService
    {
        public Task<bool> DeleteTipoDocumento(TipoDocumentoRequestDto requestDto)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TipoDocumentoDto>> GetAllTipoDocumento()
        {
            throw new NotImplementedException();
        }

        public Task<TipoDocumentoDto> GetTipoDocumento(TipoDocumentoRequestDto requestDto)
        {
            throw new NotImplementedException();
        }

        public Task<Guid> InsertTipoDocumento(TipoDocumentoRequestDto requestDto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateTipoDocumento(TipoDocumentoRequestDto requestDto)
        {
            throw new NotImplementedException();
        }
    }
}
