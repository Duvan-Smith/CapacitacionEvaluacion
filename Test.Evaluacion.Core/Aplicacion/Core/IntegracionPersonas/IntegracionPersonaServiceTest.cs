using Evaluacion.Aplicacion.Core.IntegracionPersonas;
using Evaluacion.Aplicacion.Core.IntegracionPersonas.Cofiguration;
using Evaluacion.Aplicacion.Core.IntegracionPersonas.Exceptions;
using Evaluacion.Aplicacion.Dto.Especificas.Clientes;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using Xunit.Categories;

namespace Test.Evaluacion.Core.Aplicacion.Core.IntegracionPersonas
{
    public class IntegracionPersonaServiceTest
    {
        [Fact]
        [UnitTest]
        public async Task Throws_UriFormatException_on_baseUrl_null_or_empty()
        {
            await Assert.ThrowsAsync<UriFormatException>(() => Task.FromResult(new IntegracionPersonaService(null, new HttpClient()))).ConfigureAwait(false);
        }

        [Fact]
        [UnitTest]
        public async Task Patch_Throws_NotImplementedException()
        {
            var service = new ServiceCollection();

            service.ConfigureIntegracionPersonaService(new IntegracionPersonaSettings
            {
                Context = "",
                Hostname = "localhost",
                Port = 44334,
                ServiceProtocol = "http"
            });
            var provider = service.BuildServiceProvider();
            var areaService = provider.GetRequiredService<IIntegracionPersonaService>();

            var dtoCliente = new ClienteDto
            {
                Nombre = "IntegracionPersona",
                Apellido = "IntegracionPersona"
            };

            await Assert.ThrowsAsync<IntegracionPersonaArgumentPathException>(async () => await areaService.Export("", dtoCliente).ConfigureAwait(false)).ConfigureAwait(false);
        }
        [Fact]
        [UnitTest]
        public async Task Export_Full()
        {
            var service = new ServiceCollection();

            service.ConfigureIntegracionPersonaService(new IntegracionPersonaSettings
            {
                Context = "",
                Hostname = "localhost",
                Port = 44334,
                ServiceProtocol = "http"
            });
            var provider = service.BuildServiceProvider();
            var areaService = provider.GetRequiredService<IIntegracionPersonaService>();

            //var cliente = new HttpClient();
            //cliente.BaseAddress= "http://any-url.com"
            //var client = new FakeHttpClient(cliente);

            var dtoCliente = new ClienteDto
            {
                Nombre = "IntegracionPersona",
                Apellido = "IntegracionPersona"
            };

            var result = await areaService.Export("Export", dtoCliente).ConfigureAwait(false);

            Assert.NotNull(result);
        }
    }
}
