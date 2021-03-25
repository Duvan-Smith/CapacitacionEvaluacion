using Evaluacion.Aplicacion.Dto.Especificas.Clientes;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Evaluacion.Aplicacion.Core.AdministracionPersonas.Personas.Clientes.Services
{
    public class ClienteService : IClienteService
    {
        public Task<bool> Delete(ClienteRequestDto requestDto)
        {
            throw new NotImplementedException();
        }

        public Task<ClienteDto> Get(ClienteRequestDto requestDto)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ClienteDto>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<Guid> Insert(ClienteRequestDto requestDto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(ClienteRequestDto requestDto)
        {
            throw new NotImplementedException();
        }
    }
}
