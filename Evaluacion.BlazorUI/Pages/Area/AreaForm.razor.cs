using Evaluacion.Aplicacion.Dto.Genericas.Areas;
using Evaluacion.Infraestructura.Transversal.ClientesHttp.AdministracionPersonas.Genericas.Areas;
using Microsoft.AspNetCore.Components;
using System;

namespace Evaluacion.BlazorUI.Pages.Area
{
    public partial class AreaForm
    {
        [Inject]
        public IAreaClienteHttp ClienteHttp { get; set; }

        public string NuevoValor { get; set; }
        public AreaDto AreaDto { get; set; }
        protected async void Agregar()
        {
            var areaDto = new AreaRequestDto { Id = Guid.NewGuid(), NombreArea = NuevoValor, EmpleadoResponsableId = Guid.NewGuid() };
            var response = await ClienteHttp.Post(areaDto).ConfigureAwait(false);
            await InvokeAsync(StateHasChanged).ConfigureAwait(false);
            NavigationManager.NavigateTo("/arealist");
        }
    }
}
