using Evaluacion.Aplicacion.Dto.Especificas.Clientes;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Evaluacion.Aplicacion.Core.AdministracionPersonas.Personas.Clientes.Services
{
    public interface IClienteService
    {
        public Task<Guid> Insert(ClienteRequestDto requestDto);
        public Task<bool> Delete(ClienteRequestDto requestDto);
        public Task<ClienteDto> Get(ClienteRequestDto requestDto);
        public Task<IEnumerable<ClienteDto>> GetAll();
        public Task<bool> Update(ClienteRequestDto requestDto);
        public Task<string> ExportAll();
        public Task<IEnumerable<ClienteRequestDto>> ImportAll();
    }
}
