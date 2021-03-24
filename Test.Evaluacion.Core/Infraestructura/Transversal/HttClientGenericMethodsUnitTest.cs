using Evaluacion.Infraestructura.Transversal.Exceptions;
using Evaluacion.Infraestructura.Transversal.MetodosGenericos;
using Evaluacion.Infraestructura.Transversal.MetodosGenericos.Cofiguration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using Xunit.Categories;

namespace Test.Evaluacion.Core.Infraestructura.Transversal
{
    public class HttClientGenericMethodsUnitTest
    {
        [Fact]
        [UnitTest]
        public async Task Throws_UriFormatException_on_settings_null_or_empty()
        {
            await Assert.ThrowsAsync<UriFormatException>(() => Task.FromResult(new HttpGenericClient(null, new HttpClient()))).ConfigureAwait(false);
        }
        [Fact]
        [UnitTest]
        public async Task Throws_HttpClientNotDefinedException_on_baseUrl_null_or_empty()
        {
            var service = new ServiceCollection();
            service.ConfigureHttpClientService(new HttpClientSettings());
            var httpSettings = new HttpClientSettings
            {
                ServiceProtocol = null,
                Port = 0,
                Context = null,
                Hostname = null
            };
            service.Configure<HttpClientSettings>(o => o.CopyFrom(httpSettings));
            var provider = service.BuildServiceProvider();
            await Assert.ThrowsAsync<HttpClientNotDefinedException>(() => Task.FromResult(provider.GetRequiredService<IHttpGenericClient>())).ConfigureAwait(false);
        }
    }
}
