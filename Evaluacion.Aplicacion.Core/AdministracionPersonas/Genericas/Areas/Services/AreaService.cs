using Evaluacion.Aplicacion.Dto.Genericas.Areas;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Evaluacion.Aplicacion.Core.AdministracionPersonas.Genericas.Areas.Services
{
    public class AreaService : IAreaService
    {
        public Task<bool> DeleteArea(AreaRequestDto requestDto)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<AreaDto>> GetAllArea()
        {
            throw new NotImplementedException();
        }

        public Task<AreaDto> GetArea(AreaRequestDto requestDto)
        {
            throw new NotImplementedException();
        }

        public Task<Guid> InsertArea(AreaRequestDto requestDto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateArea(AreaRequestDto requestDto)
        {
            throw new NotImplementedException();
        }
    }
}
