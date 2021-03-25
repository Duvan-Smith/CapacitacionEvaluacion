using Evaluacion.Aplicacion.Dto.Especificas.Empleados;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Evaluacion.Aplicacion.Core.AdministracionPersonas.Personas.Empleados.Services
{
    public class EmpleadoService : IEmpleadoService
    {
        public Task<bool> Delete(EmpleadoRequestDto requestDto)
        {
            throw new NotImplementedException();
        }

        public Task<EmpleadoDto> Get(EmpleadoRequestDto requestDto)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<EmpleadoDto>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<Guid> Insert(EmpleadoRequestDto requestDto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(EmpleadoRequestDto requestDto)
        {
            throw new NotImplementedException();
        }
    }
}
