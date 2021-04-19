using Evaluacion.Aplicacion.Dto.Genericas.Areas;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Evaluacion.BlazorUI.Pages.Area
{
    public partial class AreaList
    {
        private readonly string Url = "/FachadaArea/GetAllArea";
        private IEnumerable<AreaDto> areaDtos;
        private string _currentSelectedTask;
        protected override async Task OnInitializedAsync() =>
            areaDtos = await Http.GetFromJsonAsync<IEnumerable<AreaDto>>(Url).ConfigureAwait(false);
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
            this.StateHasChanged();
        }
    }
}
