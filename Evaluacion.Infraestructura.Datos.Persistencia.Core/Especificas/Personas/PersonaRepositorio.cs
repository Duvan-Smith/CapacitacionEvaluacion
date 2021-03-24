using Evaluacion.Dominio.Core.Especificas.Personas;
using Evaluacion.Infraestructura.Datos.Persistencia.Core.Base;

namespace Evaluacion.Infraestructura.Datos.Persistencia.Core.Especificas.Personas
{
    public class PersonaRepositorio : RepositorioBase<PersonaEntity>, IPersonaRepositorio
    {
        public PersonaRepositorio(IContextDb unitOfWork) : base(unitOfWork)
        {
        }
    }
}