using Evaluacion.Aplicacion.Core.AdministracionPersonas.Personas.Clientes.Excepciones;
using Evaluacion.Aplicacion.Core.AdministracionPersonas.Personas.Clientes.Services;
using Evaluacion.Aplicacion.Core.AdministracionPersonas.Personas.Configuration;
using Evaluacion.Aplicacion.Dto.Especificas.Clientes;
using Evaluacion.Dominio.Core.Especificas.Clientes;
using Evaluacion.Infraestructura.Datos.Persistencia.Core.Base.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;
using Xunit.Categories;

namespace Test.Evaluacion.Core.Aplicacion.Core.AdministracionPersonas.Personas.Clientes
{
    public enum TipoPersona
    {
        Natural = 1,
        Juridico = 2,
    }
    public class ClienteServiceTest
    {
        [Fact]
        [UnitTest]
        public async Task Check_AllParameterNull_Cliente_Exception()
        {
            var service = new ServiceCollection();

            service.ConfigurePersonasService(new DbSettings());
            var provider = service.BuildServiceProvider();
            var clienteService = provider.GetRequiredService<IClienteService>();

            await Assert.ThrowsAsync<ClienteRequestDtoNullException>(() => clienteService.Update(null)).ConfigureAwait(false);
            await Assert.ThrowsAsync<ClienteRequestDtoNullException>(() => clienteService.Delete(null)).ConfigureAwait(false);
            await Assert.ThrowsAsync<ClienteRequestDtoNullException>(() => clienteService.Get(null)).ConfigureAwait(false);
            await Assert.ThrowsAsync<ClienteRequestDtoNullException>(() => clienteService.Insert(null)).ConfigureAwait(false);
        }
        //TODO: Cliente, No puede haber dos personas con el mismo nombre / razón social
        [Fact]
        [UnitTest]
        public async Task No_Se_Repite_Nombre_Cliente_Fail()
        {
            var clienteRepoMock = new Mock<IClienteRepositorio>();

            clienteRepoMock
                .Setup(m => m.SearchMatching(It.IsAny<Expression<Func<ClienteEntity, bool>>>()))
                .Returns(new List<ClienteEntity> { new ClienteEntity
                {
                    Id = Guid.NewGuid(),
                    Nombre = "FakePrueba"
                }});

            var service = new ServiceCollection();

            service.AddTransient(_ => clienteRepoMock.Object);

            service.ConfigurePersonasService(new DbSettings());

            var provider = service.BuildServiceProvider();
            var clienteService = provider.GetRequiredService<IClienteService>();

            var clienteDto = new ClienteRequestDto
            {
                Nombre = "fake",
                TipoPersona = (global::Evaluacion.Aplicacion.Dto.Especificas.Personas.TipoPersona)TipoPersona.Natural,
                FechaNacimiento = DateTimeOffset.Now,
                FechaRegistro = DateTimeOffset.Now,
                TipoDocumentoId = Guid.Parse("581E3E67-82E2-4F1F-B379-9BD870DB669E")
            };

            await Assert.ThrowsAsync<ClientenameAlreadyExistException>(() => clienteService.Insert(clienteDto)).ConfigureAwait(false);
        }
        [Fact]
        [UnitTest]
        public async Task No_Se_Repite_Nombre_Cliente_Full()
        {
            var clienteGetRepoMock = new Mock<IClienteRepositorio>();
            var clienteInsertRepoMock = new Mock<IClienteRepositorio>();

            clienteGetRepoMock
                .Setup(m => m.SearchMatching(It.IsAny<Expression<Func<ClienteEntity, bool>>>()));
            clienteInsertRepoMock
                .Setup(m => m.Insert(It.IsAny<ClienteEntity>()))
                .Returns(Task.FromResult(new ClienteEntity { Id = Guid.NewGuid() }));

            var service = new ServiceCollection();

            service.AddTransient(_ => clienteGetRepoMock.Object);
            service.AddTransient(_ => clienteInsertRepoMock.Object);

            service.ConfigurePersonasService(new DbSettings());
            var provider = service.BuildServiceProvider();
            var clienteService = provider.GetRequiredService<IClienteService>();
            var result = await clienteService.Insert(new ClienteRequestDto
            {
                Nombre = "fake",
                TipoPersona = (global::Evaluacion.Aplicacion.Dto.Especificas.Personas.TipoPersona)TipoPersona.Natural,
                FechaNacimiento = DateTimeOffset.Now,
                FechaRegistro = DateTimeOffset.Now,
                TipoDocumentoId = Guid.Parse("581E3E67-82E2-4F1F-B379-9BD870DB669E")
            }).ConfigureAwait(false);

            Assert.NotNull(result.ToString());
            Assert.NotEqual(default, result);
        }
        //TODO: Cliente, No puede haber dos personas con el mismo numero y tipo de identificación
        #region Test Funcional
        //Se debe comentar el SearchMatching de nombre para que funcione 
        //[Fact]
        //[UnitTest]
        //public async Task No_Se_Repite_Id_Cliente_Fail()
        //{
        //    var clienteRepoMock = new Mock<IClienteRepositorio>();

        //    clienteRepoMock
        //        .Setup(m => m.SearchMatching(It.IsAny<Expression<Func<ClienteEntity, bool>>>()))
        //        .Returns(new List<ClienteEntity> { new ClienteEntity
        //        {
        //            Id = Guid.NewGuid(),
        //            Nombre = "FakePrueba"
        //        }});

        //    var service = new ServiceCollection();

        //    service.AddTransient(_ => clienteRepoMock.Object);

        //    service.ConfigurePersonasService(new DbSettings());

        //    var provider = service.BuildServiceProvider();
        //    var clienteService = provider.GetRequiredService<IClienteService>();

        //    var clienteDto = new ClienteRequestDto
        //    {
        //        Nombre = "FakePrueba"
        //    };

        //    await Assert.ThrowsAsync<ClienteidAlreadyExistException>(() => clienteService.Insert(clienteDto)).ConfigureAwait(false);
        //}
        #endregion
        [Fact]
        [UnitTest]
        public async Task No_Se_Repite_Id_Cliente_Full()
        {
            var clienteGetRepoMock = new Mock<IClienteRepositorio>();
            var clienteInsertRepoMock = new Mock<IClienteRepositorio>();

            clienteGetRepoMock
                .Setup(m => m.SearchMatching(It.IsAny<Expression<Func<ClienteEntity, bool>>>()));
            clienteInsertRepoMock
                .Setup(m => m.Insert(It.IsAny<ClienteEntity>()))
                .Returns(Task.FromResult(new ClienteEntity { Id = Guid.NewGuid() }));

            var service = new ServiceCollection();

            service.AddTransient(_ => clienteGetRepoMock.Object);
            service.AddTransient(_ => clienteInsertRepoMock.Object);

            service.ConfigurePersonasService(new DbSettings());
            var provider = service.BuildServiceProvider();
            var clienteService = provider.GetRequiredService<IClienteService>();
            var result = await clienteService.Insert(new ClienteRequestDto
            {
                Nombre = "fake",
                TipoPersona = (global::Evaluacion.Aplicacion.Dto.Especificas.Personas.TipoPersona)TipoPersona.Natural,
                FechaNacimiento = DateTimeOffset.Now,
                FechaRegistro = DateTimeOffset.Now,
                TipoDocumentoId = Guid.Parse("581E3E67-82E2-4F1F-B379-9BD870DB669E")
            }).ConfigureAwait(false);

            Assert.NotNull(result.ToString());
            Assert.NotEqual(default, result);
        }
        //TODO: Cliente, La fecha de nacimiento / creación es obligatoria
        [Fact]
        [UnitTest]
        public async Task Cliente_Validar_Fechas_Fail()
        {
            var clienteGetRepoMock = new Mock<IClienteRepositorio>();
            var clienteInsertRepoMock = new Mock<IClienteRepositorio>();

            clienteGetRepoMock
                .Setup(m => m.SearchMatching(It.IsAny<Expression<Func<ClienteEntity, bool>>>()));
            clienteInsertRepoMock
                .Setup(m => m.Insert(It.IsAny<ClienteEntity>()))
                .Returns(Task.FromResult(new ClienteEntity { Id = Guid.NewGuid() }));

            var service = new ServiceCollection();

            service.AddTransient(_ => clienteGetRepoMock.Object);
            service.AddTransient(_ => clienteInsertRepoMock.Object);

            service.ConfigurePersonasService(new DbSettings());
            var provider = service.BuildServiceProvider();
            var clienteService = provider.GetRequiredService<IClienteService>();

            await Assert.ThrowsAsync<ClienteFechaNacimientoException>(() => clienteService.Insert(new ClienteRequestDto
            {
                Nombre = "FakePrueba",
                TipoPersona = (global::Evaluacion.Aplicacion.Dto.Especificas.Personas.TipoPersona)TipoPersona.Natural,
                FechaNacimiento = default,
                FechaRegistro = DateTimeOffset.Now,
                TipoDocumentoId = Guid.Parse("581E3E67-82E2-4F1F-B379-9BD870DB669E")
            })).ConfigureAwait(false);
            await Assert.ThrowsAsync<ClienteFechaRegistroException>(() => clienteService.Insert(new ClienteRequestDto
            {
                Nombre = "FakePrueba",
                TipoPersona = (global::Evaluacion.Aplicacion.Dto.Especificas.Personas.TipoPersona)TipoPersona.Natural,
                FechaNacimiento = DateTimeOffset.Now,
                FechaRegistro = default,
                TipoDocumentoId = Guid.Parse("581E3E67-82E2-4F1F-B379-9BD870DB669E")
            })).ConfigureAwait(false);
        }
        [Fact]
        [UnitTest]
        public async Task Cliente_Validar_Fechas_Full()
        {
            var clienteGetRepoMock = new Mock<IClienteRepositorio>();
            var clienteInsertRepoMock = new Mock<IClienteRepositorio>();

            clienteGetRepoMock
                .Setup(m => m.SearchMatching(It.IsAny<Expression<Func<ClienteEntity, bool>>>()));
            clienteInsertRepoMock
                .Setup(m => m.Insert(It.IsAny<ClienteEntity>()))
                .Returns(Task.FromResult(new ClienteEntity { Id = Guid.NewGuid() }));

            var service = new ServiceCollection();

            service.AddTransient(_ => clienteGetRepoMock.Object);
            service.AddTransient(_ => clienteInsertRepoMock.Object);

            service.ConfigurePersonasService(new DbSettings());
            var provider = service.BuildServiceProvider();
            var clienteService = provider.GetRequiredService<IClienteService>();
            var result = await clienteService.Insert(new ClienteRequestDto
            {
                Nombre = "fake",
                TipoPersona = (global::Evaluacion.Aplicacion.Dto.Especificas.Personas.TipoPersona)TipoPersona.Natural,
                FechaNacimiento = DateTimeOffset.Now,
                FechaRegistro = DateTimeOffset.Now,
                TipoDocumentoId = Guid.Parse("581E3E67-82E2-4F1F-B379-9BD870DB669E")
            }).ConfigureAwait(false);

            Assert.NotNull(result.ToString());
            Assert.NotEqual(default, result);
        }
        //TODO: Cliente, Una persona no puede tener el tipo de documento nit 
        [Fact]
        [UnitTest]
        public async Task Cliente_Validar_NIT_Fail()
        {
            var clienteGetRepoMock = new Mock<IClienteRepositorio>();
            var clienteInsertRepoMock = new Mock<IClienteRepositorio>();

            clienteGetRepoMock
                .Setup(m => m.SearchMatching(It.IsAny<Expression<Func<ClienteEntity, bool>>>()));
            clienteInsertRepoMock
                .Setup(m => m.Insert(It.IsAny<ClienteEntity>()))
                .Returns(Task.FromResult(new ClienteEntity { Id = Guid.NewGuid() }));

            var service = new ServiceCollection();

            service.AddTransient(_ => clienteGetRepoMock.Object);
            service.AddTransient(_ => clienteInsertRepoMock.Object);

            service.ConfigurePersonasService(new DbSettings());
            var provider = service.BuildServiceProvider();
            var clienteService = provider.GetRequiredService<IClienteService>();

            await Assert.ThrowsAsync<ClienteTipoDocumentoException>(() => clienteService.Insert(new ClienteRequestDto
            {
                Nombre = "FakePrueba",
                TipoPersona = (global::Evaluacion.Aplicacion.Dto.Especificas.Personas.TipoPersona)TipoPersona.Natural,
                FechaNacimiento = default,
                FechaRegistro = DateTimeOffset.Now,
                TipoDocumentoId = Guid.Parse("A89DAA40-149F-439A-8A08-7842E09D7376")
            })).ConfigureAwait(false);
        }
        [Fact]
        [UnitTest]
        public async Task Cliente_Validar_NIT_Full()
        {
            var clienteGetRepoMock = new Mock<IClienteRepositorio>();
            var clienteInsertRepoMock = new Mock<IClienteRepositorio>();

            clienteGetRepoMock
                .Setup(m => m.SearchMatching(It.IsAny<Expression<Func<ClienteEntity, bool>>>()));
            clienteInsertRepoMock
                .Setup(m => m.Insert(It.IsAny<ClienteEntity>()))
                .Returns(Task.FromResult(new ClienteEntity { Id = Guid.NewGuid() }));

            var service = new ServiceCollection();

            service.AddTransient(_ => clienteGetRepoMock.Object);
            service.AddTransient(_ => clienteInsertRepoMock.Object);

            service.ConfigurePersonasService(new DbSettings());
            var provider = service.BuildServiceProvider();
            var clienteService = provider.GetRequiredService<IClienteService>();

            await Assert.ThrowsAsync<ClienteFechaNacimientoException>(() => clienteService.Insert(new ClienteRequestDto
            {
                Nombre = "FakePrueba",
                TipoPersona = (global::Evaluacion.Aplicacion.Dto.Especificas.Personas.TipoPersona)TipoPersona.Natural,
                FechaNacimiento = default,
                FechaRegistro = DateTimeOffset.Now,
                TipoDocumentoId = Guid.Parse("581E3E67-82E2-4F1F-B379-9BD870DB669E")
            })).ConfigureAwait(false);
        }
    }
}
