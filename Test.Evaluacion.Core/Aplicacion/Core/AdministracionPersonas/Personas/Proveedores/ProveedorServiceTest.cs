using AutoMapper;
using Evaluacion.Aplicacion.Core.AdministracionPersonas.Genericas.Configuration;
using Evaluacion.Aplicacion.Core.AdministracionPersonas.Genericas.TipoDocumentos.Services;
using Evaluacion.Aplicacion.Core.AdministracionPersonas.Personas.Configuration;
using Evaluacion.Aplicacion.Core.AdministracionPersonas.Personas.Proveedores.Excepciones;
using Evaluacion.Aplicacion.Core.AdministracionPersonas.Personas.Proveedores.Services;
using Evaluacion.Aplicacion.Dto.Especificas.Proveedores;
using Evaluacion.Aplicacion.Dto.Genericas.TipoDocumentos;
using Evaluacion.Dominio.Core.Especificas.Proveedores;
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

namespace Test.Evaluacion.Core.Aplicacion.Core.AdministracionPersonas.Personas.Proveedores
{
    public enum TipoPersona
    {
        Natural = 1,
        Juridico = 2,
    }
    public class ProveedorServiceTest
    {
        private static ServiceProvider ServiceCollectionProveedor()
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
        public async Task Check_AllParameterNull_Proveedor_Exception()
        {
            var service = new ServiceCollection();

            service.ConfigurePersonasService(new DbSettings());
            var provider = service.BuildServiceProvider();
            var proveedorService = provider.GetRequiredService<IProveedorService>();

            _ = await Assert.ThrowsAsync<ProveedorRequestDtoNullException>(() => proveedorService.Update(null)).ConfigureAwait(false);
            _ = await Assert.ThrowsAsync<ProveedorRequestDtoNullException>(() => proveedorService.Delete(null)).ConfigureAwait(false);
            _ = await Assert.ThrowsAsync<ProveedorRequestDtoNullException>(() => proveedorService.Get(null)).ConfigureAwait(false);
            _ = await Assert.ThrowsAsync<ProveedorRequestDtoNullException>(() => proveedorService.Insert(null)).ConfigureAwait(false);
        }
        //Proveedor, Debe poderse distinguir entre jurídicas y naturales
        #region Validar_TipoPersona_Proveedor
        [Fact]
        [UnitTest]
        public async Task Validar_TipoPersona_Proveedor_Fail()
        {
            var proveedorRepoMock = new Mock<IProveedorRepositorio>();
            var Listentity = new List<ProveedorEntity>
            {
                new ProveedorEntity
                {
                    Id = Guid.NewGuid(),
                    Nombre = "Nombre",
                    CodigoTipoDocumento="123456789",
                    TipoDocumentoId=Guid.Parse("581E3E67-82E2-4F1F-B379-9BD870DB669E"),
                    TipoPersona = (global::Evaluacion.Dominio.Core.Especificas.Personas.TipoPersona)TipoPersona.Natural,
                }
            };
            _ = proveedorRepoMock
                .Setup(m => m.GetAll<ProveedorEntity>())
                .Returns(Listentity);

            var service = new ServiceCollection();

            service.AddTransient(_ => proveedorRepoMock.Object);

            service.ConfigurePersonasService(new DbSettings());

            var provider = service.BuildServiceProvider();
            var proveedorService = provider.GetRequiredService<IProveedorService>();

            var dtoProveedor = new ProveedorRequestDto
            {
                Nombre = "fake",
                FechaNacimiento = DateTimeOffset.Now,
                FechaRegistro = DateTimeOffset.Now,
                TipoDocumentoId = Guid.Parse("581E3E67-82E2-4F1F-B379-9BD870DB669E")
            };
            _ = await Assert.ThrowsAsync<ProveedorTipoPersonaNullException>(() => proveedorService.Insert(dtoProveedor)).ConfigureAwait(false);

            dtoProveedor.TipoPersona = 0;
            _ = await Assert.ThrowsAsync<ProveedorTipoPersonaNullException>(() => proveedorService.Insert(dtoProveedor)).ConfigureAwait(false);
        }
        [Fact]
        [UnitTest]
        public async Task Validar_TipoPersona_Proveedor_Full()
        {
            var proveedorRepoMock = new Mock<IProveedorRepositorio>();
            _ = proveedorRepoMock
                .Setup(m => m.GetAll<ProveedorEntity>());
            _ = proveedorRepoMock
                .Setup(m => m.Insert(It.IsAny<ProveedorEntity>()))
                .Returns(Task.FromResult(new ProveedorEntity { Id = Guid.NewGuid() }));

            var service = new ServiceCollection();

            service.AddTransient(_ => proveedorRepoMock.Object);

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
        [Fact]
        [IntegrationTest]
        public async void Validar_TipoPersona_Proveedor_IntegrationTest()
        {
            ServiceProvider provider = ServiceCollectionProveedor();

            var proveedorService = provider.GetRequiredService<IProveedorService>();
            var proveedorRepositorio = provider.GetRequiredService<IProveedorRepositorio>();
            var documentoService = provider.GetRequiredService<ITipoDocumentoService>();
            var documentoRepo = provider.GetRequiredService<ITipoDocumentoRepositorio>();

            var dtoDocumento = new TipoDocumentoRequestDto
            {
                Id = Guid.NewGuid(),
                NombreTipoDocumento = "fakeDocumentofakeProveedor10",
            };
            var documento = documentoRepo
                .SearchMatching<TipoDocumentoEntity>(x => x.NombreTipoDocumento == dtoDocumento.NombreTipoDocumento || x.Id == dtoDocumento.Id)
                .FirstOrDefault();
            if (documento != null || documento != default)
                documentoRepo.Delete(documento);

            _ = await documentoService.Insert(dtoDocumento).ConfigureAwait(false);

            var dtoProveedor = new ProveedorRequestDto
            {
                Id = Guid.NewGuid(),
                Nombre = "fake",
                Apellido = "fake",
                NumeroTelefono = 123456789,
                CorreoElectronico = "fake@fake.fake",
                CodigoTipoDocumento = "000000007",
                FechaNacimiento = DateTimeOffset.Now,
                FechaRegistro = DateTimeOffset.Now,
                TipoDocumentoId = dtoDocumento.Id,
            };

            var proveedor = proveedorRepositorio
                .SearchMatching<ProveedorEntity>(x => x.Nombre == dtoProveedor.Nombre || x.Id == dtoProveedor.Id)
                .FirstOrDefault();
            if (proveedor != null || proveedor != default)
                proveedorRepositorio.Delete(proveedor);

            _ = await Assert.ThrowsAsync<ProveedorTipoPersonaNullException>(() => proveedorService.Insert(dtoProveedor)).ConfigureAwait(false);

            dtoProveedor.TipoPersona = 0;
            //await Assert.ThrowsAsync<ProveedorTipoPersonaNullException>(() => proveedorService.Insert(dtoProveedor)).ConfigureAwait(false);

            dtoProveedor.TipoPersona = (global::Evaluacion.Aplicacion.Dto.Especificas.Personas.TipoPersona)TipoPersona.Juridico;
            var response = await proveedorService.Insert(dtoProveedor).ConfigureAwait(false);
            Assert.NotNull(response.ToString());
            Assert.NotEqual(default, response);
            var id = dtoProveedor.Id;
            dtoProveedor.Id = Guid.NewGuid();
            _ = await Assert.ThrowsAsync<ProveedornameAlreadyExistException>(() => proveedorService.Insert(dtoProveedor)).ConfigureAwait(false);
            dtoProveedor.Id = id;
            _ = await proveedorService.Delete(dtoProveedor).ConfigureAwait(false);
            _ = await documentoService.Delete(dtoDocumento).ConfigureAwait(false);
        }
        #endregion
        //Proveedor, No puede haber dos personas con el mismo numero y tipo de identificación
        #region No_Se_Repite_CodigoTipoDocumento_Proveedor
        [Fact]
        [UnitTest]
        public async Task No_Se_Repite_CodigoTipoDocumento_Cliente_Fail()
        {
            var proveedorRepoMock = new Mock<IProveedorRepositorio>();
            var Listentity = new List<ProveedorEntity>
            {
                new ProveedorEntity
                {
                    Id = Guid.NewGuid(),
                    Nombre = "Nombre",
                    CodigoTipoDocumento="123456789",
                    TipoDocumentoId=Guid.Parse("581E3E67-82E2-4F1F-B379-9BD870DB669E"),
                    TipoPersona = (global::Evaluacion.Dominio.Core.Especificas.Personas.TipoPersona)TipoPersona.Natural,
                }
            };
            _ = proveedorRepoMock
                .Setup(m => m.GetAll<ProveedorEntity>())
                .Returns(Listentity);

            var service = new ServiceCollection();

            service.AddTransient(_ => proveedorRepoMock.Object);

            service.ConfigurePersonasService(new DbSettings());

            var provider = service.BuildServiceProvider();
            var proveedorService = provider.GetRequiredService<IProveedorService>();

            var clienteDto = new ProveedorRequestDto
            {
                Nombre = "Nombre2",
                TipoPersona = (global::Evaluacion.Aplicacion.Dto.Especificas.Personas.TipoPersona)TipoPersona.Natural,
                FechaNacimiento = DateTimeOffset.Now,
                FechaRegistro = DateTimeOffset.Now,
                CodigoTipoDocumento = "123456789",
                TipoDocumentoId = Guid.Parse("581E3E67-82E2-4F1F-B379-9BD870DB669E")
            };

            var result = await Assert.ThrowsAsync<ProveedorCodigoTipoDocumentoException>(() => proveedorService.Insert(clienteDto)).ConfigureAwait(false);
            Assert.Equal(clienteDto.CodigoTipoDocumento, result.Message);
        }
        [Fact]
        [UnitTest]
        public async Task No_Se_Repite_CodigoTipoDocumento_Proveedor_Full()
        {
            var proveedorRepoMock = new Mock<IProveedorRepositorio>();

            _ = proveedorRepoMock
                .Setup(m => m.GetAll<ProveedorEntity>());
            _ = proveedorRepoMock
                .Setup(m => m.Insert(It.IsAny<ProveedorEntity>()))
                .Returns(Task.FromResult(new ProveedorEntity { Id = Guid.NewGuid() }));

            var service = new ServiceCollection();

            service.AddTransient(_ => proveedorRepoMock.Object);

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
        [Fact]
        [IntegrationTest]
        public async void No_Se_Repite_CodigoTipoDocumento_Proveedor_IntegrationTest()
        {
            ServiceProvider provider = ServiceCollectionProveedor();

            var proveedorService = provider.GetRequiredService<IProveedorService>();
            var proveedorRepositorio = provider.GetRequiredService<IProveedorRepositorio>();
            var documentoService = provider.GetRequiredService<ITipoDocumentoService>();
            var documentoRepo = provider.GetRequiredService<ITipoDocumentoRepositorio>();

            var dtoDocumento = new TipoDocumentoRequestDto
            {
                Id = Guid.NewGuid(),
                NombreTipoDocumento = "fakeDocumentofakeProveedor01",
            };
            var documento = documentoRepo
                .SearchMatching<TipoDocumentoEntity>(x => x.NombreTipoDocumento == dtoDocumento.NombreTipoDocumento || x.Id == dtoDocumento.Id)
                .FirstOrDefault();
            if (documento != null || documento != default)
                documentoRepo.Delete(documento);

            _ = await documentoService.Insert(dtoDocumento).ConfigureAwait(false);

            var dtoDocumento2 = new TipoDocumentoRequestDto
            {
                Id = Guid.NewGuid(),
                NombreTipoDocumento = "fakeDocumentofakeProveedor02",
            };
            var documento2 = documentoRepo
                .SearchMatching<TipoDocumentoEntity>(x => x.NombreTipoDocumento == dtoDocumento2.NombreTipoDocumento || x.Id == dtoDocumento2.Id)
                .FirstOrDefault();
            if (documento2 != null || documento2 != default)
                documentoRepo.Delete(documento2);

            _ = await documentoService.Insert(dtoDocumento2).ConfigureAwait(false);

            var dtoProveedor = new ProveedorRequestDto
            {
                Id = Guid.NewGuid(),
                Nombre = "fake_proveerdor_1",
                Apellido = "fake_proveerdor_1",
                NumeroTelefono = 123456789,
                CorreoElectronico = "fake@fake.fake",
                CodigoTipoDocumento = "000000006",
                TipoPersona = (global::Evaluacion.Aplicacion.Dto.Especificas.Personas.TipoPersona)TipoPersona.Natural,
                FechaNacimiento = DateTimeOffset.Now,
                FechaRegistro = DateTimeOffset.Now,
                TipoDocumentoId = dtoDocumento.Id,
            };

            var proveedor = proveedorRepositorio
                .SearchMatching<ProveedorEntity>(x => x.Nombre == dtoProveedor.Nombre || x.Id == dtoProveedor.Id)
                .FirstOrDefault();
            if (proveedor != null || proveedor != default)
                proveedorRepositorio.Delete(proveedor);

            var response = await proveedorService.Insert(dtoProveedor).ConfigureAwait(false);
            Assert.NotNull(response.ToString());
            Assert.NotEqual(default, response);

            var id = dtoProveedor.Id;
            dtoProveedor.Nombre = "fake_proveedor_Throws";
            dtoProveedor.Id = Guid.NewGuid();
            _ = await Assert.ThrowsAsync<ProveedorCodigoTipoDocumentoException>(() => proveedorService.Insert(dtoProveedor)).ConfigureAwait(false);
            dtoProveedor.Id = id;

            var dtoProveedorI2 = new ProveedorRequestDto
            {
                Id = Guid.NewGuid(),
                Nombre = "fake_proveerdor_2",
                Apellido = "fake_proveerdor_2",
                NumeroTelefono = 223456789,
                CorreoElectronico = "fake2@fake2.fake2",
                CodigoTipoDocumento = "123456789",
                TipoPersona = (global::Evaluacion.Aplicacion.Dto.Especificas.Personas.TipoPersona)TipoPersona.Juridico,
                FechaNacimiento = DateTimeOffset.Now,
                FechaRegistro = DateTimeOffset.Now,
                TipoDocumentoId = dtoDocumento2.Id,
            };

            var proveedor2 = proveedorRepositorio
                .SearchMatching<ProveedorEntity>(x => x.Nombre == dtoProveedorI2.Nombre || x.Id == dtoProveedorI2.Id)
                .FirstOrDefault();
            if (proveedor2 != null || proveedor2 != default)
                proveedorRepositorio.Delete(proveedor2);

            var responseI2 = await proveedorService.Insert(dtoProveedorI2).ConfigureAwait(false);
            Assert.NotNull(responseI2.ToString());
            Assert.NotEqual(default, responseI2);

            _ = await proveedorService.Delete(dtoProveedor).ConfigureAwait(false);
            _ = await proveedorService.Delete(dtoProveedorI2).ConfigureAwait(false);
            _ = await documentoService.Delete(dtoDocumento).ConfigureAwait(false);
            _ = await documentoService.Delete(dtoDocumento2).ConfigureAwait(false);
        }
        #endregion
        //Proveedor, No puede haber dos personas con el mismo nombre / razón social
        #region No_Se_Repite_Nombre_Proveedor
        [Fact]
        [UnitTest]
        public async Task No_Se_Repite_Nombre_Proveedor_Fail()
        {
            var proveedorRepoMock = new Mock<IProveedorRepositorio>();
            var Listentity = new List<ProveedorEntity>
            {
                new ProveedorEntity
                {
                    Id = Guid.NewGuid(),
                    Nombre = "Nombre",
                    CodigoTipoDocumento="123456789",
                    TipoDocumentoId=Guid.Parse("581E3E67-82E2-4F1F-B379-9BD870DB669E"),
                    TipoPersona = (global::Evaluacion.Dominio.Core.Especificas.Personas.TipoPersona)TipoPersona.Natural,
                }
            };
            _ = proveedorRepoMock
                .Setup(m => m.GetAll<ProveedorEntity>())
                .Returns(Listentity);

            var service = new ServiceCollection();

            service.AddTransient(_ => proveedorRepoMock.Object);

            service.ConfigurePersonasService(new DbSettings());

            var provider = service.BuildServiceProvider();
            var proveedorService = provider.GetRequiredService<IProveedorService>();

            var clienteDto = new ProveedorRequestDto
            {
                Nombre = "Nombre",
                TipoPersona = (global::Evaluacion.Aplicacion.Dto.Especificas.Personas.TipoPersona)TipoPersona.Natural,
                FechaNacimiento = DateTimeOffset.Now,
                FechaRegistro = DateTimeOffset.Now,
                TipoDocumentoId = Guid.Parse("581E3E67-82E2-4F1F-B379-9BD870DB669E")
            };

            var result = await Assert.ThrowsAsync<ProveedornameAlreadyExistException>(() => proveedorService.Insert(clienteDto)).ConfigureAwait(false);
            Assert.Equal(clienteDto.Nombre, result.Message);
        }
        [Fact]
        [UnitTest]
        public async Task No_Se_Repite_Nombre_Proveedor_Full()
        {
            var proveedorRepoMock = new Mock<IProveedorRepositorio>();

            _ = proveedorRepoMock
                .Setup(m => m.GetAll<ProveedorEntity>());
            _ = proveedorRepoMock
                .Setup(m => m.Insert(It.IsAny<ProveedorEntity>()))
                .Returns(Task.FromResult(new ProveedorEntity { Id = Guid.NewGuid() }));

            var service = new ServiceCollection();

            service.AddTransient(_ => proveedorRepoMock.Object);

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
        [Fact]
        [IntegrationTest]
        public async void No_Se_Repite_Nombre_Proveedor_IntegrationTest()
        {
            ServiceProvider provider = ServiceCollectionProveedor();

            var proveedorService = provider.GetRequiredService<IProveedorService>();
            var proveedorRepositorio = provider.GetRequiredService<IProveedorRepositorio>();
            var documentoService = provider.GetRequiredService<ITipoDocumentoService>();
            var documentoRepo = provider.GetRequiredService<ITipoDocumentoRepositorio>();

            var dtoDocumento = new TipoDocumentoRequestDto
            {
                Id = Guid.NewGuid(),
                NombreTipoDocumento = "fakeDocumentofakeProveedor03",
            };
            var documento = documentoRepo
                .SearchMatching<TipoDocumentoEntity>(x => x.NombreTipoDocumento == dtoDocumento.NombreTipoDocumento || x.Id == dtoDocumento.Id)
                .FirstOrDefault();
            if (documento != null || documento != default)
                documentoRepo.Delete(documento);

            _ = await documentoService.Insert(dtoDocumento).ConfigureAwait(false);

            var dtoProveedor = new ProveedorRequestDto
            {
                Id = Guid.NewGuid(),
                Nombre = "fake",
                Apellido = "fake",
                NumeroTelefono = 123456789,
                CorreoElectronico = "fake@fake.fake",
                CodigoTipoDocumento = "000000009",
                TipoPersona = (global::Evaluacion.Aplicacion.Dto.Especificas.Personas.TipoPersona)TipoPersona.Juridico,
                FechaNacimiento = DateTimeOffset.Now,
                FechaRegistro = DateTimeOffset.Now,
                TipoDocumentoId = dtoDocumento.Id,
            };

            var proveedor = proveedorRepositorio
                .SearchMatching<ProveedorEntity>(x => x.Nombre == dtoProveedor.Nombre || x.Id == dtoProveedor.Id)
                .FirstOrDefault();
            if (proveedor != null || proveedor != default)
                proveedorRepositorio.Delete(proveedor);

            var response = await proveedorService.Insert(dtoProveedor).ConfigureAwait(false);
            Assert.NotNull(response.ToString());
            Assert.NotEqual(default, response);

            var id = dtoProveedor.Id;
            dtoProveedor.Id = Guid.NewGuid();
            _ = await Assert.ThrowsAsync<ProveedornameAlreadyExistException>(() => proveedorService.Insert(dtoProveedor)).ConfigureAwait(false);
            dtoProveedor.Id = id;

            _ = await proveedorService.Delete(dtoProveedor).ConfigureAwait(false);
            _ = await documentoService.Delete(dtoDocumento).ConfigureAwait(false);
        }
        #endregion
        //Proveedor, La fecha de nacimiento / creación es obligatoria
        #region Proveedor_Validar_Fechas
        [Fact]
        [UnitTest]
        public async Task Proveedor_Validar_Fechas_Fail()
        {
            var proveedorRepoMock = new Mock<IProveedorRepositorio>();
            _ = proveedorRepoMock
                .Setup(m => m.GetAll<ProveedorEntity>());
            _ = proveedorRepoMock
                .Setup(m => m.Insert(It.IsAny<ProveedorEntity>()))
                .Returns(Task.FromResult(new ProveedorEntity { Id = Guid.NewGuid() }));

            var service = new ServiceCollection();

            service.AddTransient(_ => proveedorRepoMock.Object);

            service.ConfigurePersonasService(new DbSettings());
            var provider = service.BuildServiceProvider();
            var proveedorService = provider.GetRequiredService<IProveedorService>();

            var proveedorDto = new ProveedorRequestDto
            {
                Nombre = "FakePrueba",
                TipoPersona = (global::Evaluacion.Aplicacion.Dto.Especificas.Personas.TipoPersona)TipoPersona.Natural,
                FechaNacimiento = DateTimeOffset.Now,
                FechaRegistro = DateTimeOffset.Now,
                TipoDocumentoId = Guid.Parse("581E3E67-82E2-4F1F-B379-9BD870DB669E")
            };

            proveedorDto.FechaNacimiento = default;
            _ = await Assert.ThrowsAsync<ProveedorFechaNacimientoException>(() => proveedorService.Insert(proveedorDto)).ConfigureAwait(false);

            proveedorDto.FechaNacimiento = DateTimeOffset.Now;
            proveedorDto.FechaRegistro = default;
            _ = await Assert.ThrowsAsync<ProveedorFechaRegistroException>(() => proveedorService.Insert(proveedorDto)).ConfigureAwait(false);
        }
        [Fact]
        [UnitTest]
        public async Task Proveedor_Validar_Fechas_Full()
        {
            var proveedorRepoMock = new Mock<IProveedorRepositorio>();

            _ = proveedorRepoMock
                .Setup(m => m.SearchMatching(It.IsAny<Expression<Func<ProveedorEntity, bool>>>()));
            _ = proveedorRepoMock
                .Setup(m => m.Insert(It.IsAny<ProveedorEntity>()))
                .Returns(Task.FromResult(new ProveedorEntity { Id = Guid.NewGuid() }));

            var service = new ServiceCollection();

            service.AddTransient(_ => proveedorRepoMock.Object);

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
        [Fact]
        [IntegrationTest]
        public async void Proveedor_Validar_Fechas_IntegrationTest()
        {
            ServiceProvider provider = ServiceCollectionProveedor();

            var proveedorService = provider.GetRequiredService<IProveedorService>();
            var documentoService = provider.GetRequiredService<ITipoDocumentoService>();
            var documentoRepo = provider.GetRequiredService<ITipoDocumentoRepositorio>();

            var dtoDocumento = new TipoDocumentoRequestDto
            {
                Id = Guid.NewGuid(),
                NombreTipoDocumento = "fakeDocumentofakeProveedor08",
            };
            var documento = documentoRepo
                .SearchMatching<TipoDocumentoEntity>(x => x.NombreTipoDocumento == dtoDocumento.NombreTipoDocumento || x.Id == dtoDocumento.Id)
                .FirstOrDefault();
            if (documento != null || documento != default)
                documentoRepo.Delete(documento);

            _ = await documentoService.Insert(dtoDocumento).ConfigureAwait(false);

            var dtoProveedor = new ProveedorRequestDto
            {
                Id = Guid.NewGuid(),
                Nombre = "fake",
                Apellido = "fake",
                NumeroTelefono = 123456789,
                CorreoElectronico = "fake@fake.fake",
                CodigoTipoDocumento = "000000004",
                TipoPersona = (global::Evaluacion.Aplicacion.Dto.Especificas.Personas.TipoPersona)TipoPersona.Juridico,
                FechaNacimiento = DateTimeOffset.Now,
                FechaRegistro = DateTimeOffset.Now,
                TipoDocumentoId = dtoDocumento.Id,
            };

            dtoProveedor.FechaNacimiento = default;
            _ = await Assert.ThrowsAsync<ProveedorFechaNacimientoException>(() => proveedorService.Insert(dtoProveedor)).ConfigureAwait(false);

            dtoProveedor.FechaNacimiento = DateTimeOffset.Now;
            dtoProveedor.FechaRegistro = default;
            _ = await Assert.ThrowsAsync<ProveedorFechaRegistroException>(() => proveedorService.Insert(dtoProveedor)).ConfigureAwait(false);

            dtoProveedor.FechaRegistro = DateTimeOffset.Now;
            var response = await proveedorService.Insert(dtoProveedor).ConfigureAwait(false);
            Assert.NotNull(response.ToString());
            Assert.NotEqual(default, response);

            _ = await proveedorService.Delete(dtoProveedor).ConfigureAwait(false);
            _ = await documentoService.Delete(dtoDocumento).ConfigureAwait(false);
        }
        #endregion
        //Proveedor, Test de integracion para el Proveedor
        #region Delect
        [Fact]
        [UnitTest]
        public async Task Proveedor_Delete_Test_Fail()
        {
            var proveedorRepoMock = new Mock<IProveedorRepositorio>();
            proveedorRepoMock
                .Setup(m => m.SearchMatchingOneResult(It.IsAny<Expression<Func<ProveedorEntity, bool>>>()));

            var service = new ServiceCollection();

            service.AddTransient(_ => proveedorRepoMock.Object);

            service.ConfigurePersonasService(new DbSettings());
            var provider = service.BuildServiceProvider();
            var proveedorService = provider.GetRequiredService<IProveedorService>();

            _ = await Assert.ThrowsAsync<ProveedorNoExistException>(() => proveedorService.Delete(new ProveedorRequestDto { Nombre = "Nombre", Id = Guid.NewGuid() })).ConfigureAwait(false);
        }
        [Fact]
        [UnitTest]
        public async Task Proveedor_Delete_Test_Full()
        {
            var proveedorRepoMock = new Mock<IProveedorRepositorio>();
            var entity = new ProveedorEntity
            {
                Id = Guid.NewGuid(),
                Nombre = "Nombre"
            };
            proveedorRepoMock
                .Setup(m => m.SearchMatchingOneResult(It.IsAny<Expression<Func<ProveedorEntity, bool>>>()))
                .Returns(entity);
            proveedorRepoMock
                .Setup(m => m.Delete(It.IsAny<ProveedorEntity>()))
                .Returns(Task.FromResult(true));

            var service = new ServiceCollection();

            service.AddTransient(_ => proveedorRepoMock.Object);

            service.ConfigurePersonasService(new DbSettings());
            var provider = service.BuildServiceProvider();
            var proveedorService = provider.GetRequiredService<IProveedorService>();

            var result = await proveedorService.Delete(new ProveedorRequestDto
            {
                Id = Guid.NewGuid(),
            }).ConfigureAwait(false);

            Assert.NotNull(result.ToString());
            Assert.True(result);
        }
        [Fact]
        [IntegrationTest]
        public async void Proveedor_Delete_Test_IntegrationTest()
        {
            ServiceProvider provider = ServiceCollectionProveedor();

            var proveedorService = provider.GetRequiredService<IProveedorService>();
            var proveedorRepositorio = provider.GetRequiredService<IProveedorRepositorio>();
            var documentoService = provider.GetRequiredService<ITipoDocumentoService>();
            var documentoRepo = provider.GetRequiredService<ITipoDocumentoRepositorio>();

            var dtoDocumento = new TipoDocumentoRequestDto
            {
                Id = Guid.NewGuid(),
                NombreTipoDocumento = "fakeDocumentofakeProveedor04",
            };
            var documento = documentoRepo
                .SearchMatching<TipoDocumentoEntity>(x => x.NombreTipoDocumento == dtoDocumento.NombreTipoDocumento || x.Id == dtoDocumento.Id)
                .FirstOrDefault();
            if (documento != null || documento != default)
                documentoRepo.Delete(documento);

            _ = await documentoService.Insert(dtoDocumento).ConfigureAwait(false);

            var dtoProveedor = new ProveedorRequestDto
            {
                Id = Guid.NewGuid(),
                Nombre = "fakeProveedorDeleteTestI1",
                Apellido = "fakeProveedorDeleteTestI1",
                NumeroTelefono = 123456789,
                CorreoElectronico = "fake@fake.fake",
                CodigoTipoDocumento = "000000001",
                TipoPersona = (global::Evaluacion.Aplicacion.Dto.Especificas.Personas.TipoPersona)TipoPersona.Juridico,
                FechaNacimiento = DateTimeOffset.Now,
                FechaRegistro = DateTimeOffset.Now,
                TipoDocumentoId = dtoDocumento.Id,
            };

            var proveedor = proveedorRepositorio
                .SearchMatching<ProveedorEntity>(x => x.Nombre == dtoProveedor.Nombre || x.Id == dtoProveedor.Id)
                .FirstOrDefault();
            if (proveedor != null || proveedor != default)
                proveedorRepositorio.Delete(proveedor);

            _ = await proveedorService.Insert(dtoProveedor).ConfigureAwait(false);

            var dtoProveedor2 = new ProveedorRequestDto
            {
                Id = dtoProveedor.Id
            };
            var result = await proveedorService.Delete(dtoProveedor2).ConfigureAwait(false);

            Assert.True(result);
            _ = await documentoService.Delete(dtoDocumento).ConfigureAwait(false);
        }
        #endregion
        #region Update
        [Fact]
        [UnitTest]
        public async Task Proveedor_Update_Test_Fail()
        {
            var proveedorRepoMock = new Mock<IProveedorRepositorio>();
            _ = proveedorRepoMock
                .Setup(m => m.SearchMatchingOneResult(It.IsAny<Expression<Func<ProveedorEntity, bool>>>()));

            var service = new ServiceCollection();

            service.AddTransient(_ => proveedorRepoMock.Object);

            service.ConfigurePersonasService(new DbSettings());
            var provider = service.BuildServiceProvider();
            var proveedorService = provider.GetRequiredService<IProveedorService>();

            _ = await Assert.ThrowsAsync<ProveedorNoExistException>(() => proveedorService.Update(new ProveedorRequestDto { Nombre = "Nombre", Id = Guid.NewGuid() })).ConfigureAwait(false);
        }
        [Fact]
        [UnitTest]
        public async Task Proveedor_Update_Test_Full()
        {
            var proveedorRepoMock = new Mock<IProveedorRepositorio>();
            var entity = new ProveedorEntity
            {
                Id = Guid.NewGuid(),
                Nombre = "Nombre",
                CodigoTipoDocumento = "123456789",
                TipoDocumentoId = Guid.Parse("581E3E67-82E2-4F1F-B379-9BD870DB669E"),
                TipoPersona = (global::Evaluacion.Dominio.Core.Especificas.Personas.TipoPersona)TipoPersona.Natural,
            };
            _ = proveedorRepoMock
                .Setup(m => m.SearchMatchingOneResult(It.IsAny<Expression<Func<ProveedorEntity, bool>>>()))
                .Returns(entity);
            _ = proveedorRepoMock
                .Setup(m => m.Update(It.IsAny<ProveedorEntity>()))
                .Returns(Task.FromResult(true));

            var service = new ServiceCollection();

            service.AddTransient(_ => proveedorRepoMock.Object);

            service.ConfigurePersonasService(new DbSettings());
            var provider = service.BuildServiceProvider();
            var proveedorService = provider.GetRequiredService<IProveedorService>();

            var result = await proveedorService.Update(new ProveedorRequestDto
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
        public async void Proveedor_Update_Test_IntegrationTest()
        {
            ServiceProvider provider = ServiceCollectionProveedor();

            var proveedorService = provider.GetRequiredService<IProveedorService>();
            var proveedorRepositorio = provider.GetRequiredService<IProveedorRepositorio>();
            var documentoService = provider.GetRequiredService<ITipoDocumentoService>();
            var documentoRepo = provider.GetRequiredService<ITipoDocumentoRepositorio>();

            var dtoDocumento = new TipoDocumentoRequestDto
            {
                Id = Guid.NewGuid(),
                NombreTipoDocumento = "fakeDocumentofakeProveedor07",
            };
            var documento = documentoRepo
                .SearchMatching<TipoDocumentoEntity>(x => x.NombreTipoDocumento == dtoDocumento.NombreTipoDocumento || x.Id == dtoDocumento.Id)
                .FirstOrDefault();
            if (documento != null || documento != default)
                documentoRepo.Delete(documento);

            _ = await documentoService.Insert(dtoDocumento).ConfigureAwait(false);

            var dtoProveedor = new ProveedorRequestDto
            {
                Id = Guid.NewGuid(),
                Nombre = "fake",
                Apellido = "fake",
                NumeroTelefono = 123456789,
                CorreoElectronico = "fake@fake.fake",
                CodigoTipoDocumento = "000000005",
                TipoPersona = (global::Evaluacion.Aplicacion.Dto.Especificas.Personas.TipoPersona)TipoPersona.Natural,
                FechaNacimiento = DateTimeOffset.Now,
                FechaRegistro = DateTimeOffset.Now,
                TipoDocumentoId = dtoDocumento.Id,
            };

            var proveedor = proveedorRepositorio
                .SearchMatching<ProveedorEntity>(x => x.Nombre == dtoProveedor.Nombre || x.Id == dtoProveedor.Id)
                .FirstOrDefault();
            if (proveedor != null || proveedor != default)
                proveedorRepositorio.Delete(proveedor);

            _ = await proveedorService.Insert(dtoProveedor).ConfigureAwait(false);

            var dtoProveedor2 = new ProveedorRequestDto
            {
                Id = dtoProveedor.Id,
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
            var result = await proveedorService.Update(dtoProveedor2).ConfigureAwait(false);

            Assert.True(result);

            _ = await proveedorService.Delete(dtoProveedor).ConfigureAwait(false);
            _ = await documentoService.Delete(dtoDocumento).ConfigureAwait(false);
        }
        #endregion
        #region Get
        [Fact]
        [UnitTest]
        public async Task Proveedor_Get_Test_Fail()
        {
            var proveedorRepoMock = new Mock<IProveedorRepositorio>();
            proveedorRepoMock
                .Setup(m => m.SearchMatchingOneResult(It.IsAny<Expression<Func<ProveedorEntity, bool>>>()));

            var service = new ServiceCollection();

            service.AddTransient(_ => proveedorRepoMock.Object);

            service.ConfigurePersonasService(new DbSettings());
            var provider = service.BuildServiceProvider();
            var proveedorService = provider.GetRequiredService<IProveedorService>();

            _ = await Assert.ThrowsAsync<ProveedorNoExistException>(() => proveedorService.Get(new ProveedorRequestDto { Nombre = "Nombre", Id = Guid.NewGuid() })).ConfigureAwait(false);
        }
        [Fact]
        [UnitTest]
        public async Task Proveedor_Get_Test_Full()
        {
            var proveedorRepoMock = new Mock<IProveedorRepositorio>();
            var entity = new ProveedorEntity
            {
                Id = Guid.NewGuid(),
                Nombre = "Nombre"
            };
            _ = proveedorRepoMock
                .Setup(m => m.SearchMatchingOneResult(It.IsAny<Expression<Func<ProveedorEntity, bool>>>()))
                .Returns(entity);

            var service = new ServiceCollection();

            service.AddTransient(_ => proveedorRepoMock.Object);

            service.ConfigurePersonasService(new DbSettings());
            var provider = service.BuildServiceProvider();
            var proveedorService = provider.GetRequiredService<IProveedorService>();

            var result = await proveedorService.Get(new ProveedorRequestDto
            {
                Id = Guid.NewGuid(),
            }).ConfigureAwait(false);

            Assert.NotNull(result.ToString());
            Assert.Equal(entity.Id, result.Id);

        }
        [Fact]
        [IntegrationTest]
        public async void Proveedor_Get_Test_IntegrationTest()
        {
            ServiceProvider provider = ServiceCollectionProveedor();

            var proveedorService = provider.GetRequiredService<IProveedorService>();
            var proveedorRepositorio = provider.GetRequiredService<IProveedorRepositorio>();
            var documentoService = provider.GetRequiredService<ITipoDocumentoService>();
            var documentoRepo = provider.GetRequiredService<ITipoDocumentoRepositorio>();

            var dtoDocumento = new TipoDocumentoRequestDto
            {
                Id = Guid.NewGuid(),
                NombreTipoDocumento = "fakeDocumentofakeProveedor05",
            };
            var documento = documentoRepo
                .SearchMatching<TipoDocumentoEntity>(x => x.NombreTipoDocumento == dtoDocumento.NombreTipoDocumento || x.Id == dtoDocumento.Id)
                .FirstOrDefault();
            if (documento != null || documento != default)
                documentoRepo.Delete(documento);

            _ = await documentoService.Insert(dtoDocumento).ConfigureAwait(false);

            var dtoProveedor = new ProveedorRequestDto
            {
                Id = Guid.NewGuid(),
                Nombre = "fake",
                Apellido = "fake",
                NumeroTelefono = 123456789,
                CorreoElectronico = "fake@fake.fake",
                CodigoTipoDocumento = "000000002",
                TipoPersona = (global::Evaluacion.Aplicacion.Dto.Especificas.Personas.TipoPersona)TipoPersona.Juridico,
                FechaNacimiento = DateTimeOffset.Now,
                FechaRegistro = DateTimeOffset.Now,
                TipoDocumentoId = dtoDocumento.Id,
            };

            var proveedor = proveedorRepositorio
                .SearchMatching<ProveedorEntity>(x => x.Nombre == dtoProveedor.Nombre || x.Id == dtoProveedor.Id)
                .FirstOrDefault();
            if (proveedor != null || proveedor != default)
                proveedorRepositorio.Delete(proveedor);

            _ = await proveedorService.Insert(dtoProveedor).ConfigureAwait(false);

            var dtoProveedor2 = new ProveedorRequestDto
            {
                Id = dtoProveedor.Id,
            };
            var result = await proveedorService.Get(dtoProveedor2).ConfigureAwait(false);

            Assert.Equal(dtoProveedor.Id, result.Id);
            _ = await proveedorService.Delete(dtoProveedor).ConfigureAwait(false);
            _ = await documentoService.Delete(dtoDocumento).ConfigureAwait(false);
        }
        #endregion
        #region GetAll
        [Fact]
        [UnitTest]
        public async Task Proveedor_GetAll_Test_Full()
        {
            var proveedorRepoMock = new Mock<IProveedorRepositorio>();
            var Listentity = new List<ProveedorEntity>
            {
                new ProveedorEntity
                {
                    Id = Guid.NewGuid(),
                    Nombre = "Nombre"
                }
            };
            _ = proveedorRepoMock
                .Setup(m => m.GetAll<ProveedorEntity>())
                .Returns(Listentity);

            var service = new ServiceCollection();

            service.AddTransient(_ => proveedorRepoMock.Object);

            service.ConfigurePersonasService(new DbSettings());
            var provider = service.BuildServiceProvider();
            var proveedorService = provider.GetRequiredService<IProveedorService>();
            var mapper = provider.GetRequiredService<IMapper>();

            var result = mapper.Map<IEnumerable<ProveedorEntity>>(await proveedorService.GetAll().ConfigureAwait(false));

            Assert.NotNull(result.ToString());
            Assert.Equal(Listentity[0].Id, result.First().Id);

        }
        [Fact]
        [IntegrationTest]
        public async void Proveedor_GetAll_Test_IntegrationTest()
        {
            ServiceProvider provider = ServiceCollectionProveedor();

            var proveedorService = provider.GetRequiredService<IProveedorService>();
            var proveedorRepositorio = provider.GetRequiredService<IProveedorRepositorio>();
            var mapper = provider.GetRequiredService<IMapper>();
            var documentoService = provider.GetRequiredService<ITipoDocumentoService>();
            var documentoRepo = provider.GetRequiredService<ITipoDocumentoRepositorio>();

            var dtoDocumento = new TipoDocumentoRequestDto
            {
                Id = Guid.NewGuid(),
                NombreTipoDocumento = "fakeDocumentofakeProveedor06",
            };
            var documento = documentoRepo
                .SearchMatching<TipoDocumentoEntity>(x => x.NombreTipoDocumento == dtoDocumento.NombreTipoDocumento || x.Id == dtoDocumento.Id)
                .FirstOrDefault();
            if (documento != null || documento != default)
                _ = await documentoRepo.Delete(documento).ConfigureAwait(false);

            _ = await documentoService.Insert(dtoDocumento).ConfigureAwait(false);

            var dtoProveedor = new ProveedorRequestDto
            {
                Id = Guid.Parse("45c2a9b5-1eac-48d3-83a4-ff692326e4f7"),
                Nombre = "NombreProveedor",
                Apellido = "NombreProveedor",
                NumeroTelefono = 123456789,
                CorreoElectronico = "fake@fake.fake",
                CodigoTipoDocumento = "000000008",
                TipoPersona = (global::Evaluacion.Aplicacion.Dto.Especificas.Personas.TipoPersona)TipoPersona.Juridico,
                FechaNacimiento = DateTimeOffset.Now,
                FechaRegistro = DateTimeOffset.Now,
                TipoDocumentoId = dtoDocumento.Id,
            };

            var proveedor = proveedorRepositorio
                .SearchMatching<ProveedorEntity>(x => x.Nombre == dtoProveedor.Nombre || x.Id == dtoProveedor.Id)
                .FirstOrDefault();
            if (proveedor != null || proveedor != default)
                _ = await proveedorRepositorio.Delete(proveedor).ConfigureAwait(false);

            _ = await proveedorService.Insert(dtoProveedor).ConfigureAwait(false);

            var result = mapper.Map<IEnumerable<ProveedorEntity>>(await proveedorService.GetAll().ConfigureAwait(false));

            Assert.NotNull(result.ToString());
            Assert.True(result.Any());

            _ = await proveedorService.Delete(dtoProveedor).ConfigureAwait(false);
            _ = await documentoService.Delete(dtoDocumento).ConfigureAwait(false);
        }
        #endregion
        [Fact]
        [IntegrationTest]
        public async void Validacion_Parametros_Proveedor_Integration()
        {
            ServiceProvider provider = ServiceCollectionProveedor();

            var proveedorService = provider.GetRequiredService<IProveedorService>();
            var proveedorRepositorio = provider.GetRequiredService<IProveedorRepositorio>();
            var documentoService = provider.GetRequiredService<ITipoDocumentoService>();
            var documentoRepo = provider.GetRequiredService<ITipoDocumentoRepositorio>();

            var dtoDocumento = new TipoDocumentoRequestDto
            {
                Id = Guid.NewGuid(),
                NombreTipoDocumento = "fakeDocumentofakeProveedor09",
            };
            var documento = documentoRepo
                .SearchMatching<TipoDocumentoEntity>(x => x.NombreTipoDocumento == dtoDocumento.NombreTipoDocumento || x.Id == dtoDocumento.Id)
                .FirstOrDefault();
            if (documento != null || documento != default)
                documentoRepo.Delete(documento);

            _ = await documentoService.Insert(dtoDocumento).ConfigureAwait(false);

            var dtoProveedor = new ProveedorRequestDto
            {
                Id = Guid.NewGuid(),
                Nombre = "Validacion_Parametros_Proveedor",
                Apellido = "Validacion_Parametros_Proveedor",
                NumeroTelefono = 123456789,
                CorreoElectronico = "fake@fake.fake",
                CodigoTipoDocumento = "000000003",
                TipoPersona = (global::Evaluacion.Aplicacion.Dto.Especificas.Personas.TipoPersona)TipoPersona.Juridico,
                FechaNacimiento = DateTimeOffset.Now,
                FechaRegistro = DateTimeOffset.Now,
                TipoDocumentoId = dtoDocumento.Id,
            };

            var proveedor = proveedorRepositorio
                .SearchMatching<ProveedorEntity>(x => x.Nombre == dtoProveedor.Nombre || x.Id == dtoProveedor.Id)
                .FirstOrDefault();
            if (proveedor != null || proveedor != default)
                proveedorRepositorio.Delete(proveedor);

            var response = await proveedorService.Insert(dtoProveedor).ConfigureAwait(false);
            Assert.NotNull(response.ToString());
            Assert.NotEqual(default, response);

            var id = dtoProveedor.Id;
            dtoProveedor.Id = Guid.NewGuid();
            _ = await Assert.ThrowsAsync<ProveedornameAlreadyExistException>(() => proveedorService.Insert(dtoProveedor)).ConfigureAwait(false);

            dtoProveedor.Nombre = "Fake2_Throws";
            _ = await Assert.ThrowsAsync<ProveedorCodigoTipoDocumentoException>(() => proveedorService.Insert(dtoProveedor)).ConfigureAwait(false);
            dtoProveedor.Id = id;

            dtoProveedor.CodigoTipoDocumento = "300000002";
            dtoProveedor.FechaNacimiento = default;
            _ = await Assert.ThrowsAsync<ProveedorFechaNacimientoException>(() => proveedorService.Insert(dtoProveedor)).ConfigureAwait(false);

            dtoProveedor.FechaNacimiento = DateTimeOffset.Now;
            dtoProveedor.FechaRegistro = default;
            _ = await Assert.ThrowsAsync<ProveedorFechaRegistroException>(() => proveedorService.Insert(dtoProveedor)).ConfigureAwait(false);

            _ = await proveedorService.Delete(dtoProveedor).ConfigureAwait(false);
            _ = await documentoService.Delete(dtoDocumento).ConfigureAwait(false);
        }
    }
}
