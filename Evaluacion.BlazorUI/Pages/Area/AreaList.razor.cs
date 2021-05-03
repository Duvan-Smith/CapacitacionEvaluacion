using Evaluacion.Aplicacion.Dto.Genericas.Areas;
using Evaluacion.Infraestructura.Transversal.ClientesHttp.AdministracionPersonas.Genericas.Areas;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Evaluacion.BlazorUI.Pages.Area
{
    public partial class AreaList
    {
        [Inject]
        public IAreaClienteHttp ClienteHttp { get; set; }
        public IEnumerable<AreaDto> Areas;

        private string _currentSelectedTask;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                var response = await ClienteHttp.GetAll().ConfigureAwait(false);
                Areas = response ?? new List<AreaDto>();
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
                _currentSelectedTask = string.Format("areaDtos Nr. {0} has been selected", ((AreaDto)row).Id);
            }
            StateHasChanged();
        }
    }
}
