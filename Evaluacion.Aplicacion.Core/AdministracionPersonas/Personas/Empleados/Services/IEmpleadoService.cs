using Evaluacion.Aplicacion.Dto.Especificas.Empleados;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Evaluacion.Aplicacion.Core.AdministracionPersonas.Personas.Empleados.Services
{
    public interface IEmpleadoService
    {
        public Task<Guid> Insert(EmpleadoRequestDto requestDto);
        public Task<bool> Delete(EmpleadoRequestDto requestDto);
        public Task<EmpleadoDto> Get(EmpleadoRequestDto requestDto);
        public Task<IEnumerable<EmpleadoDto>> GetAll();
        public Task<bool> Update(EmpleadoRequestDto requestDto);
        public Task<string> ExportAll();
        public Task<IEnumerable<EmpleadoRequestDto>> ImportAll();

    }
}
