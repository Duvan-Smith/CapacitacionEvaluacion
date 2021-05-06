using Evaluacion.Aplicacion.Dto.Genericas.Areas;
using Evaluacion.Infraestructura.Transversal.ClientesHttp.AdministracionPersonas.Genericas.Areas;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace Evaluacion.BlazorUI.Pages.Area
{
    public partial class AreaForm
    {
        [Inject]
        public IAreaClienteHttp ClienteHttp { get; set; }

        public bool Loading { get; set; }
        public string NuevoValor { get; set; }

        [Parameter]
        public string Id { get; set; }

        public AreaDto AreaDto { get; set; }
        public AreaRequestDto AreaRequestDto { get; set; }

        protected override async Task OnInitializedAsync()
        {
            if (Id != "")
            {
                var areaDto = new AreaRequestDto { Id = Guid.Parse(Id), NombreArea = "id", EmpleadoResponsableId = Guid.NewGuid() };
                AreaDto = await ClienteHttp.GetId(areaDto).ConfigureAwait(false);
                if (AreaDto != null)
                {
                    NuevoValor = AreaDto.NombreArea;
                    Loading = false;
                }
                else
                    Loading = true;
            }
            _ = base.OnInitializedAsync();
        }
        protected async void Agregar()
        {
            if (AreaDto == null)
            {
                var areaDto = new AreaRequestDto { Id = Guid.NewGuid(), NombreArea = NuevoValor, EmpleadoResponsableId = Guid.NewGuid() };
                var response = await ClienteHttp.Post(areaDto).ConfigureAwait(false);
            }
            else
            {
                var areaDtoEdit = new AreaRequestDto { Id = AreaDto.Id, NombreArea = AreaDto.NombreArea, EmpleadoResponsableId = AreaDto.EmpleadoResponsableId };
                var response = await ClienteHttp.Put(areaDtoEdit).ConfigureAwait(false);
            }
            await InvokeAsync(StateHasChanged).ConfigureAwait(false);
            NavigationManager.NavigateTo("/arealist");
        }
    }
}
