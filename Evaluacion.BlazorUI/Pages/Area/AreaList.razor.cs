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

        public string UrlEditar;


        private string _currentSelectedTask;

        public void IdTablaComportamiento(string currentSelectedTask) =>
            _currentSelectedTask = currentSelectedTask;
        public void UrlTablaComportamiento(string urlEditar) =>
            UrlEditar = urlEditar;

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


    }
}
