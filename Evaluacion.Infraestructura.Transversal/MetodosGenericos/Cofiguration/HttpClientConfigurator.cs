using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Evaluacion.Infraestructura.Transversal.MetodosGenericos.Cofiguration
{
    public static class HttpClientConfigurator
    {
        public static void ConfigureHttpClientService(this IServiceCollection services, HttpClientSettings settings)
        {
            services.AddHttpClient<HttpGenericClient>();
            services.Configure<HttpClientSettings>(o => o.CopyFrom(settings));
            services.TryAddTransient<IHttpGenericClient, HttpGenericClient>();
        }
    }
}
