using Evaluacion.Aplicacion.Dto.Genericas.TipoDocumentos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Evaluacion.Aplicacion.Core.AdministracionPersonas.Genericas.TipoDocumentos.Services
{
    public interface ITipoDocumentoService
    {
        public Task<Guid> Insert(TipoDocumentoRequestDto requestDto);
        public Task<bool> Delete(TipoDocumentoRequestDto requestDto);
        public Task<TipoDocumentoDto> Get(TipoDocumentoRequestDto requestDto);
        public Task<IEnumerable<TipoDocumentoDto>> GetAll();
        public Task<bool> Update(TipoDocumentoRequestDto requestDto);
    }
}
