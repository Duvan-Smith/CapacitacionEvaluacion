using Evaluacion.Infraestructura.Transversal.ClientesHttp.AdministracionPersonas.Genericas.Areas;
using MatBlazor;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Evaluacion.BlazorUI.Components.Configuration
{
    public static class BlazorConfigurator
    {
        public static void ConfigureBlazorApp(this IServiceCollection services)
        {
            services.AddMatBlazor();
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.TryAddTransient<IAreaClienteHttp, AreaClienteHttp>();

        }
    }
}
