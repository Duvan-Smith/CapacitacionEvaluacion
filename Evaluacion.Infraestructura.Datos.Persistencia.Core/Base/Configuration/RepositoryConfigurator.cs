using Evaluacion.Dominio.Core.Especificas.Clientes;
using Evaluacion.Dominio.Core.Especificas.Empleados;
using Evaluacion.Dominio.Core.Especificas.Proveedores;
using Evaluacion.Dominio.Core.Genericas.Areas;
using Evaluacion.Dominio.Core.Genericas.TipoDocumentos;
using Evaluacion.Infraestructura.Datos.Persistencia.Core.Especificas.Clientes;
using Evaluacion.Infraestructura.Datos.Persistencia.Core.Especificas.Empleados;
using Evaluacion.Infraestructura.Datos.Persistencia.Core.Especificas.Proveedores;
using Evaluacion.Infraestructura.Datos.Persistencia.Core.Genericas.Areas;
using Evaluacion.Infraestructura.Datos.Persistencia.Core.Genericas.TipoDocumentos;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Evaluacion.Infraestructura.Datos.Persistencia.Core.Base.Configuration
{
    public static class RepositoryConfigurator
    {
        public static void ConfigureBaseRepository(this IServiceCollection services, DbSettings settings)
        {
            //TODO: Lookup
            services.TryAddTransient<IAreaRepositorio, AreaRepositorio>();
            services.TryAddTransient<ITipoDocumentoRepositorio, TipoDocumentoRepositorio>();
            services.TryAddTransient<IClienteRepositorio, ClienteRepositorio>();
            services.TryAddTransient<IEmpleadoRepositorio, EmpleadoRepositorio>();
            services.TryAddTransient<IProveedorRepositorio, ProveedorRepositorio>();

            services.ConfigureContext(settings);
        }

        public static void ConfigureContext(this IServiceCollection services, DbSettings settings)
        {
            services.Configure<DbSettings>(o => o.CopyFrom(settings));
            services.TryAddScoped<IContextDb, ContextoDb>();
        }
    }
}
