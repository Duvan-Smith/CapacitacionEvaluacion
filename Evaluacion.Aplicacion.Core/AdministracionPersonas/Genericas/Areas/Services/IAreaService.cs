using Evaluacion.Aplicacion.Dto.Genericas.Areas;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Evaluacion.Aplicacion.Core.AdministracionPersonas.Genericas.Areas.Services
{
    public interface IAreaService
    {
        public Task<Guid> InsertArea(AreaRequestDto requestDto);
        public Task<bool> DeleteArea(AreaRequestDto requestDto);
        public Task<AreaDto> GetArea(AreaRequestDto requestDto);
        public Task<IEnumerable<AreaDto>> GetAllArea();
        public Task<bool> UpdateArea(AreaRequestDto requestDto);
    }
}
