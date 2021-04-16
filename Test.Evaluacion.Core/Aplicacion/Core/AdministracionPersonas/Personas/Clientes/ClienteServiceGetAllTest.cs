using AutoMapper;
using Evaluacion.Aplicacion.Core.AdministracionPersonas.Genericas.Configuration;
using Evaluacion.Aplicacion.Core.AdministracionPersonas.Genericas.TipoDocumentos.Services;
using Evaluacion.Aplicacion.Core.AdministracionPersonas.Personas.Clientes.Services;
using Evaluacion.Aplicacion.Core.AdministracionPersonas.Personas.Configuration;
using Evaluacion.Aplicacion.Dto.Especificas.Clientes;
using Evaluacion.Aplicacion.Dto.Genericas.TipoDocumentos;
using Evaluacion.Dominio.Core.Especificas.Clientes;
using Evaluacion.Dominio.Core.Genericas.TipoDocumentos;
using Evaluacion.Infraestructura.Datos.Persistencia.Core.Base.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Categories;

namespace Test.Evaluacion.Core.Aplicacion.Core.AdministracionPersonas.Personas.Clientes
{
    public class ClienteServiceTest
    {
        private static ServiceProvider ServiceCollectionCliente()
        {
            var service = new ServiceCollection();

            service.ConfigurePersonasService(new DbSettings
            {
                ConnectionString = "Data Source=DSMITH;Initial Catalog=evaluacion;Integrated Security=True"
            });
            service.ConfigureGenericasService(new DbSettings
            {
                ConnectionString = "Data Source=DSMITH;Initial Catalog=evaluacion;Integrated Security=True"
            });

            var provider = service.BuildServiceProvider();
            return provider;
        }
        [Fact]
        [UnitTest]
        public async Task Cliente_GetAll_Test_Full()
        {
            var clienteRepoMock = new Mock<IClienteRepositorio>();
            var Listentity = new List<ClienteEntity>
            {
                new ClienteEntity
                {
                    Id = Guid.NewGuid(),
                    Nombre = "Nombre"
                }
            };
            clienteRepoMock
                .Setup(m => m.GetAll<ClienteEntity>())
                .Returns(Listentity);

            var service = new ServiceCollection();

            service.AddTransient(_ => clienteRepoMock.Object);

            service.ConfigurePersonasService(new DbSettings());
            var provider = service.BuildServiceProvider();
            var clienteService = provider.GetRequiredService<IClienteService>();
            var mapper = provider.GetRequiredService<IMapper>();

            var result = mapper.Map<IEnumerable<ClienteEntity>>(await clienteService.GetAll().ConfigureAwait(false));

            Assert.NotNull(result.ToString());
            Assert.Equal(Listentity[0].Id, result.First().Id);

        }
        [Fact]
        [IntegrationTest]
        public async void Cliente_GetAll_Test_IntegrationTest()
        {
            ServiceProvider provider = ServiceCollectionCliente();

            var clienteService = provider.GetRequiredService<IClienteService>();
            var clienteRepositorio = provider.GetRequiredService<IClienteRepositorio>();
            var mapper = provider.GetRequiredService<IMapper>();
            var documentoService = provider.GetRequiredService<ITipoDocumentoService>();
            var documentoRepo = provider.GetRequiredService<ITipoDocumentoRepositorio>();

            var dtoDocumento = new TipoDocumentoRequestDto
            {
                Id = Guid.NewGuid(),
                NombreTipoDocumento = "fakeDocumentofakeCliente03",
            };
            var documento = documentoRepo
                .SearchMatching<TipoDocumentoEntity>(x => x.NombreTipoDocumento == dtoDocumento.NombreTipoDocumento || x.Id == dtoDocumento.Id)
                .FirstOrDefault();
            if (documento != null || documento != default)
                _ = await documentoRepo.Delete(documento).ConfigureAwait(false);

            _ = await documentoService.Insert(dtoDocumento).ConfigureAwait(false);

            var dtoCliente = new ClienteRequestDto
            {
                Id = Guid.Parse("45c2a9b5-1eac-48d3-83a4-ff692326e4f7"),
                Nombre = "NombreClienteGetAll",
                Apellido = "NombreClienteGetAll",
                NumeroTelefono = 123456789,
                CorreoElectronico = "fake@fake.fake",
                CodigoTipoDocumento = "000000008",
                TipoPersona = (global::Evaluacion.Aplicacion.Dto.Especificas.Personas.TipoPersona)TipoPersona.Juridico,
                FechaNacimiento = DateTimeOffset.Now,
                FechaRegistro = DateTimeOffset.Now,
                TipoDocumentoId = dtoDocumento.Id,
            };

            var cliente = clienteRepositorio
                .SearchMatching<ClienteEntity>(x => x.Nombre == dtoCliente.Nombre || x.Id == dtoCliente.Id)
                .FirstOrDefault();
            if (cliente != null || cliente != default)
                _ = await clienteRepositorio.Delete(cliente).ConfigureAwait(false);

            _ = await clienteService.Insert(dtoCliente).ConfigureAwait(false);

            var result = mapper.Map<IEnumerable<ClienteEntity>>(await clienteService.GetAll().ConfigureAwait(false));

            Assert.NotNull(result.ToString());
            Assert.True(result.Any());

            _ = await clienteService.Delete(dtoCliente).ConfigureAwait(false);
            _ = await documentoService.Delete(dtoDocumento).ConfigureAwait(false);
        }
    }
}
