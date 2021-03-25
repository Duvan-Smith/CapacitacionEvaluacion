using Evaluacion.Aplicacion.Dto.Genericas.Areas;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Evaluacion.Aplicacion.Core.AdministracionPersonas.Genericas.Areas.Services
{
    public class AreaService : IAreaService
    {
        public Task<bool> ActualizarArea(AreaRequestDto request)
        {
            throw new NotImplementedException();
        }

        public Task<Guid> Agregar(AreaRequestDto request)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Eliminar(AreaRequestDto request)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<AreaDto>> GetAllArea()
        {
            throw new NotImplementedException();
        }

        public Task<AreaDto> GetAreaByArea(AreaRequestDto request)
        {
            throw new NotImplementedException();
        }
    }
}
