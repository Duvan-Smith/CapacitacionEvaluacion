using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Evaluacion.Aplicacion.Core.IntegracionPersonas.Cofiguration
{
    public static class IntegracionPersonaConfigurator
    {
        public static void ConfigureIntegracionPersonaService(this IServiceCollection services) =>
            services.TryAddTransient<IIntegracionPersonaService, IntegracionPersonaService>();
    }
}
