using Evaluacion.Aplicacion.Core.AdministracionPersonas.Genericas.Configuration;
using Evaluacion.Aplicacion.Core.AdministracionPersonas.Genericas.TipoDocumentos.Services;
using Evaluacion.Aplicacion.Core.AdministracionPersonas.Personas.Clientes.Excepciones;
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
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;
using Xunit.Categories;

namespace Test.Evaluacion.Core.Aplicacion.Core.AdministracionPersonas.Personas.Clientes
{
    public class ClienteServiceUpdateTest : ClienteServiceTestBase
    {
        private static ServiceProvider ServiceCollectionCliente()
        {
            var service = new ServiceCollection();

            service.ConfigurePersonasService(new DbSettings
            {
                ConnectionString = "Data Source=Asus;Initial Catalog=evaluacion;Integrated Security=True"
            });
            service.ConfigureGenericasService(new DbSettings
            {
                ConnectionString = "Data Source=Asus;Initial Catalog=evaluacion;Integrated Security=True"
            });

            var provider = service.BuildServiceProvider();
            return provider;
        }
        [Fact]
        [UnitTest]
        public async Task Cliente_Update_Test_Fail()
        {
            var clienteRepoMock = new Mock<IClienteRepositorio>();
            clienteRepoMock
                .Setup(m => m.SearchMatchingOneResult(It.IsAny<Expression<Func<ClienteEntity, bool>>>()));

            var service = new ServiceCollection();

            service.AddTransient(_ => clienteRepoMock.Object);

            service.ConfigurePersonasService(new DbSettings());
            var provider = service.BuildServiceProvider();
            var clienteService = provider.GetRequiredService<IClienteService>();

            await Assert.ThrowsAsync<ClienteNoExistException>(() => clienteService.Update(new ClienteRequestDto { Nombre = "Nombre", Id = Guid.NewGuid() })).ConfigureAwait(false);
        }
        [Fact]
        [UnitTest]
        public async Task Proveedor_Update_Test_Full()
        {
            var clienteRepoMock = new Mock<IClienteRepositorio>();
            var entity = new ClienteEntity
            {
                Id = Guid.NewGuid(),
                Nombre = "Nombre",
                CodigoTipoDocumento = "123456789",
                TipoDocumentoId = Guid.Parse("581E3E67-82E2-4F1F-B379-9BD870DB669E"),
                TipoPersona = (global::Evaluacion.Dominio.Core.Especificas.Personas.TipoPersona)TipoPersona.Natural,
            };
            var tipoDocumentoRepoMock = new Mock<ITipoDocumentoRepositorio>();

            _ = tipoDocumentoRepoMock
                .Setup(m => m.SearchMatching(It.IsAny<Expression<Func<TipoDocumentoEntity, bool>>>()))
                .Returns(new List<TipoDocumentoEntity>{ new TipoDocumentoEntity
                {
                    Id = Guid.NewGuid(),
                    NombreTipoDocumento = "fakenonit"
                }});

            _ = clienteRepoMock
                .Setup(m => m.GetAll<ClienteEntity>())
                .Returns(new List<ClienteEntity> { entity });

            _ = clienteRepoMock
                .Setup(m => m.SearchMatchingOneResult(It.IsAny<Expression<Func<ClienteEntity, bool>>>()))
                .Returns(entity);

            _ = clienteRepoMock
                .Setup(m => m.Update(It.IsAny<ClienteEntity>()))
                .Returns(Task.FromResult(true));

            var service = new ServiceCollection();

            service.AddTransient(_ => clienteRepoMock.Object);
            service.AddTransient(_ => tipoDocumentoRepoMock.Object);

            service.ConfigurePersonasService(new DbSettings());
            var provider = service.BuildServiceProvider();
            var clienteService = provider.GetRequiredService<IClienteService>();

            var result = await clienteService.Update(new ClienteRequestDto
            {
                Id = entity.Id,
                Nombre = "Nombre2",
                TipoPersona = (global::Evaluacion.Aplicacion.Dto.Especificas.Personas.TipoPersona)TipoPersona.Natural,
                FechaNacimiento = DateTimeOffset.Now,
                FechaRegistro = DateTimeOffset.Now,
                CodigoTipoDocumento = "123456789",
                TipoDocumentoId = Guid.Parse("581E3E67-82E2-4F1F-B379-9BD870DB669E")
            }).ConfigureAwait(false);

            Assert.NotNull(result.ToString());
            Assert.True(result);
        }
        [Fact]
        [IntegrationTest]
        public async void Cliente_Update_Test_IntegrationTest()
        {
            ServiceProvider provider = ServiceCollectionCliente();

            var clienteService = provider.GetRequiredService<IClienteService>();
            var clienteRepositorio = provider.GetRequiredService<IClienteRepositorio>();
            var documentoService = provider.GetRequiredService<ITipoDocumentoService>();
            var documentoRepo = provider.GetRequiredService<ITipoDocumentoRepositorio>();

            var dtoDocumento = new TipoDocumentoRequestDto
            {
                Id = Guid.NewGuid(),
                NombreTipoDocumento = "fakeDocumentofakeCliente04",
            };
            var documento = documentoRepo
                .SearchMatching<TipoDocumentoEntity>(x => x.NombreTipoDocumento == dtoDocumento.NombreTipoDocumento || x.Id == dtoDocumento.Id)
                .FirstOrDefault();
            if (documento != null || documento != default)
                _ = await documentoRepo.Delete(documento).ConfigureAwait(false);

            _ = await documentoService.Insert(dtoDocumento).ConfigureAwait(false);

            var dtoCliente = new ClienteRequestDto
            {
                Id = Guid.NewGuid(),
                Nombre = "fake_Cliente04_U",
                Apellido = "fake_Cliente04_U",
                NumeroTelefono = 123456789,
                CorreoElectronico = "fake@fake.fake",
                CodigoTipoDocumento = "0000000011",
                TipoPersona = (global::Evaluacion.Aplicacion.Dto.Especificas.Personas.TipoPersona)TipoPersona.Natural,
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

            var dtoCliente2 = new ClienteRequestDto
            {
                Id = dtoCliente.Id,
                Nombre = "fake2",
                Apellido = "fake2",
                NumeroTelefono = 123456789,
                CorreoElectronico = "fake@fake.fake2",
                //CodigoTipoDocumento = "223456789",
                TipoPersona = (global::Evaluacion.Aplicacion.Dto.Especificas.Personas.TipoPersona)TipoPersona.Juridico,
                FechaNacimiento = DateTimeOffset.Now.AddHours(1),
                //FechaRegistro = DateTimeOffset.Now,
                TipoDocumentoId = dtoDocumento.Id,
            };
            var result = await clienteService.Update(dtoCliente2).ConfigureAwait(false);

            Assert.True(result);

            _ = await clienteService.Delete(dtoCliente).ConfigureAwait(false);
            _ = await documentoService.Delete(dtoDocumento).ConfigureAwait(false);
        }
    }
}
