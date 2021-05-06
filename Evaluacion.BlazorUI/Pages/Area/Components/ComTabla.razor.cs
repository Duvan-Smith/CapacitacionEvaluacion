using Evaluacion.Aplicacion.Dto.Genericas.Areas;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;

namespace Evaluacion.BlazorUI.Pages.Area.Components
{
    public partial class ComTabla
    {
        [Parameter]
        public IEnumerable<AreaDto> Areas { get; set; }

        [Parameter]
        public string UrlEditar { get; set; }

        [Parameter]
        public EventCallback<string> SelecionarId { get; set; }

        [Parameter]
        public EventCallback<string> SelecionarUrl { get; set; }

        private string _currentSelectedTask { get; set; }

        public void SelectionChangedEvent(object row)
        {
            if (row == null)
            {
                _currentSelectedTask = "";
            }
            else
            {
                _currentSelectedTask = ((AreaDto)row).Id.ToString();
            }
            UrlEditar = "/areaform/{" + _currentSelectedTask + "}";
            SelecionarId.InvokeAsync(_currentSelectedTask);
            SelecionarUrl.InvokeAsync(UrlEditar);
            StateHasChanged();
        }
    }
}
