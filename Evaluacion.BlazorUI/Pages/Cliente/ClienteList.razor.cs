using Evaluacion.Aplicacion.Dto.Especificas.Clientes;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Evaluacion.BlazorUI.Pages.Cliente
{
    public partial class ClienteList
    {
        private readonly string Url = "/FachadaCliente/GetAllCliente";
        private IEnumerable<ClienteDto> clienteDtos;
        private string _currentSelectedTask;

        protected override async Task OnInitializedAsync()
        {
            clienteDtos = await Http.GetFromJsonAsync<IEnumerable<ClienteDto>>(Url).ConfigureAwait(false);
        }
        public void SelectionChangedEvent(object row)
        {
            if (row == null)
            {
                _currentSelectedTask = "";
            }
            else
            {
                _currentSelectedTask = string.Format("clienteDtos Nr. {0} has been selected", ((ClienteDto)row).Id);
            }
            this.StateHasChanged();
        }
    }
}
