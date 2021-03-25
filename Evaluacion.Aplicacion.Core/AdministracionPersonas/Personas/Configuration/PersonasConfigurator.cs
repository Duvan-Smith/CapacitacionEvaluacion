using Evaluacion.Aplicacion.Core.Mapper.Configuration;
using Evaluacion.Infraestructura.Datos.Persistencia.Core.Base.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Evaluacion.Aplicacion.Core.AdministracionPersonas.Personas.Configuration
{
    public static class PersonasConfigurator
    {
        public static void ConfigurePersonasService(this IServiceCollection services, DbSettings settings)
        {
            //services.TryAddTransient<IService, Service>();
            //services.TryAddTransient<IService, Service>();
            //services.TryAddTransient<IService, Service>();

            services.ConfigureMapper();
            services.ConfigureBaseRepository(settings);
        }
    }
}
