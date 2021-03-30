using Evaluacion.Aplicacion.Core.IntegracionPersonas;
using Evaluacion.Aplicacion.Core.IntegracionPersonas.Cofiguration;
using Evaluacion.Aplicacion.Core.IntegracionPersonas.Exceptions;
using Evaluacion.Aplicacion.Dto.Especificas.Clientes;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Xunit;
using Xunit.Categories;

namespace Test.Evaluacion.Core.Aplicacion.Core.IntegracionPersonas
{
    public class IntegracionPersonaServiceTest
    {
        [Fact]
        [UnitTest]
        public async Task Patch_Throws_NotImplementedException()
        {
            var service = new ServiceCollection();

            service.ConfigureIntegracionPersonaService();
            //service.ConfigureIntegracionPersonaService(new IntegracionPersonaSettings
            //{
            //    Context = "",
            //    Hostname = "localhost",
            //    Port = 44334,
            //    ServiceProtocol = "http"
            //});
            var provider = service.BuildServiceProvider();
            var integracionPersonaService = provider.GetRequiredService<IIntegracionPersonaService>();

            var dtoCliente = new ClienteDto
            {
                Nombre = "IntegracionPersona",
                Apellido = "IntegracionPersona"
            };

            await Assert.ThrowsAsync<IntegracionPersonaArgumentPathException>(async () => await integracionPersonaService.ExportJson("", dtoCliente).ConfigureAwait(false)).ConfigureAwait(false);
        }
        [Fact]
        [UnitTest]
        public async Task Export_Full()
        {
            var service = new ServiceCollection();

            service.ConfigureIntegracionPersonaService();
            //service.ConfigureIntegracionPersonaService(new IntegracionPersonaSettings
            //{
            //    Context = "",
            //    Hostname = "localhost",
            //    Port = 44334,
            //    ServiceProtocol = "http"
            //});
            var provider = service.BuildServiceProvider();
            var integracionPersonaService = provider.GetRequiredService<IIntegracionPersonaService>();

            var dtoCliente = new ClienteDto
            {
                Id = Guid.NewGuid(),
                Nombre = "IntegracionPersona",
                Apellido = "IntegracionPersona",
                FechaNacimiento = DateTimeOffset.Now,
                FechaRegistro = DateTimeOffset.Now,
                NumeroTelefono = 123456789,
                CorreoElectronico = "fake@fake.fake",
            };

            var result = await integracionPersonaService.ExportJson("Export", dtoCliente).ConfigureAwait(false);

            Assert.NotNull(result);
        }
        [Fact]
        [UnitTest]
        public async Task Import_Full()
        {
            var service = new ServiceCollection();

            service.ConfigureIntegracionPersonaService();
            //service.ConfigureIntegracionPersonaService(new IntegracionPersonaSettings
            //{
            //    Context = "",
            //    Hostname = "localhost",
            //    Port = 44334,
            //    ServiceProtocol = "http"
            //});
            var provider = service.BuildServiceProvider();
            var integracionPersonaService = provider.GetRequiredService<IIntegracionPersonaService>();

            //var cliente = new HttpClient();
            //cliente.BaseAddress= "http://any-url.com"
            //var client = new FakeHttpClient(cliente);

            var dtoCliente = new ClienteDto
            {
                Nombre = "IntegracionPersona",
                Apellido = "IntegracionPersona"
            };

            var result = await integracionPersonaService.ImportJson<ClienteDto>("Import").ConfigureAwait(false);
            var result2 = await integracionPersonaService.ImportJson<ClienteDto>("Import").ConfigureAwait(false);

            Assert.NotNull(result);
        }
    }
}
