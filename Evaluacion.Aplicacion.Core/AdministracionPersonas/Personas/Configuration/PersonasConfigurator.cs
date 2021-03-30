using Evaluacion.Aplicacion.Core.AdministracionPersonas.Personas.Clientes.Services;
using Evaluacion.Aplicacion.Core.AdministracionPersonas.Personas.Empleados.Services;
using Evaluacion.Aplicacion.Core.AdministracionPersonas.Personas.FachadaPersonas;
using Evaluacion.Aplicacion.Core.AdministracionPersonas.Personas.Proveedores.Services;
using Evaluacion.Aplicacion.Core.IntegracionPersonas.Cofiguration;
using Evaluacion.Aplicacion.Core.Mapper.Configuration;
using Evaluacion.Infraestructura.Datos.Persistencia.Core.Base.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Evaluacion.Aplicacion.Core.AdministracionPersonas.Personas.Configuration
{
    public static class PersonasConfigurator
    {
        public static void ConfigurePersonasService(this IServiceCollection services, DbSettings settings)
        {
            services.TryAddTransient<IClienteService, ClienteService>();
            services.TryAddTransient<IEmpleadoService, EmpleadoService>();
            services.TryAddTransient<IProveedorService, ProveedorService>();
            services.TryAddTransient<IFachadaPersonasService, FachadaPersonasService>();

            services.ConfigureIntegracionPersonaService();
            services.ConfigureMapper();
            services.ConfigureBaseRepository(settings);
        }
    }
}
