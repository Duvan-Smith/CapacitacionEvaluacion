using Evaluacion.Aplicacion.Dto.Especificas.Proveedores;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Evaluacion.Aplicacion.Core.AdministracionPersonas.Personas.Proveedores.Services
{
    public interface IProveedorService
    {
        public Task<Guid> Insert(ProveedorRequestDto requestDto);
        public Task<bool> Delete(ProveedorRequestDto requestDto);
        public Task<ProveedorDto> Get(ProveedorRequestDto requestDto);
        public Task<IEnumerable<ProveedorDto>> GetAll();
        public Task<bool> Update(ProveedorRequestDto requestDto);
    }
}
