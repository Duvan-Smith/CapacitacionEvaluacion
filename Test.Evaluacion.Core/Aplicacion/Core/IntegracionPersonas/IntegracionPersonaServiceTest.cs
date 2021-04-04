using Evaluacion.Aplicacion.Core.IntegracionPersonas;
using Evaluacion.Aplicacion.Core.IntegracionPersonas.Cofiguration;
using Evaluacion.Aplicacion.Core.IntegracionPersonas.Exceptions;
using Evaluacion.Aplicacion.Dto.Especificas.Clientes;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
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
            var provider = service.BuildServiceProvider();
            var integracionPersonaService = provider.GetRequiredService<IIntegracionPersonaService>();

            var dtoCliente = new List<ClienteDto>{ new ClienteDto
            {
                Nombre = "IntegracionPersona",
                Apellido = "IntegracionPersona"
            } };

            await Assert.ThrowsAsync<IntegracionPersonaArgumentPathException>(async () => await integracionPersonaService.ExportJson("", dtoCliente).ConfigureAwait(false)).ConfigureAwait(false);
        }
        [Fact]
        [IntegrationTest]
        public async Task Export_Full()
        {
            var service = new ServiceCollection();

            service.ConfigureIntegracionPersonaService();
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

            var result = await integracionPersonaService.ExportJson("ExportCliente", new List<ClienteDto> { dtoCliente }).ConfigureAwait(false);
            var dtoClienteList = new List<ClienteDto>{ new ClienteDto
            {
                Id = Guid.NewGuid(),
                Nombre = "IntegracionPersona",
                Apellido = "IntegracionPersona",
                FechaNacimiento = DateTimeOffset.Now,
                FechaRegistro = DateTimeOffset.Now,
                NumeroTelefono = 123456789,
                CorreoElectronico = "fake@fake.fake",
            },
            new ClienteDto
            {
                Id = Guid.NewGuid(),
                Nombre = "IntegracionPersona",
                Apellido = "IntegracionPersona",
                FechaNacimiento = DateTimeOffset.Now,
                FechaRegistro = DateTimeOffset.Now,
                NumeroTelefono = 123456789,
                CorreoElectronico = "fake@fake.fake",
            }};
            var result2 = await integracionPersonaService.ExportJson("ExportListCliente", dtoClienteList).ConfigureAwait(false);

            Assert.NotNull(result);
            Assert.NotNull(result2);
        }
        [Fact]
        [IntegrationTest]
        public async Task Import_Full()
        {
            var service = new ServiceCollection();

            service.ConfigureIntegracionPersonaService();
            var provider = service.BuildServiceProvider();
            var integracionPersonaService = provider.GetRequiredService<IIntegracionPersonaService>();

            var result = await integracionPersonaService.ImportJson<IEnumerable<ClienteDto>>("ExportCliente").ConfigureAwait(false);
            var result2 = await integracionPersonaService.ImportJson<IEnumerable<ClienteDto>>("ExportListCliente").ConfigureAwait(false);

            Assert.NotNull(result);
            Assert.NotNull(result2);
        }
    }
}
