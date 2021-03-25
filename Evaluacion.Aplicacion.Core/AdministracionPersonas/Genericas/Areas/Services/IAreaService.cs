using Evaluacion.Aplicacion.Dto.Genericas.Areas;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Evaluacion.Aplicacion.Core.AdministracionPersonas.Genericas.Areas.Services
{
    public interface IAreaService
    {
        public Task<Guid> Agregar<Trequest>(Trequest requestDto);
        public Task<bool> Eliminar<Trequest>(Trequest requestDto);
        public Task<AreaDto> GetAreaByArea<Trequest>(Trequest requestDto);
        public Task<IEnumerable<AreaDto>> GetAllArea();
        public Task<bool> ActualizarArea<Trequest>(Trequest requestDto);
    }
}
