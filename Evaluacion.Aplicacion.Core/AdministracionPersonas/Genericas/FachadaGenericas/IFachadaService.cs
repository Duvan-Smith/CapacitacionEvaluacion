using Evaluacion.Aplicacion.Dto.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Evaluacion.Aplicacion.Core.AdministracionPersonas.Genericas.FachadaGenericas
{
    public interface IFachadaService
    {
        public Task<Tresponse> ManagementGet<Tresponse, Trequest>(Trequest requestDto) where Tresponse : DataTransferObject;
        public Task<Tresponse> ManagementInsert<Tresponse, Trequest>(Trequest requestDto) where Tresponse : DataTransferObject;
        public Task<Tresponse> ManagementDelete<Tresponse, Trequest>(Trequest requestDto) where Tresponse : DataTransferObject;
        public Task<Tresponse> ManagementUpdate<Tresponse, Trequest>(Trequest requestDto) where Tresponse : DataTransferObject;
        public Task<IEnumerable<Tresponse>> ManagementGetAll<Tresponse>() where Tresponse : DataTransferObject;
    }
}
