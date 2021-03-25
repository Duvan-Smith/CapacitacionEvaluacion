using Evaluacion.Aplicacion.Dto.Especificas.Proveedores;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Evaluacion.Aplicacion.Core.AdministracionPersonas.Personas.Proveedores.Services
{
    public class ProveedorService : IProveedorService
    {
        public Task<bool> Delete(ProveedorRequestDto requestDto)
        {
            throw new NotImplementedException();
        }

        public Task<ProveedorDto> Get(ProveedorRequestDto requestDto)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ProveedorDto>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<Guid> Insert(ProveedorRequestDto requestDto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(ProveedorRequestDto requestDto)
        {
            throw new NotImplementedException();
        }
    }
}
