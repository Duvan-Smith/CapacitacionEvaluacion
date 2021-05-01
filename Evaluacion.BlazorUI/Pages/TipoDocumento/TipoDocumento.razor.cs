using Evaluacion.Aplicacion.Dto.Genericas.TipoDocumentos;
using Evaluacion.Infraestructura.Transversal.ClientesHttp.AdministracionPersonas.Genericas.TipoDocumentos;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Evaluacion.BlazorUI.Pages.TipoDocumento
{
    public partial class TipoDocumento
    {
        [Inject]
        public ITipoDocumentoClienteHttp ClienteHttp { get; set; }
        private IEnumerable<TipoDocumentoDto> tipoDocumentoDtos;
        private string _currentSelectedTask;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                var response = await ClienteHttp.GetAll().ConfigureAwait(false);
                tipoDocumentoDtos = response ?? new List<TipoDocumentoDto>();
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
                _currentSelectedTask = string.Format("tipoDocumentoDtos Nr. {0} has been selected", ((TipoDocumentoDto)row).Id);
            }
            this.StateHasChanged();
        }
    }
}
