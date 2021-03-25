using Evaluacion.Dominio.Core.Especificas.Clientes;
using Evaluacion.Infraestructura.Datos.Persistencia.Core.Base;

namespace Evaluacion.Infraestructura.Datos.Persistencia.Core.Especificas.Clientes
{
    public class ClienteRepositorio : RepositorioBase<ClienteEntity>, IClienteRepositorio
    {
        public ClienteRepositorio(IContextDb unitOfWork) : base(unitOfWork)
        {
        }
    }
}