using Microsoft.AspNetCore.Components;

namespace Evaluacion.BlazorUI.Components.Crud
{
    public class CustomeCrud : ComponentBase
    {
        [Parameter]
        public virtual string Tipo { get; set; }

        [Parameter]
        public virtual string Url { get; set; }

        [Parameter]
        public virtual string Etiqueta { get; set; }
    }
}
