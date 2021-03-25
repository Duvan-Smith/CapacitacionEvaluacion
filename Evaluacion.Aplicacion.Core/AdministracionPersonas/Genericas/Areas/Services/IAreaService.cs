using Evaluacion.Aplicacion.Dto.Genericas.Areas;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Evaluacion.Aplicacion.Core.AdministracionPersonas.Genericas.Areas.Services
{
    public interface IAreaService
    {
        public Task<Guid> Agregar(AreaRequestDto requestDto);
        public Task<bool> Eliminar(AreaRequestDto requestDto);
        public Task<AreaDto> GetAreaByArea(AreaRequestDto requestDto);
        public Task<IEnumerable<AreaDto>> GetAllArea();
        public Task<bool> ActualizarArea(AreaRequestDto requestDto);
    }
}
