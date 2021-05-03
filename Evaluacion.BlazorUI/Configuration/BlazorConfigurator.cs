using Evaluacion.Infraestructura.Transversal.ClientesHttp.AdministracionPersonas.Genericas.Areas;
using Evaluacion.Infraestructura.Transversal.ClientesHttp.AdministracionPersonas.Genericas.TipoDocumentos;
using Evaluacion.Infraestructura.Transversal.ClientesHttp.AdministracionPersonas.Personas.Clientes;
using Evaluacion.Infraestructura.Transversal.ClientesHttp.AdministracionPersonas.Personas.Empleados;
using Evaluacion.Infraestructura.Transversal.ClientesHttp.AdministracionPersonas.Personas.Proveedores;
using MatBlazor;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Evaluacion.BlazorUI.Configuration
{
    public static class BlazorConfigurator
    {
        public static void ConfigureBlazorApp(this IServiceCollection services)
        {
            services.AddMatBlazor();
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.TryAddTransient<IAreaClienteHttp, AreaClienteHttp>();
            services.TryAddTransient<IClienteClienteHttp, ClienteClienteHttp>();
            services.TryAddTransient<IEmpleadoClienteHttp, EmpleadoClienteHttp>();
            services.TryAddTransient<IProveedorClienteHttp, ProveedorClienteHttp>();
            services.TryAddTransient<ITipoDocumentoClienteHttp, TipoDocumentoClienteHttp>();

        }
    }
}
