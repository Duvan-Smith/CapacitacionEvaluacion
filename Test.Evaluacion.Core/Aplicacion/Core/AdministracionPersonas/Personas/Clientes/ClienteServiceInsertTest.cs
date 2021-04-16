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
    public enum TipoPersona
    {
        Natural = 1,
        Juridico = 2,
    }
    public class ClienteServiceInsertTest
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
        //Cliente, Debe poderse distinguir entre jurídicas y naturales - hace parte de los parametros
        #region Validar_TipoPersona_Cliente
        [Fact]
        [UnitTest]
        public async Task Validar_TipoPersona_Cliente_Fail()
        {
            var clienteRepoMock = new Mock<IClienteRepositorio>();

            var Listentity = new List<ClienteEntity>
            {
                new ClienteEntity
                {
                    Id = Guid.NewGuid(),
                    Nombre = "Nombre",
                    CodigoTipoDocumento="123456789",
                    TipoDocumentoId=Guid.Parse("581E3E67-82E2-4F1F-B379-9BD870DB669E"),
                    TipoPersona = (global::Evaluacion.Dominio.Core.Especificas.Personas.TipoPersona)TipoPersona.Natural,
                }
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
                .Returns(Listentity);

            var service = new ServiceCollection();

            service.AddTransient(_ => clienteRepoMock.Object);
            service.AddTransient(_ => tipoDocumentoRepoMock.Object);

            service.ConfigurePersonasService(new DbSettings());

            var provider = service.BuildServiceProvider();
            var clienteService = provider.GetRequiredService<IClienteService>();

            var dtoCliente = new ClienteRequestDto
            {
                Nombre = "fake",
                FechaNacimiento = DateTimeOffset.Now,
                FechaRegistro = DateTimeOffset.Now,
                TipoDocumentoId = Guid.Parse("581E3E67-82E2-4F1F-B379-9BD870DB669E")
            };

            _ = await Assert.ThrowsAsync<ClienteTipoPersonaNullException>(() => clienteService.Insert(dtoCliente)).ConfigureAwait(false);

            dtoCliente.TipoPersona = 0;
            _ = await Assert.ThrowsAsync<ClienteTipoPersonaNullException>(() => clienteService.Insert(dtoCliente)).ConfigureAwait(false);
        }
        [Fact]
        [UnitTest]
        public async Task Validar_TipoPersona_Cliente_Full()
        {
            var clienteRepoMock = new Mock<IClienteRepositorio>();

            var Listentity = new List<ClienteEntity>
            {
                new ClienteEntity
                {
                    Id = Guid.NewGuid(),
                    Nombre = "Nombre",
                    CodigoTipoDocumento="123456789",
                    TipoDocumentoId=Guid.Parse("581E3E67-82E2-4F1F-B379-9BD870DB669E"),
                    TipoPersona = (global::Evaluacion.Dominio.Core.Especificas.Personas.TipoPersona)TipoPersona.Natural,
                }
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
                .Setup(m => m.Insert(It.IsAny<ClienteEntity>()))
                .Returns(Task.FromResult(new ClienteEntity { Id = Guid.NewGuid() }));

            var service = new ServiceCollection();

            service.AddTransient(_ => clienteRepoMock.Object);
            service.AddTransient(_ => tipoDocumentoRepoMock.Object);

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
        [Fact]
        [IntegrationTest]
        public async void Validar_TipoPersona_Cliente_IntegrationTest()
        {
            ServiceProvider provider = ServiceCollectionCliente();

            var clienteService = provider.GetRequiredService<IClienteService>();
            var documentoService = provider.GetRequiredService<ITipoDocumentoService>();
            var documentoRepo = provider.GetRequiredService<ITipoDocumentoRepositorio>();

            var dtoDocumento = new TipoDocumentoRequestDto
            {
                Id = Guid.NewGuid(),
                NombreTipoDocumento = "fakeDocumentoClienteInsert11",
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
                Nombre = "fake",
                Apellido = "fake",
                NumeroTelefono = 123456789,
                CorreoElectronico = "fake@fake.fake",
                CodigoTipoDocumento = "000000006",
                FechaNacimiento = DateTimeOffset.Now,
                FechaRegistro = DateTimeOffset.Now,
                TipoDocumentoId = dtoDocumento.Id,
            };

            var id = dtoCliente.Id;
            dtoCliente.Id = Guid.NewGuid();
            _ = await Assert.ThrowsAsync<ClienteTipoPersonaNullException>(() => clienteService.Insert(dtoCliente)).ConfigureAwait(false);
            dtoCliente.Id = id;

            dtoCliente.TipoPersona = (global::Evaluacion.Aplicacion.Dto.Especificas.Personas.TipoPersona)TipoPersona.Juridico;
            var response = await clienteService.Insert(dtoCliente).ConfigureAwait(false);
            Assert.NotNull(response.ToString());
            Assert.NotEqual(default, response);

            _ = await clienteService.Delete(dtoCliente).ConfigureAwait(false);
            _ = await documentoService.Delete(dtoDocumento).ConfigureAwait(false);
        }
        #endregion
        //Cliente, No puede haber dos personas con el mismo numero y tipo de identificación
        #region No_Se_Repite_CodigoTipoDocumento_Cliente
        [Fact]
        [UnitTest]
        public async Task No_Se_Repite_CodigoTipoDocumento_Cliente_Fail()
        {
            var clienteRepoMock = new Mock<IClienteRepositorio>();

            var Listentity = new List<ClienteEntity>
            {
                new ClienteEntity
                {
                    Id = Guid.NewGuid(),
                    Nombre = "Nombre",
                    CodigoTipoDocumento="123456789",
                    TipoDocumentoId=Guid.Parse("581E3E67-82E2-4F1F-B379-9BD870DB669E"),
                    TipoPersona = (global::Evaluacion.Dominio.Core.Especificas.Personas.TipoPersona)TipoPersona.Natural,
                }
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
                .Returns(Listentity);

            var service = new ServiceCollection();

            service.AddTransient(_ => clienteRepoMock.Object);
            service.AddTransient(_ => tipoDocumentoRepoMock.Object);

            service.ConfigurePersonasService(new DbSettings());

            var provider = service.BuildServiceProvider();
            var clienteService = provider.GetRequiredService<IClienteService>();

            var clienteDto = new ClienteRequestDto
            {
                Nombre = "Nombre2",
                TipoPersona = (global::Evaluacion.Aplicacion.Dto.Especificas.Personas.TipoPersona)TipoPersona.Natural,
                FechaNacimiento = DateTimeOffset.Now,
                FechaRegistro = DateTimeOffset.Now,
                CodigoTipoDocumento = "123456789",
                TipoDocumentoId = Guid.Parse("581E3E67-82E2-4F1F-B379-9BD870DB669E")
            };

            _ = await Assert.ThrowsAsync<ClienteCodigoTipoDocumentoException>(() => clienteService.Insert(clienteDto)).ConfigureAwait(false);
        }
        [Fact]
        [UnitTest]
        public async Task No_Se_Repite_CodigoTipoDocumento_Cliente_Full()
        {
            var clienteRepoMock = new Mock<IClienteRepositorio>();
            var Listentity = new List<ClienteEntity>
            {
                new ClienteEntity
                {
                    Id = Guid.NewGuid(),
                    Nombre = "Nombre",
                    CodigoTipoDocumento="123456789",
                    TipoDocumentoId=Guid.Parse("581E3E67-82E2-4F1F-B379-9BD870DB669E"),
                    TipoPersona = (global::Evaluacion.Dominio.Core.Especificas.Personas.TipoPersona)TipoPersona.Natural,
                }
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
                .Setup(m => m.Insert(It.IsAny<ClienteEntity>()))
                .Returns(Task.FromResult(new ClienteEntity { Id = Guid.NewGuid() }));

            var service = new ServiceCollection();

            service.AddTransient(_ => clienteRepoMock.Object);
            service.AddTransient(_ => tipoDocumentoRepoMock.Object);

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
        [Fact]
        [IntegrationTest]
        public async void No_Se_Repite_CodigoTipoDocumento_Cliente_IntegrationTest()
        {
            ServiceProvider provider = ServiceCollectionCliente();

            var clienteService = provider.GetRequiredService<IClienteService>();
            var clienteRepositorio = provider.GetRequiredService<IClienteRepositorio>();
            var documentoService = provider.GetRequiredService<ITipoDocumentoService>();
            var documentoRepo = provider.GetRequiredService<ITipoDocumentoRepositorio>();

            var dtoDocumento = new TipoDocumentoRequestDto
            {
                Id = Guid.NewGuid(),
                NombreTipoDocumento = "fakeDocumentofakeCliente07",
            };
            var documento = documentoRepo
                .SearchMatching<TipoDocumentoEntity>(x => x.NombreTipoDocumento == dtoDocumento.NombreTipoDocumento || x.Id == dtoDocumento.Id)
                .FirstOrDefault();
            if (documento != null || documento != default)
                _ = await documentoRepo.Delete(documento).ConfigureAwait(false);

            _ = await documentoService.Insert(dtoDocumento).ConfigureAwait(false);

            var dtoDocumento2 = new TipoDocumentoRequestDto
            {
                Id = Guid.NewGuid(),
                NombreTipoDocumento = "fakeDocumentofakeCliente08",
            };
            var documento2 = documentoRepo
                .SearchMatching<TipoDocumentoEntity>(x => x.NombreTipoDocumento == dtoDocumento2.NombreTipoDocumento || x.Id == dtoDocumento2.Id)
                .FirstOrDefault();
            if (documento2 != null || documento2 != default)
                _ = await documentoRepo.Delete(documento2).ConfigureAwait(false);

            _ = await documentoService.Insert(dtoDocumento2).ConfigureAwait(false);

            var dtoCliente = new ClienteRequestDto
            {
                Id = Guid.NewGuid(),
                Nombre = "fake_Cliente_1",
                Apellido = "fake_Cliente_1",
                NumeroTelefono = 123456789,
                CorreoElectronico = "fake@fake.fake",
                CodigoTipoDocumento = "000000005",
                TipoPersona = (global::Evaluacion.Aplicacion.Dto.Especificas.Personas.TipoPersona)TipoPersona.Juridico,
                FechaNacimiento = DateTimeOffset.Now,
                FechaRegistro = DateTimeOffset.Now,
                TipoDocumentoId = dtoDocumento.Id,
            };
            var usernameExist = clienteRepositorio
                            .SearchMatching<ClienteEntity>(x => x.Nombre == dtoCliente.Nombre)
                            .FirstOrDefault();
            if (usernameExist != null || usernameExist != default)
                _ = await clienteRepositorio.Delete(usernameExist).ConfigureAwait(false);

            var response = await clienteService.Insert(dtoCliente).ConfigureAwait(false);
            Assert.NotNull(response.ToString());
            Assert.NotEqual(default, response);

            var id = dtoCliente.Id;
            dtoCliente.Id = Guid.NewGuid();
            dtoCliente.Nombre = "Cliente_fake_Throws_1";
            _ = await Assert.ThrowsAsync<ClienteCodigoTipoDocumentoException>(() => clienteService.Insert(dtoCliente)).ConfigureAwait(false);
            dtoCliente.Id = id;

            var dtoCliente2 = new ClienteRequestDto
            {
                Id = Guid.NewGuid(),
                Nombre = "Fake_Cliente_2",
                Apellido = "Fake_Cliente_2",
                NumeroTelefono = 123456789,
                CorreoElectronico = "fake@fake.fake",
                CodigoTipoDocumento = "000000005",
                TipoPersona = (global::Evaluacion.Aplicacion.Dto.Especificas.Personas.TipoPersona)TipoPersona.Juridico,
                FechaNacimiento = DateTimeOffset.Now,
                FechaRegistro = DateTimeOffset.Now,
                TipoDocumentoId = dtoDocumento2.Id,
            };
            var usernameExist2 = clienteRepositorio
                            .SearchMatching<ClienteEntity>(x => x.Nombre == dtoCliente2.Nombre)
                            .FirstOrDefault();
            if (usernameExist2 != null || usernameExist2 != default)
                _ = await clienteRepositorio.Delete(usernameExist2).ConfigureAwait(false);

            var response2 = await clienteService.Insert(dtoCliente2).ConfigureAwait(false);
            Assert.NotNull(response2.ToString());
            Assert.NotEqual(default, response2);

            _ = await clienteService.Delete(dtoCliente).ConfigureAwait(false);
            _ = await clienteService.Delete(dtoCliente2).ConfigureAwait(false);
            _ = await documentoService.Delete(dtoDocumento).ConfigureAwait(false);
            _ = await documentoService.Delete(dtoDocumento2).ConfigureAwait(false);
        }
        #endregion
        //Cliente, No puede haber dos personas con el mismo nombre / razón social
        #region No_Se_Repite_Nombre_Cliente
        [Fact]
        [UnitTest]
        public async Task No_Se_Repite_Nombre_Cliente_Fail()
        {
            var clienteRepoMock = new Mock<IClienteRepositorio>();

            var Listentity = new List<ClienteEntity>
            {
                new ClienteEntity
                {
                    Id = Guid.NewGuid(),
                    Nombre = "Nombre",
                    CodigoTipoDocumento="123456789",
                    TipoDocumentoId=Guid.Parse("581E3E67-82E2-4F1F-B379-9BD870DB669E"),
                    TipoPersona = (global::Evaluacion.Dominio.Core.Especificas.Personas.TipoPersona)TipoPersona.Natural,
                }
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
                .Returns(Listentity);

            var service = new ServiceCollection();

            service.AddTransient(_ => clienteRepoMock.Object);
            service.AddTransient(_ => tipoDocumentoRepoMock.Object);

            service.ConfigurePersonasService(new DbSettings());

            var provider = service.BuildServiceProvider();
            var clienteService = provider.GetRequiredService<IClienteService>();

            _ = await Assert.ThrowsAsync<ClientenameAlreadyExistException>(() => clienteService.Insert(new ClienteRequestDto
            {
                Id = Guid.NewGuid(),
                Nombre = "Nombre",
                TipoPersona = (global::Evaluacion.Aplicacion.Dto.Especificas.Personas.TipoPersona)TipoPersona.Natural,
                FechaNacimiento = DateTimeOffset.Now,
                FechaRegistro = DateTimeOffset.Now,
                TipoDocumentoId = Guid.Parse("581E3E67-82E2-4F1F-B379-9BD870DB669E")
            })).ConfigureAwait(false);
        }
        [Fact]
        [UnitTest]
        public async Task No_Se_Repite_Nombre_Cliente_Full()
        {
            var clienteRepoMock = new Mock<IClienteRepositorio>();

            var tipoDocumentoRepoMock = new Mock<ITipoDocumentoRepositorio>();

            _ = tipoDocumentoRepoMock
                .Setup(m => m.SearchMatching(It.IsAny<Expression<Func<TipoDocumentoEntity, bool>>>()))
                .Returns(new List<TipoDocumentoEntity>{ new TipoDocumentoEntity
                {
                    Id = Guid.NewGuid(),
                    NombreTipoDocumento = "fakenonit"
                }});
            _ = clienteRepoMock
                .Setup(m => m.GetAll<ClienteEntity>());

            _ = clienteRepoMock
                .Setup(m => m.Insert(It.IsAny<ClienteEntity>()))
                .Returns(Task.FromResult(new ClienteEntity { Id = Guid.NewGuid() }));

            var service = new ServiceCollection();

            service.AddTransient(_ => clienteRepoMock.Object);
            service.AddTransient(_ => tipoDocumentoRepoMock.Object);

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
        [Fact]
        [IntegrationTest]
        public async void No_Se_Repite_Nombre_Cliente_IntegrationTest()
        {
            ServiceProvider provider = ServiceCollectionCliente();

            var clienteService = provider.GetRequiredService<IClienteService>();
            var documentoService = provider.GetRequiredService<ITipoDocumentoService>();
            var documentoRepo = provider.GetRequiredService<ITipoDocumentoRepositorio>();

            var dtoDocumento = new TipoDocumentoRequestDto
            {
                Id = Guid.NewGuid(),
                NombreTipoDocumento = "fakeDocumentoClienteInsertI09",
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
            var response = await clienteService.Insert(dtoCliente).ConfigureAwait(false);
            Assert.NotNull(response.ToString());
            Assert.NotEqual(default, response);

            var id = dtoCliente.Id;
            dtoCliente.Id = Guid.NewGuid();
            _ = await Assert.ThrowsAsync<ClientenameAlreadyExistException>(() => clienteService.Insert(dtoCliente)).ConfigureAwait(false);
            dtoCliente.Id = id;

            _ = await clienteService.Delete(dtoCliente).ConfigureAwait(false);
            _ = await documentoService.Delete(dtoDocumento).ConfigureAwait(false);
        }
        #endregion
        //Cliente, La fecha de nacimiento / creación es obligatoria
        #region Cliente_Validar_Fechas
        [Fact]
        [UnitTest]
        public async Task Cliente_Validar_Fechas_Fail()
        {
            var clienteRepoMock = new Mock<IClienteRepositorio>();
            var tipoDocumentoRepoMock = new Mock<ITipoDocumentoRepositorio>();

            _ = tipoDocumentoRepoMock
                .Setup(m => m.SearchMatching(It.IsAny<Expression<Func<TipoDocumentoEntity, bool>>>()))
                .Returns(new List<TipoDocumentoEntity>{ new TipoDocumentoEntity
                {
                    Id = Guid.NewGuid(),
                    NombreTipoDocumento = "fakenonit"
                }});
            _ = clienteRepoMock
                .Setup(m => m.GetAll<ClienteEntity>());
            _ = clienteRepoMock
                .Setup(m => m.Insert(It.IsAny<ClienteEntity>()))
                .Returns(Task.FromResult(new ClienteEntity { Id = Guid.NewGuid() }));

            var service = new ServiceCollection();

            service.AddTransient(_ => clienteRepoMock.Object);
            service.AddTransient(_ => tipoDocumentoRepoMock.Object);

            service.ConfigurePersonasService(new DbSettings());
            var provider = service.BuildServiceProvider();
            var clienteService = provider.GetRequiredService<IClienteService>();
            var clienteDto = new ClienteRequestDto
            {
                Nombre = "FakePrueba",
                TipoPersona = (global::Evaluacion.Aplicacion.Dto.Especificas.Personas.TipoPersona)TipoPersona.Natural,
                FechaNacimiento = DateTimeOffset.Now,
                FechaRegistro = DateTimeOffset.Now,
                TipoDocumentoId = Guid.NewGuid()
            };

            clienteDto.FechaNacimiento = default;
            _ = await Assert.ThrowsAsync<ClienteFechaNacimientoException>(() => clienteService.Insert(clienteDto)).ConfigureAwait(false);

            clienteDto.FechaNacimiento = DateTimeOffset.Now;
            clienteDto.FechaRegistro = default;
            _ = await Assert.ThrowsAsync<ClienteFechaRegistroException>(() => clienteService.Insert(clienteDto)).ConfigureAwait(false);
        }
        [Fact]
        [UnitTest]
        public async Task Cliente_Validar_Fechas_Full()
        {
            var clienteRepoMock = new Mock<IClienteRepositorio>();

            var Listentity = new List<ClienteEntity>
            {
                new ClienteEntity
                {
                    Id = Guid.NewGuid(),
                    Nombre = "Nombre",
                    CodigoTipoDocumento="123456789",
                    TipoDocumentoId=Guid.Parse("581E3E67-82E2-4F1F-B379-9BD870DB669E"),
                    TipoPersona = (global::Evaluacion.Dominio.Core.Especificas.Personas.TipoPersona)TipoPersona.Natural,
                }
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
                .Setup(m => m.Insert(It.IsAny<ClienteEntity>()))
                .Returns(Task.FromResult(new ClienteEntity { Id = Guid.NewGuid() }));

            var service = new ServiceCollection();

            service.AddTransient(_ => clienteRepoMock.Object);
            service.AddTransient(_ => tipoDocumentoRepoMock.Object);

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
        [Fact]
        [IntegrationTest]
        public async void Cliente_Validar_Fechas_IntegrationTest()
        {
            ServiceProvider provider = ServiceCollectionCliente();

            var clienteService = provider.GetRequiredService<IClienteService>();
            var documentoService = provider.GetRequiredService<ITipoDocumentoService>();
            var documentoRepo = provider.GetRequiredService<ITipoDocumentoRepositorio>();

            var dtoDocumento = new TipoDocumentoRequestDto
            {
                Id = Guid.NewGuid(),
                NombreTipoDocumento = "fakeDocumentofakeCliente05",
            };
            var documento = documentoRepo
                .SearchMatching<TipoDocumentoEntity>(x => x.NombreTipoDocumento == dtoDocumento.NombreTipoDocumento || x.Id == dtoDocumento.Id)
                .FirstOrDefault();
            if (documento != null || documento != default)
                _ = await documentoRepo.Delete(documento).ConfigureAwait(false);

            await documentoService.Insert(dtoDocumento).ConfigureAwait(false);

            var dtoCliente = new ClienteRequestDto
            {
                Id = Guid.NewGuid(),
                Nombre = "fake",
                Apellido = "fake",
                NumeroTelefono = 123456789,
                CorreoElectronico = "fake@fake.fake",
                CodigoTipoDocumento = "000000007",
                TipoPersona = (global::Evaluacion.Aplicacion.Dto.Especificas.Personas.TipoPersona)TipoPersona.Juridico,
                FechaNacimiento = DateTimeOffset.Now,
                FechaRegistro = DateTimeOffset.Now,
                TipoDocumentoId = dtoDocumento.Id,
            };


            dtoCliente.FechaNacimiento = default;
            _ = await Assert.ThrowsAsync<ClienteFechaNacimientoException>(() => clienteService.Insert(dtoCliente)).ConfigureAwait(false);

            dtoCliente.FechaNacimiento = DateTimeOffset.Now;
            dtoCliente.FechaRegistro = default;
            _ = await Assert.ThrowsAsync<ClienteFechaRegistroException>(() => clienteService.Insert(dtoCliente)).ConfigureAwait(false);

            dtoCliente.FechaRegistro = DateTimeOffset.Now;
            var response = await clienteService.Insert(dtoCliente).ConfigureAwait(false);
            Assert.NotNull(response.ToString());
            Assert.NotEqual(default, response);

            _ = await clienteService.Delete(dtoCliente).ConfigureAwait(false);
            _ = await documentoService.Delete(dtoDocumento).ConfigureAwait(false);
        }
        #endregion
        //Cliente, Una persona no puede tener el tipo de documento nit 
        #region Cliente_Validar_NIT
        [Fact]
        [UnitTest]
        public async Task Cliente_Validar_NIT_Fail()
        {
            var Listentity = new List<ClienteEntity>
            {
                new ClienteEntity
                {
                    Id = Guid.NewGuid(),
                    Nombre = "Nombre",
                    CodigoTipoDocumento="123456789",
                    TipoDocumentoId=Guid.Parse("581E3E67-82E2-4F1F-B379-9BD870DB669E"),
                    TipoPersona = (global::Evaluacion.Dominio.Core.Especificas.Personas.TipoPersona)TipoPersona.Natural,
                }
            };
            var clienteRepoMock = new Mock<IClienteRepositorio>();

            var tipoDocumentoRepoMock = new Mock<ITipoDocumentoRepositorio>();

            _ = tipoDocumentoRepoMock
                .Setup(m => m.SearchMatching(It.IsAny<Expression<Func<TipoDocumentoEntity, bool>>>()))
                .Returns(new List<TipoDocumentoEntity>{ new TipoDocumentoEntity
                {
                    Id = Guid.NewGuid(),
                    NombreTipoDocumento = "nit"
                }});
            _ = clienteRepoMock
                .Setup(m => m.GetAll<ClienteEntity>())
                .Returns(Listentity);
            _ = clienteRepoMock
                .Setup(m => m.Insert(It.IsAny<ClienteEntity>()))
                .Returns(Task.FromResult(new ClienteEntity { Id = Guid.NewGuid() }));

            var service = new ServiceCollection();

            service.AddTransient(_ => clienteRepoMock.Object);
            service.AddTransient(_ => tipoDocumentoRepoMock.Object);

            service.ConfigurePersonasService(new DbSettings());
            var provider = service.BuildServiceProvider();
            var clienteService = provider.GetRequiredService<IClienteService>();

            _ = await Assert.ThrowsAsync<ClienteTipoDocumentoException>(() => clienteService.Insert(new ClienteRequestDto
            {
                Nombre = "FakePrueba",
                TipoPersona = (global::Evaluacion.Aplicacion.Dto.Especificas.Personas.TipoPersona)TipoPersona.Natural,
                FechaNacimiento = DateTimeOffset.Now,
                FechaRegistro = DateTimeOffset.Now,
                TipoDocumentoId = Guid.Parse("A89DAA40-149F-439A-8A08-7842E09D7376")
            })).ConfigureAwait(false);
        }
        [Fact]
        [UnitTest]
        public async Task Cliente_Validar_NIT_Full()
        {
            var Listentity = new List<ClienteEntity>
            {
                new ClienteEntity
                {
                    Id = Guid.NewGuid(),
                    Nombre = "Nombre",
                    CodigoTipoDocumento="123456789",
                    TipoDocumentoId=Guid.Parse("581E3E67-82E2-4F1F-B379-9BD870DB669E"),
                    TipoPersona = (global::Evaluacion.Dominio.Core.Especificas.Personas.TipoPersona)TipoPersona.Natural,
                }
            };
            var clienteRepoMock = new Mock<IClienteRepositorio>();

            var tipoDocumentoRepoMock = new Mock<ITipoDocumentoRepositorio>();

            _ = tipoDocumentoRepoMock
                .Setup(m => m.SearchMatching(It.IsAny<Expression<Func<TipoDocumentoEntity, bool>>>()))
                .Returns(new List<TipoDocumentoEntity>{ new TipoDocumentoEntity
                {
                    Id = Guid.NewGuid(),
                    NombreTipoDocumento = "nonit"
                }});
            _ = clienteRepoMock
                .Setup(m => m.GetAll<ClienteEntity>())
                .Returns(Listentity);
            clienteRepoMock
                .Setup(m => m.Insert(It.IsAny<ClienteEntity>()))
                .Returns(Task.FromResult(new ClienteEntity { Id = Guid.NewGuid() }));

            var service = new ServiceCollection();

            service.AddTransient(_ => clienteRepoMock.Object);
            service.AddTransient(_ => tipoDocumentoRepoMock.Object);

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
        [Fact]
        [IntegrationTest]
        public async void Cliente_Validar_NIT_IntegrationTest()
        {
            ServiceProvider provider = ServiceCollectionCliente();

            var clienteService = provider.GetRequiredService<IClienteService>();
            var documentoService = provider.GetRequiredService<ITipoDocumentoService>();
            var documentoRepo = provider.GetRequiredService<ITipoDocumentoRepositorio>();

            var dtoDocumento = new TipoDocumentoRequestDto
            {
                Id = Guid.NewGuid(),
                NombreTipoDocumento = "fakeDocumentofakeCliente06",
            };
            var documento = documentoRepo
                .SearchMatching<TipoDocumentoEntity>(x => x.NombreTipoDocumento == dtoDocumento.NombreTipoDocumento || x.Id == dtoDocumento.Id)
                .FirstOrDefault();
            if (documento != null || documento != default)
                _ = await documentoRepo.Delete(documento).ConfigureAwait(false);

            _ = await documentoService.Insert(dtoDocumento).ConfigureAwait(false);

            var dtoDocumento2 = new TipoDocumentoRequestDto
            {
                Id = Guid.NewGuid(),
                NombreTipoDocumento = "nit",
            };
            var documento2 = documentoRepo
                .SearchMatching<TipoDocumentoEntity>(x => x.NombreTipoDocumento == dtoDocumento2.NombreTipoDocumento || x.Id == dtoDocumento2.Id)
                .FirstOrDefault();
            if (documento2 != null || documento2 != default)
                _ = await documentoRepo.Delete(documento2).ConfigureAwait(false);

            _ = await documentoService.Insert(dtoDocumento2).ConfigureAwait(false);

            var dtoCliente = new ClienteRequestDto
            {
                Id = Guid.NewGuid(),
                Nombre = "fake_cliente_insert3",
                Apellido = "fake_cliente_insert3",
                NumeroTelefono = 123456789,
                CorreoElectronico = "fake@fake.fake",
                CodigoTipoDocumento = "000000003",
                TipoPersona = (global::Evaluacion.Aplicacion.Dto.Especificas.Personas.TipoPersona)TipoPersona.Juridico,
                FechaNacimiento = DateTimeOffset.Now,
                FechaRegistro = DateTimeOffset.Now,
                TipoDocumentoId = dtoDocumento.Id,
            };

            dtoCliente.TipoDocumentoId = dtoDocumento2.Id;
            _ = await Assert.ThrowsAsync<ClienteTipoDocumentoException>(() => clienteService.Insert(dtoCliente)).ConfigureAwait(false);

            dtoCliente.TipoDocumentoId = dtoDocumento.Id;
            var response = await clienteService.Insert(dtoCliente).ConfigureAwait(false);
            Assert.NotNull(response.ToString());
            Assert.NotEqual(default, response);

            _ = await clienteService.Delete(dtoCliente).ConfigureAwait(false);
            _ = await documentoService.Delete(dtoDocumento).ConfigureAwait(false);
            _ = await documentoService.Delete(dtoDocumento2).ConfigureAwait(false);
        }
        #endregion
        //Cliente, Test de integracion para el cliente
        [Fact]
        [IntegrationTest]
        public async void Validacion_Parametros_Cliente_Integration()
        {
            ServiceProvider provider = ServiceCollectionCliente();

            var clienteService = provider.GetRequiredService<IClienteService>();
            var documentoService = provider.GetRequiredService<ITipoDocumentoService>();
            var documentoRepo = provider.GetRequiredService<ITipoDocumentoRepositorio>();

            var dtoDocumento = new TipoDocumentoRequestDto
            {
                Id = Guid.NewGuid(),
                NombreTipoDocumento = "fakeDocumentoClienteInsert10",
            };
            var documento = documentoRepo
                .SearchMatching<TipoDocumentoEntity>(x => x.NombreTipoDocumento == dtoDocumento.NombreTipoDocumento || x.Id == dtoDocumento.Id)
                .FirstOrDefault();
            if (documento != null || documento != default)
                _ = await documentoRepo.Delete(documento).ConfigureAwait(false);

            _ = await documentoService.Insert(dtoDocumento).ConfigureAwait(false);

            var dtoDocumento2 = new TipoDocumentoRequestDto
            {
                Id = Guid.NewGuid(),
                NombreTipoDocumento = "nit",
            };
            var documento2 = documentoRepo
                .SearchMatching<TipoDocumentoEntity>(x => x.NombreTipoDocumento == dtoDocumento2.NombreTipoDocumento || x.Id == dtoDocumento2.Id)
                .FirstOrDefault();
            if (documento2 != null || documento2 != default)
                _ = await documentoRepo.Delete(documento2).ConfigureAwait(false);

            _ = await documentoService.Insert(dtoDocumento2).ConfigureAwait(false);

            var dtoCliente = new ClienteRequestDto
            {
                Id = Guid.NewGuid(),
                Nombre = "fake",
                Apellido = "fake",
                NumeroTelefono = 123456789,
                CorreoElectronico = "fake@fake.fake",
                CodigoTipoDocumento = "000000001",
                TipoPersona = (global::Evaluacion.Aplicacion.Dto.Especificas.Personas.TipoPersona)TipoPersona.Juridico,
                FechaNacimiento = DateTimeOffset.Now,
                FechaRegistro = DateTimeOffset.Now,
                TipoDocumentoId = dtoDocumento.Id,
            };
            var response = await clienteService.Insert(dtoCliente).ConfigureAwait(false);
            Assert.NotNull(response.ToString());
            Assert.NotEqual(default, response);

            var id = dtoCliente.Id;
            dtoCliente.Id = Guid.NewGuid();
            _ = await Assert.ThrowsAsync<ClientenameAlreadyExistException>(() => clienteService.Insert(dtoCliente)).ConfigureAwait(false);

            dtoCliente.Nombre = "Fake2";
            _ = await Assert.ThrowsAsync<ClienteCodigoTipoDocumentoException>(() => clienteService.Insert(dtoCliente)).ConfigureAwait(false);
            dtoCliente.Id = id;

            dtoCliente.CodigoTipoDocumento = "000000002";
            dtoCliente.FechaNacimiento = default;
            _ = await Assert.ThrowsAsync<ClienteFechaNacimientoException>(() => clienteService.Insert(dtoCliente)).ConfigureAwait(false);

            dtoCliente.FechaNacimiento = DateTimeOffset.Now;
            dtoCliente.FechaRegistro = default;
            _ = await Assert.ThrowsAsync<ClienteFechaRegistroException>(() => clienteService.Insert(dtoCliente)).ConfigureAwait(false);

            dtoCliente.FechaRegistro = DateTimeOffset.Now;
            dtoCliente.TipoDocumentoId = dtoDocumento2.Id;
            _ = await Assert.ThrowsAsync<ClienteTipoDocumentoException>(() => clienteService.Insert(dtoCliente)).ConfigureAwait(false);

            _ = await clienteService.Delete(dtoCliente).ConfigureAwait(false);
            _ = await documentoService.Delete(dtoDocumento).ConfigureAwait(false);
            _ = await documentoService.Delete(dtoDocumento2).ConfigureAwait(false);
        }
    }
}
