using Evaluacion.Aplicacion.Dto.Especificas.Clientes;
using Evaluacion.Infraestructura.Transversal.ClientesHttp.AdministracionPersonas.Personas.Clientes;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Evaluacion.BlazorUI.Pages.Cliente
{
    public partial class ClienteList
    {
        [Inject]
        public IClienteClienteHttp ClienteHttp { get; set; }
        private IEnumerable<ClienteDto> clienteDtos;
        private string _currentSelectedTask;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                var response = await ClienteHttp.GetAll().ConfigureAwait(false);
                clienteDtos = response ?? new List<ClienteDto>();
                await InvokeAsync(StateHasChanged).ConfigureAwait(false);
            }
            await base.OnAfterRenderAsync(firstRender);
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
