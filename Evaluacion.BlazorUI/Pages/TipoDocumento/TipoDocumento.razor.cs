using Evaluacion.Aplicacion.Dto.Genericas.TipoDocumentos;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Evaluacion.BlazorUI.Pages.TipoDocumento
{
    public partial class TipoDocumento
    {
        private readonly string Url = "/FachadaTipoDocumento/GetAllTipoDocumento";
        private IEnumerable<TipoDocumentoDto> tipoDocumentoDtos;
        private string _currentSelectedTask;
        protected override async Task OnInitializedAsync() =>
            tipoDocumentoDtos = await Http.GetFromJsonAsync<IEnumerable<TipoDocumentoDto>>(Url).ConfigureAwait(false);
        public void SelectionChangedEvent(object row)
        {
            if (row == null)
            {
                _currentSelectedTask = "";
            }
            else
            {
                _currentSelectedTask = string.Format("tipoDocumentoDtos Nr. {0} has been selected", ((TipoDocumentoDto)row).Id);
            }
            this.StateHasChanged();
        }
    }
}
