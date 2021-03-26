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
                //TODO: Debe poderse distinguir entre jurídicas y naturales
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
        //TODO: Cliente, Test de integracion para el cliente
        [Fact]
        [IntegrationTest]
        public async void Validacion_Parametros_Cliente_Integration()
        {
            var serviceP = new ServiceCollection();

            serviceP.ConfigurePersonasService(new DbSettings
            {
                ConnectionString = "Data Source=DESKTOP-NE15I70\\BDDUVAN;Initial Catalog=evaluacion;User ID=sa;Password=3147073260"
            });

            var providerP = serviceP.BuildServiceProvider();

            var clienteService = providerP.GetRequiredService<IClienteService>();

            var dtoCliente = new ClienteRequestDto
            {
                Id = Guid.NewGuid(),
                Nombre = "fake",
                Apellido = "fake",
                NumeroTelefono = 123456789,
                CorreoElectronico = "fake@fake.fake",
                CodigoTipoDocumento = "123456789",
                TipoPersona = (global::Evaluacion.Aplicacion.Dto.Especificas.Personas.TipoPersona)TipoPersona.Juridico,
                FechaNacimiento = DateTimeOffset.Now,
                FechaRegistro = DateTimeOffset.Now,
                TipoDocumentoId = Guid.Parse("581E3E67-82E2-4F1F-B379-9BD870DB669E"),
            };
            var response = await clienteService.Insert(dtoCliente).ConfigureAwait(false);
            Assert.NotNull(response.ToString());
            Assert.NotEqual(default, response);

            await Assert.ThrowsAsync<ClientenameAlreadyExistException>(() => clienteService.Insert(dtoCliente)).ConfigureAwait(false);

            dtoCliente.Nombre = "Fake2";
            await Assert.ThrowsAsync<ClienteCodigoTipoDocumentoException>(() => clienteService.Insert(dtoCliente)).ConfigureAwait(false);

            dtoCliente.CodigoTipoDocumento = "345678912";
            dtoCliente.FechaNacimiento = default;
            await Assert.ThrowsAsync<ClienteFechaNacimientoException>(() => clienteService.Insert(dtoCliente)).ConfigureAwait(false);

            dtoCliente.FechaNacimiento = DateTimeOffset.Now;
            dtoCliente.FechaRegistro = default;
            await Assert.ThrowsAsync<ClienteFechaRegistroException>(() => clienteService.Insert(dtoCliente)).ConfigureAwait(false);

            dtoCliente.FechaRegistro = DateTimeOffset.Now;
            dtoCliente.TipoDocumentoId = Guid.Parse("A89DAA40-149F-439A-8A08-7842E09D7376");
            await Assert.ThrowsAsync<ClienteTipoDocumentoException>(() => clienteService.Insert(dtoCliente)).ConfigureAwait(false);

            _ = clienteService.Delete(dtoCliente);
        }
        [Fact]
        [IntegrationTest]
        public async void Validar_Identificacion_Tipo_Cliente_Integration()
        {
            var serviceP = new ServiceCollection();

            serviceP.ConfigurePersonasService(new DbSettings
            {
                ConnectionString = "Data Source=DESKTOP-NE15I70\\BDDUVAN;Initial Catalog=evaluacion;User ID=sa;Password=3147073260"
            });

            var providerP = serviceP.BuildServiceProvider();

            var clienteService = providerP.GetRequiredService<IClienteService>();

            var dtoCliente = new ClienteRequestDto
            {
                Id = Guid.NewGuid(),
                Nombre = "fake",
                Apellido = "fake",
                NumeroTelefono = 123456789,
                CorreoElectronico = "fake@fake.fake",
                CodigoTipoDocumento = "123456789",
                TipoPersona = (global::Evaluacion.Aplicacion.Dto.Especificas.Personas.TipoPersona)TipoPersona.Natural,
                FechaNacimiento = DateTimeOffset.Now,
                FechaRegistro = DateTimeOffset.Now,
                TipoDocumentoId = Guid.Parse("581E3E67-82E2-4F1F-B379-9BD870DB669E"),
            };
            var response = await clienteService.Insert(dtoCliente).ConfigureAwait(false);
            Assert.NotNull(response.ToString());
            Assert.NotEqual(default, response);

            var dtoClienteI2 = new ClienteRequestDto
            {
                Id = Guid.NewGuid(),
                Nombre = "fake2",
                Apellido = "fake2",
                NumeroTelefono = 223456789,
                CorreoElectronico = "fake2@fake2.fake2",
                CodigoTipoDocumento = "123456789",
                TipoPersona = (global::Evaluacion.Aplicacion.Dto.Especificas.Personas.TipoPersona)TipoPersona.Juridico,
                FechaNacimiento = DateTimeOffset.Now,
                FechaRegistro = DateTimeOffset.Now,
                TipoDocumentoId = Guid.Parse("12427378-28E4-48CB-8ED7-097116F8064E"),
            };

            var responseI2 = await clienteService.Insert(dtoClienteI2).ConfigureAwait(false);
            Assert.NotNull(responseI2.ToString());
            Assert.NotEqual(default, responseI2);

            _ = clienteService.Delete(dtoCliente);
            _ = clienteService.Delete(dtoClienteI2);
        }
    }
}
