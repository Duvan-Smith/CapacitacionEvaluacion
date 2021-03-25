using Evaluacion.Aplicacion.Dto.Genericas.Areas;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Evaluacion.Aplicacion.Core.AdministracionPersonas.Genericas.Areas.Services
{
    public interface IAreaService
    {
        public Task<Guid> Insert(AreaRequestDto requestDto);
        public Task<bool> Delete(AreaRequestDto requestDto);
        public Task<AreaDto> Get(AreaRequestDto requestDto);
        public Task<IEnumerable<AreaDto>> GetAll();
        public Task<bool> Update(AreaRequestDto requestDto);
    }
}
