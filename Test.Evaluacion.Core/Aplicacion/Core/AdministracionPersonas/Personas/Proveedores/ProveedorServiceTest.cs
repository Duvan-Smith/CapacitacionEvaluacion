using Evaluacion.Aplicacion.Core.AdministracionPersonas.Personas.Configuration;
using Evaluacion.Aplicacion.Core.AdministracionPersonas.Personas.Proveedores.Excepciones;
using Evaluacion.Aplicacion.Core.AdministracionPersonas.Personas.Proveedores.Services;
using Evaluacion.Aplicacion.Dto.Especificas.Proveedores;
using Evaluacion.Dominio.Core.Especificas.Proveedores;
using Evaluacion.Infraestructura.Datos.Persistencia.Core.Base.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;
using Xunit.Categories;

namespace Test.Evaluacion.Core.Aplicacion.Core.AdministracionPersonas.Personas.Proveedores
{
    public enum TipoPersona
    {
        Natural = 1,
        Juridico = 2,
    }
    public class ProveedorServiceTest
    {
        [Fact]
        [UnitTest]
        public async Task Check_AllParameterNull_Proveedor_Exception()
        {
            var service = new ServiceCollection();

            service.ConfigurePersonasService(new DbSettings());
            var provider = service.BuildServiceProvider();
            var proveedorService = provider.GetRequiredService<IProveedorService>();

            await Assert.ThrowsAsync<ProveedorRequestDtoNullException>(() => proveedorService.Update(null)).ConfigureAwait(false);
            await Assert.ThrowsAsync<ProveedorRequestDtoNullException>(() => proveedorService.Delete(null)).ConfigureAwait(false);
            await Assert.ThrowsAsync<ProveedorRequestDtoNullException>(() => proveedorService.Get(null)).ConfigureAwait(false);
            await Assert.ThrowsAsync<ProveedorRequestDtoNullException>(() => proveedorService.Insert(null)).ConfigureAwait(false);
        }
        //TODO: Proveedor, No puede haber dos personas con el mismo nombre / razón social
        [Fact]
        [UnitTest]
        public async Task No_Se_Repite_Nombre_Proveedor_Fail()
        {
            var proveedorRepoMock = new Mock<IProveedorRepositorio>();

            proveedorRepoMock
                .Setup(m => m.SearchMatching(It.IsAny<Expression<Func<ProveedorEntity, bool>>>()))
                .Returns(new List<ProveedorEntity> { new ProveedorEntity
                {
                    Id = Guid.NewGuid(),
                    Nombre = "FakePrueba"
                }});

            var service = new ServiceCollection();

            service.AddTransient(_ => proveedorRepoMock.Object);

            service.ConfigurePersonasService(new DbSettings());

            var provider = service.BuildServiceProvider();
            var proveedorService = provider.GetRequiredService<IProveedorService>();

            var clienteDto = new ProveedorRequestDto
            {
                Nombre = "fake",
                TipoPersona = (global::Evaluacion.Aplicacion.Dto.Especificas.Personas.TipoPersona)TipoPersona.Natural,
                FechaNacimiento = DateTimeOffset.Now,
                FechaRegistro = DateTimeOffset.Now,
                TipoDocumentoId = Guid.Parse("581E3E67-82E2-4F1F-B379-9BD870DB669E")
            };

            await Assert.ThrowsAsync<ProveedornameAlreadyExistException>(() => proveedorService.Insert(clienteDto)).ConfigureAwait(false);
        }
        [Fact]
        [UnitTest]
        public async Task No_Se_Repite_Nombre_Proveedor_Full()
        {
            var clienteGetRepoMock = new Mock<IProveedorRepositorio>();
            var clienteInsertRepoMock = new Mock<IProveedorRepositorio>();

            clienteGetRepoMock
                .Setup(m => m.SearchMatching(It.IsAny<Expression<Func<ProveedorEntity, bool>>>()));
            clienteInsertRepoMock
                .Setup(m => m.Insert(It.IsAny<ProveedorEntity>()))
                .Returns(Task.FromResult(new ProveedorEntity { Id = Guid.NewGuid() }));

            var service = new ServiceCollection();

            service.AddTransient(_ => clienteGetRepoMock.Object);
            service.AddTransient(_ => clienteInsertRepoMock.Object);

            service.ConfigurePersonasService(new DbSettings());
            var provider = service.BuildServiceProvider();
            var proveedorService = provider.GetRequiredService<IProveedorService>();
            var result = await proveedorService.Insert(new ProveedorRequestDto
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
        //TODO: Proveedor, No puede haber dos personas con el mismo numero y tipo de identificación
        #region Test Funcional
        //Se debe comentar el SearchMatching de nombre para que funcione 
        //[Fact]
        //[UnitTest]
        //public async Task No_Se_Repite_Id_Cliente_Fail()
        //{
        //    var proveedorRepoMock = new Mock<IProveedorRepositorio>();

        //    proveedorRepoMock
        //        .Setup(m => m.SearchMatching(It.IsAny<Expression<Func<ProveedorEntity, bool>>>()))
        //        .Returns(new List<ProveedorEntity> { new ProveedorEntity
        //        {
        //            Id = Guid.NewGuid(),
        //            Nombre = "FakePrueba"
        //        }});

        //    var service = new ServiceCollection();

        //    service.AddTransient(_ => proveedorRepoMock.Object);

        //    service.ConfigurePersonasService(new DbSettings());

        //    var provider = service.BuildServiceProvider();
        //    var proveedorService = provider.GetRequiredService<IProveedorService>();

        //    var clienteDto = new ProveedorRequestDto
        //    {
        //        Nombre = "FakePrueba"
        //    };

        //    await Assert.ThrowsAsync<ProveedoridAlreadyExistException>(() => proveedorService.Insert(clienteDto)).ConfigureAwait(false);
        //}
        #endregion
        [Fact]
        [UnitTest]
        public async Task No_Se_Repite_Id_Proveedor_Full()
        {
            var clienteGetRepoMock = new Mock<IProveedorRepositorio>();
            var clienteInsertRepoMock = new Mock<IProveedorRepositorio>();

            clienteGetRepoMock
                .Setup(m => m.SearchMatching(It.IsAny<Expression<Func<ProveedorEntity, bool>>>()));
            clienteInsertRepoMock
                .Setup(m => m.Insert(It.IsAny<ProveedorEntity>()))
                .Returns(Task.FromResult(new ProveedorEntity { Id = Guid.NewGuid() }));

            var service = new ServiceCollection();

            service.AddTransient(_ => clienteGetRepoMock.Object);
            service.AddTransient(_ => clienteInsertRepoMock.Object);

            service.ConfigurePersonasService(new DbSettings());
            var provider = service.BuildServiceProvider();
            var proveedorService = provider.GetRequiredService<IProveedorService>();
            var result = await proveedorService.Insert(new ProveedorRequestDto
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
        //TODO: Proveedor, La fecha de nacimiento / creación es obligatoria
        [Fact]
        [UnitTest]
        public async Task Proveedor_Validar_Fechas_Fail()
        {
            var clienteGetRepoMock = new Mock<IProveedorRepositorio>();
            var clienteInsertRepoMock = new Mock<IProveedorRepositorio>();

            clienteGetRepoMock
                .Setup(m => m.SearchMatching(It.IsAny<Expression<Func<ProveedorEntity, bool>>>()));
            clienteInsertRepoMock
                .Setup(m => m.Insert(It.IsAny<ProveedorEntity>()))
                .Returns(Task.FromResult(new ProveedorEntity { Id = Guid.NewGuid() }));

            var service = new ServiceCollection();

            service.AddTransient(_ => clienteGetRepoMock.Object);
            service.AddTransient(_ => clienteInsertRepoMock.Object);

            service.ConfigurePersonasService(new DbSettings());
            var provider = service.BuildServiceProvider();
            var proveedorService = provider.GetRequiredService<IProveedorService>();

            await Assert.ThrowsAsync<ProveedorFechaNacimientoException>(() => proveedorService.Insert(new ProveedorRequestDto
            {
                Nombre = "FakePrueba",
                TipoPersona = (global::Evaluacion.Aplicacion.Dto.Especificas.Personas.TipoPersona)TipoPersona.Natural,
                FechaNacimiento = default,
                FechaRegistro = DateTimeOffset.Now,
                TipoDocumentoId = Guid.Parse("581E3E67-82E2-4F1F-B379-9BD870DB669E")
            })).ConfigureAwait(false);
            await Assert.ThrowsAsync<ProveedorFechaRegistroException>(() => proveedorService.Insert(new ProveedorRequestDto
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
        public async Task Proveedor_Validar_Fechas_Full()
        {
            var clienteGetRepoMock = new Mock<IProveedorRepositorio>();
            var clienteInsertRepoMock = new Mock<IProveedorRepositorio>();

            clienteGetRepoMock
                .Setup(m => m.SearchMatching(It.IsAny<Expression<Func<ProveedorEntity, bool>>>()));
            clienteInsertRepoMock
                .Setup(m => m.Insert(It.IsAny<ProveedorEntity>()))
                .Returns(Task.FromResult(new ProveedorEntity { Id = Guid.NewGuid() }));

            var service = new ServiceCollection();

            service.AddTransient(_ => clienteGetRepoMock.Object);
            service.AddTransient(_ => clienteInsertRepoMock.Object);

            service.ConfigurePersonasService(new DbSettings());
            var provider = service.BuildServiceProvider();
            var proveedorService = provider.GetRequiredService<IProveedorService>();
            var result = await proveedorService.Insert(new ProveedorRequestDto
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
        //TODO: Proveedor, Test de integracion para el cliente
        [Fact]
        [IntegrationTest]
        public async void Validacion_Parametros_Proveedor_Integration()
        {
            var serviceP = new ServiceCollection();

            serviceP.ConfigurePersonasService(new DbSettings
            {
                ConnectionString = "Data Source=DESKTOP-NE15I70\\BDDUVAN;Initial Catalog=evaluacion;User ID=sa;Password=3147073260"
            });

            var providerP = serviceP.BuildServiceProvider();

            var proveedorService = providerP.GetRequiredService<IProveedorService>();

            var dtoProveedor = new ProveedorRequestDto
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
            var response = await proveedorService.Insert(dtoProveedor).ConfigureAwait(false);
            Assert.NotNull(response.ToString());
            Assert.NotEqual(default, response);

            await Assert.ThrowsAsync<ProveedornameAlreadyExistException>(() => proveedorService.Insert(dtoProveedor)).ConfigureAwait(false);

            dtoProveedor.Nombre = "Fake2";
            await Assert.ThrowsAsync<ProveedorCodigoTipoDocumentoException>(() => proveedorService.Insert(dtoProveedor)).ConfigureAwait(false);

            dtoProveedor.CodigoTipoDocumento = "345678912";
            dtoProveedor.FechaNacimiento = default;
            await Assert.ThrowsAsync<ProveedorFechaNacimientoException>(() => proveedorService.Insert(dtoProveedor)).ConfigureAwait(false);

            dtoProveedor.FechaNacimiento = DateTimeOffset.Now;
            dtoProveedor.FechaRegistro = default;
            await Assert.ThrowsAsync<ProveedorFechaRegistroException>(() => proveedorService.Insert(dtoProveedor)).ConfigureAwait(false);

            _ = proveedorService.Delete(dtoProveedor);
        }
        [Fact]
        [IntegrationTest]
        public async void Validar_Identificacion_Tipo_Proveedor_Integration()
        {
            var serviceP = new ServiceCollection();

            serviceP.ConfigurePersonasService(new DbSettings
            {
                ConnectionString = "Data Source=DESKTOP-NE15I70\\BDDUVAN;Initial Catalog=evaluacion;User ID=sa;Password=3147073260"
            });

            var providerP = serviceP.BuildServiceProvider();

            var proveedorService = providerP.GetRequiredService<IProveedorService>();

            var dtoProveedor = new ProveedorRequestDto
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
            var response = await proveedorService.Insert(dtoProveedor).ConfigureAwait(false);
            Assert.NotNull(response.ToString());
            Assert.NotEqual(default, response);

            var dtoProveedorI2 = new ProveedorRequestDto
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

            var responseI2 = await proveedorService.Insert(dtoProveedorI2).ConfigureAwait(false);
            Assert.NotNull(responseI2.ToString());
            Assert.NotEqual(default, responseI2);

            _ = proveedorService.Delete(dtoProveedor);
            _ = proveedorService.Delete(dtoProveedorI2);
        }
    }
}
