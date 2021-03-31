using AutoMapper;
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
        //TODO: Cliente, Debe poderse distinguir entre jurídicas y naturales - hace parte de los parametros
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
            clienteRepoMock
                .Setup(m => m.GetAll<ClienteEntity>())
                .Returns(Listentity);

            var service = new ServiceCollection();

            service.AddTransient(_ => clienteRepoMock.Object);

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

            await Assert.ThrowsAsync<ClienteTipoPersonaNullException>(() => clienteService.Insert(dtoCliente)).ConfigureAwait(false);

            dtoCliente.TipoPersona = 0;
            await Assert.ThrowsAsync<ClienteTipoPersonaNullException>(() => clienteService.Insert(dtoCliente)).ConfigureAwait(false);
        }
        [Fact]
        [UnitTest]
        public async Task Validar_TipoPersona_Cliente_Full()
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
        [Fact]
        [IntegrationTest]
        public async void Validar_TipoPersona_Cliente_IntegrationTest()
        {
            var service = new ServiceCollection();

            service.ConfigurePersonasService(new DbSettings
            {
                ConnectionString = "Data Source=DESKTOP-NE15I70\\BDDUVAN;Initial Catalog=evaluacion;User ID=sa;Password=3147073260"
            });

            var providerP = service.BuildServiceProvider();

            var clienteService = providerP.GetRequiredService<IClienteService>();

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
                TipoDocumentoId = Guid.Parse("581E3E67-82E2-4F1F-B379-9BD870DB669E"),
            };

            var id = dtoCliente.Id;
            dtoCliente.Id = Guid.NewGuid();
            await Assert.ThrowsAsync<ClienteTipoPersonaNullException>(() => clienteService.Insert(dtoCliente)).ConfigureAwait(false);
            dtoCliente.Id = id;

            dtoCliente.TipoPersona = (global::Evaluacion.Aplicacion.Dto.Especificas.Personas.TipoPersona)TipoPersona.Juridico;
            var response = await clienteService.Insert(dtoCliente).ConfigureAwait(false);
            Assert.NotNull(response.ToString());
            Assert.NotEqual(default, response);

            _ = clienteService.Delete(dtoCliente);
        }
        #endregion
        //TODO: Cliente, No puede haber dos personas con el mismo numero y tipo de identificación
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
            clienteRepoMock
                .Setup(m => m.GetAll<ClienteEntity>())
                .Returns(Listentity);

            var service = new ServiceCollection();

            service.AddTransient(_ => clienteRepoMock.Object);

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

            await Assert.ThrowsAsync<ClienteCodigoTipoDocumentoException>(() => clienteService.Insert(clienteDto)).ConfigureAwait(false);
        }
        [Fact]
        [UnitTest]
        public async Task No_Se_Repite_CodigoTipoDocumento_Cliente_Full()
        {
            var clienteRepoMock = new Mock<IClienteRepositorio>();

            clienteRepoMock
                .Setup(m => m.SearchMatching(It.IsAny<Expression<Func<ClienteEntity, bool>>>()));
            clienteRepoMock
                .Setup(m => m.Insert(It.IsAny<ClienteEntity>()))
                .Returns(Task.FromResult(new ClienteEntity { Id = Guid.NewGuid() }));

            var service = new ServiceCollection();

            service.AddTransient(_ => clienteRepoMock.Object);

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
            var service = new ServiceCollection();

            service.ConfigurePersonasService(new DbSettings
            {
                ConnectionString = "Data Source=DESKTOP-NE15I70\\BDDUVAN;Initial Catalog=evaluacion;User ID=sa;Password=3147073260"
            });

            var providerP = service.BuildServiceProvider();

            var clienteService = providerP.GetRequiredService<IClienteService>();
            var clienteRepositorio = providerP.GetRequiredService<IClienteRepositorio>();

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
                TipoDocumentoId = Guid.Parse("581E3E67-82E2-4F1F-B379-9BD870DB669E"),
            };
            var usernameExist = clienteRepositorio
                            .SearchMatching<ClienteEntity>(x => x.Nombre == dtoCliente.Nombre)
                            .FirstOrDefault();
            if (usernameExist != null || usernameExist != default)
                clienteRepositorio.Delete(usernameExist);

            var response = await clienteService.Insert(dtoCliente).ConfigureAwait(false);
            Assert.NotNull(response.ToString());
            Assert.NotEqual(default, response);

            var id = dtoCliente.Id;
            dtoCliente.Id = Guid.NewGuid();
            dtoCliente.Nombre = "Cliente_fake_Throws_1";
            await Assert.ThrowsAsync<ClienteCodigoTipoDocumentoException>(() => clienteService.Insert(dtoCliente)).ConfigureAwait(false);
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
                TipoDocumentoId = Guid.Parse("12427378-28E4-48CB-8ED7-097116F8064E"),
            };
            var usernameExist2 = clienteRepositorio
                            .SearchMatching<ClienteEntity>(x => x.Nombre == dtoCliente2.Nombre)
                            .FirstOrDefault();
            if (usernameExist2 != null || usernameExist2 != default)
                clienteRepositorio.Delete(usernameExist2);

            var response2 = await clienteService.Insert(dtoCliente2).ConfigureAwait(false);
            Assert.NotNull(response2.ToString());
            Assert.NotEqual(default, response2);

            _ = clienteService.Delete(dtoCliente);
            _ = clienteService.Delete(dtoCliente2);
        }
        #endregion
        //TODO: Cliente, No puede haber dos personas con el mismo nombre / razón social
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
            clienteRepoMock
                .Setup(m => m.GetAll<ClienteEntity>())
                .Returns(Listentity);

            var service = new ServiceCollection();

            service.AddTransient(_ => clienteRepoMock.Object);

            service.ConfigurePersonasService(new DbSettings());

            var provider = service.BuildServiceProvider();
            var clienteService = provider.GetRequiredService<IClienteService>();

            await Assert.ThrowsAsync<ClientenameAlreadyExistException>(() => clienteService.Insert(new ClienteRequestDto
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
        [Fact]
        [IntegrationTest]
        public async void No_Se_Repite_Nombre_Cliente_IntegrationTest()
        {
            var service = new ServiceCollection();

            service.ConfigurePersonasService(new DbSettings
            {
                ConnectionString = "Data Source=DESKTOP-NE15I70\\BDDUVAN;Initial Catalog=evaluacion;User ID=sa;Password=3147073260"
            });

            var providerP = service.BuildServiceProvider();

            var clienteService = providerP.GetRequiredService<IClienteService>();

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
                TipoDocumentoId = Guid.Parse("581E3E67-82E2-4F1F-B379-9BD870DB669E"),
            };
            var response = await clienteService.Insert(dtoCliente).ConfigureAwait(false);
            Assert.NotNull(response.ToString());
            Assert.NotEqual(default, response);

            var id = dtoCliente.Id;
            dtoCliente.Id = Guid.NewGuid();
            await Assert.ThrowsAsync<ClientenameAlreadyExistException>(() => clienteService.Insert(dtoCliente)).ConfigureAwait(false);
            dtoCliente.Id = id;

            _ = clienteService.Delete(dtoCliente);
        }
        #endregion
        //TODO: Cliente, La fecha de nacimiento / creación es obligatoria
        #region Cliente_Validar_Fechas
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
            var clienteDto = new ClienteRequestDto
            {
                Nombre = "FakePrueba",
                TipoPersona = (global::Evaluacion.Aplicacion.Dto.Especificas.Personas.TipoPersona)TipoPersona.Natural,
                FechaNacimiento = DateTimeOffset.Now,
                FechaRegistro = DateTimeOffset.Now,
                TipoDocumentoId = Guid.Parse("581E3E67-82E2-4F1F-B379-9BD870DB669E")
            };

            clienteDto.FechaNacimiento = default;
            await Assert.ThrowsAsync<ClienteFechaNacimientoException>(() => clienteService.Insert(clienteDto)).ConfigureAwait(false);

            clienteDto.FechaNacimiento = DateTimeOffset.Now;
            clienteDto.FechaRegistro = default;
            await Assert.ThrowsAsync<ClienteFechaRegistroException>(() => clienteService.Insert(clienteDto)).ConfigureAwait(false);
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
        [Fact]
        [IntegrationTest]
        public async void Cliente_Validar_Fechas_IntegrationTest()
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
                CodigoTipoDocumento = "000000007",
                TipoPersona = (global::Evaluacion.Aplicacion.Dto.Especificas.Personas.TipoPersona)TipoPersona.Juridico,
                FechaNacimiento = DateTimeOffset.Now,
                FechaRegistro = DateTimeOffset.Now,
                TipoDocumentoId = Guid.Parse("581E3E67-82E2-4F1F-B379-9BD870DB669E"),
            };


            dtoCliente.FechaNacimiento = default;
            await Assert.ThrowsAsync<ClienteFechaNacimientoException>(() => clienteService.Insert(dtoCliente)).ConfigureAwait(false);

            dtoCliente.FechaNacimiento = DateTimeOffset.Now;
            dtoCliente.FechaRegistro = default;
            await Assert.ThrowsAsync<ClienteFechaRegistroException>(() => clienteService.Insert(dtoCliente)).ConfigureAwait(false);

            dtoCliente.FechaRegistro = DateTimeOffset.Now;
            var response = await clienteService.Insert(dtoCliente).ConfigureAwait(false);
            Assert.NotNull(response.ToString());
            Assert.NotEqual(default, response);

            _ = clienteService.Delete(dtoCliente);
        }
        #endregion
        //TODO: Cliente, Una persona no puede tener el tipo de documento nit 
        #region Cliente_Validar_NIT
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
        [Fact]
        [IntegrationTest]
        public async void Cliente_Validar_NIT_IntegrationTest()
        {
            var service = new ServiceCollection();

            service.ConfigurePersonasService(new DbSettings
            {
                ConnectionString = "Data Source=DESKTOP-NE15I70\\BDDUVAN;Initial Catalog=evaluacion;User ID=sa;Password=3147073260"
            });

            var provider = service.BuildServiceProvider();

            var clienteService = provider.GetRequiredService<IClienteService>();

            var dtoCliente = new ClienteRequestDto
            {
                Id = Guid.NewGuid(),
                Nombre = "fake",
                Apellido = "fake",
                NumeroTelefono = 123456789,
                CorreoElectronico = "fake@fake.fake",
                CodigoTipoDocumento = "000000003",
                TipoPersona = (global::Evaluacion.Aplicacion.Dto.Especificas.Personas.TipoPersona)TipoPersona.Juridico,
                FechaNacimiento = DateTimeOffset.Now,
                FechaRegistro = DateTimeOffset.Now,
                TipoDocumentoId = Guid.Parse("581E3E67-82E2-4F1F-B379-9BD870DB669E"),
            };

            dtoCliente.TipoDocumentoId = Guid.Parse("A89DAA40-149F-439A-8A08-7842E09D7376");
            await Assert.ThrowsAsync<ClienteTipoDocumentoException>(() => clienteService.Insert(dtoCliente)).ConfigureAwait(false);

            dtoCliente.TipoDocumentoId = Guid.Parse("581E3E67-82E2-4F1F-B379-9BD870DB669E");
            var response = await clienteService.Insert(dtoCliente).ConfigureAwait(false);
            Assert.NotNull(response.ToString());
            Assert.NotEqual(default, response);

            _ = clienteService.Delete(dtoCliente);
        }
        #endregion
        #region Delect
        [Fact]
        [UnitTest]
        public async Task Cliente_Delete_Test_Fail()
        {
            var clienteRepoMock = new Mock<IClienteRepositorio>();
            clienteRepoMock
                .Setup(m => m.SearchMatchingOneResult(It.IsAny<Expression<Func<ClienteEntity, bool>>>()));

            var service = new ServiceCollection();

            service.AddTransient(_ => clienteRepoMock.Object);

            service.ConfigurePersonasService(new DbSettings());
            var provider = service.BuildServiceProvider();
            var clienteService = provider.GetRequiredService<IClienteService>();

            await Assert.ThrowsAsync<ClienteNoExistException>(() => clienteService.Delete(new ClienteRequestDto { Nombre = "Nombre", Id = Guid.NewGuid() })).ConfigureAwait(false);
        }
        [Fact]
        [UnitTest]
        public async Task Cliente_Delete_Test_Full()
        {
            var clienteRepoMock = new Mock<IClienteRepositorio>();
            var entity = new ClienteEntity
            {
                Id = Guid.NewGuid(),
                Nombre = "Nombre"
            };
            clienteRepoMock
                .Setup(m => m.SearchMatchingOneResult(It.IsAny<Expression<Func<ClienteEntity, bool>>>()))
                .Returns(entity);
            clienteRepoMock
                .Setup(m => m.Delete(It.IsAny<ClienteEntity>()))
                .Returns(true);

            var service = new ServiceCollection();

            service.AddTransient(_ => clienteRepoMock.Object);

            service.ConfigurePersonasService(new DbSettings());
            var provider = service.BuildServiceProvider();
            var clienteService = provider.GetRequiredService<IClienteService>();

            var result = await clienteService.Delete(new ClienteRequestDto
            {
                Id = Guid.NewGuid(),
            }).ConfigureAwait(false);

            Assert.NotNull(result.ToString());
            Assert.True(result);
        }
        [Fact]
        [IntegrationTest]
        public async void Cliente_Delete_Test_IntegrationTest()
        {
            var service = new ServiceCollection();

            service.ConfigurePersonasService(new DbSettings
            {
                ConnectionString = "Data Source=DESKTOP-NE15I70\\BDDUVAN;Initial Catalog=evaluacion;User ID=sa;Password=3147073260"
            });
            service.ConfigureBaseRepository(new DbSettings
            {
                ConnectionString = "Data Source=DESKTOP-NE15I70\\BDDUVAN;Initial Catalog=evaluacion;User ID=sa;Password=3147073260"
            });
            var provider = service.BuildServiceProvider();

            var clienteService = provider.GetRequiredService<IClienteService>();
            var clienteRepositorio = provider.GetRequiredService<IClienteRepositorio>();

            var dtoCliente = new ClienteRequestDto
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
                TipoDocumentoId = Guid.Parse("581E3E67-82E2-4F1F-B379-9BD870DB669E"),
            };

            var cliente = clienteRepositorio
                .SearchMatching<ClienteEntity>(x => x.Nombre == dtoCliente.Nombre || x.Id == dtoCliente.Id)
                .FirstOrDefault();
            if (cliente != null || cliente != default)
                clienteRepositorio.Delete(cliente);

            await clienteService.Insert(dtoCliente).ConfigureAwait(false);

            var dtoCliente2 = new ClienteRequestDto
            {
                Id = dtoCliente.Id
            };
            var result = await clienteService.Delete(dtoCliente2).ConfigureAwait(false);

            Assert.True(result);
        }
        #endregion
        #region Update
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
            clienteRepoMock
                .Setup(m => m.SearchMatchingOneResult(It.IsAny<Expression<Func<ClienteEntity, bool>>>()))
                .Returns(entity);
            clienteRepoMock
                .Setup(m => m.Update(It.IsAny<ClienteEntity>()))
                .Returns(true);

            var service = new ServiceCollection();

            service.AddTransient(_ => clienteRepoMock.Object);

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
            var service = new ServiceCollection();

            service.ConfigurePersonasService(new DbSettings
            {
                ConnectionString = "Data Source=DESKTOP-NE15I70\\BDDUVAN;Initial Catalog=evaluacion;User ID=sa;Password=3147073260"
            });
            service.ConfigureBaseRepository(new DbSettings
            {
                ConnectionString = "Data Source=DESKTOP-NE15I70\\BDDUVAN;Initial Catalog=evaluacion;User ID=sa;Password=3147073260"
            });
            var provider = service.BuildServiceProvider();

            var clienteService = provider.GetRequiredService<IClienteService>();
            var clienteRepositorio = provider.GetRequiredService<IClienteRepositorio>();

            var dtoCliente = new ClienteRequestDto
            {
                Id = Guid.NewGuid(),
                Nombre = "fake",
                Apellido = "fake",
                NumeroTelefono = 123456789,
                CorreoElectronico = "fake@fake.fake",
                CodigoTipoDocumento = "0000000011",
                TipoPersona = (global::Evaluacion.Aplicacion.Dto.Especificas.Personas.TipoPersona)TipoPersona.Natural,
                FechaNacimiento = DateTimeOffset.Now,
                FechaRegistro = DateTimeOffset.Now,
                TipoDocumentoId = Guid.Parse("581E3E67-82E2-4F1F-B379-9BD870DB669E"),
            };

            var cliente = clienteRepositorio
                .SearchMatching<ClienteEntity>(x => x.Nombre == dtoCliente.Nombre || x.Id == dtoCliente.Id)
                .FirstOrDefault();
            if (cliente != null || cliente != default)
                clienteRepositorio.Delete(cliente);

            await clienteService.Insert(dtoCliente).ConfigureAwait(false);

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
                TipoDocumentoId = Guid.Parse("581E3E67-82E2-4F1F-B379-9BD870DB669E"),
            };
            var result = await clienteService.Update(dtoCliente2).ConfigureAwait(false);

            Assert.True(result);

            _ = await clienteService.Delete(dtoCliente).ConfigureAwait(false);
        }
        #endregion
        #region Get
        [Fact]
        [UnitTest]
        public async Task Cliente_Get_Test_Fail()
        {
            var clienteRepoMock = new Mock<IClienteRepositorio>();
            clienteRepoMock
                .Setup(m => m.SearchMatchingOneResult(It.IsAny<Expression<Func<ClienteEntity, bool>>>()));

            var service = new ServiceCollection();

            service.AddTransient(_ => clienteRepoMock.Object);

            service.ConfigurePersonasService(new DbSettings());
            var provider = service.BuildServiceProvider();
            var clienteService = provider.GetRequiredService<IClienteService>();

            await Assert.ThrowsAsync<ClienteNoExistException>(() => clienteService.Get(new ClienteRequestDto { Nombre = "Nombre", Id = Guid.NewGuid() })).ConfigureAwait(false);
        }
        [Fact]
        [UnitTest]
        public async Task Cliente_Get_Test_Full()
        {
            var clienteRepoMock = new Mock<IClienteRepositorio>();
            var entity = new ClienteEntity
            {
                Id = Guid.NewGuid(),
                Nombre = "Nombre"
            };
            clienteRepoMock
                .Setup(m => m.SearchMatchingOneResult(It.IsAny<Expression<Func<ClienteEntity, bool>>>()))
                .Returns(entity);

            var service = new ServiceCollection();

            service.AddTransient(_ => clienteRepoMock.Object);

            service.ConfigurePersonasService(new DbSettings());
            var provider = service.BuildServiceProvider();
            var clienteService = provider.GetRequiredService<IClienteService>();

            var result = await clienteService.Get(new ClienteRequestDto
            {
                Id = Guid.NewGuid(),
            }).ConfigureAwait(false);

            Assert.NotNull(result.ToString());
            Assert.Equal(entity.Id, result.Id);

        }
        [Fact]
        [IntegrationTest]
        public async void Cliente_Get_Test_IntegrationTest()
        {
            var service = new ServiceCollection();

            service.ConfigurePersonasService(new DbSettings
            {
                ConnectionString = "Data Source=DESKTOP-NE15I70\\BDDUVAN;Initial Catalog=evaluacion;User ID=sa;Password=3147073260"
            });
            service.ConfigureBaseRepository(new DbSettings
            {
                ConnectionString = "Data Source=DESKTOP-NE15I70\\BDDUVAN;Initial Catalog=evaluacion;User ID=sa;Password=3147073260"
            });
            var provider = service.BuildServiceProvider();

            var clienteService = provider.GetRequiredService<IClienteService>();
            var clienteRepositorio = provider.GetRequiredService<IClienteRepositorio>();

            var dtoCliente = new ClienteRequestDto
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
                TipoDocumentoId = Guid.Parse("581E3E67-82E2-4F1F-B379-9BD870DB669E"),
            };

            var cliente = clienteRepositorio
                .SearchMatching<ClienteEntity>(x => x.Nombre == dtoCliente.Nombre || x.Id == dtoCliente.Id)
                .FirstOrDefault();
            if (cliente != null || cliente != default)
                clienteRepositorio.Delete(cliente);

            await clienteService.Insert(dtoCliente).ConfigureAwait(false);

            var dtoCliente2 = new ClienteRequestDto
            {
                Id = dtoCliente.Id,
            };
            var result = await clienteService.Get(dtoCliente2).ConfigureAwait(false);

            Assert.Equal(dtoCliente.Id, result.Id);
            _ = await clienteService.Delete(dtoCliente).ConfigureAwait(false);
        }
        #endregion
        #region GetAll
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
            var service = new ServiceCollection();

            service.ConfigurePersonasService(new DbSettings
            {
                ConnectionString = "Data Source=DESKTOP-NE15I70\\BDDUVAN;Initial Catalog=evaluacion;User ID=sa;Password=3147073260"
            });
            service.ConfigureBaseRepository(new DbSettings
            {
                ConnectionString = "Data Source=DESKTOP-NE15I70\\BDDUVAN;Initial Catalog=evaluacion;User ID=sa;Password=3147073260"
            });
            var provider = service.BuildServiceProvider();

            var clienteService = provider.GetRequiredService<IClienteService>();
            var clienteRepositorio = provider.GetRequiredService<IClienteRepositorio>();
            var mapper = provider.GetRequiredService<IMapper>();

            var dtoCliente = new ClienteRequestDto
            {
                Id = Guid.Parse("45c2a9b5-1eac-48d3-83a4-ff692326e4f7"),
                Nombre = "FakeListTipoDocumento1",
                Apellido = "FakeListTipoDocumento1",
                NumeroTelefono = 123456789,
                CorreoElectronico = "fake@fake.fake",
                CodigoTipoDocumento = "000000008",
                TipoPersona = (global::Evaluacion.Aplicacion.Dto.Especificas.Personas.TipoPersona)TipoPersona.Juridico,
                FechaNacimiento = DateTimeOffset.Now,
                FechaRegistro = DateTimeOffset.Now,
                TipoDocumentoId = Guid.Parse("581E3E67-82E2-4F1F-B379-9BD870DB669E"),
            };

            var cliente = clienteRepositorio
                .SearchMatching<ClienteEntity>(x => x.Nombre == dtoCliente.Nombre || x.Id == dtoCliente.Id)
                .FirstOrDefault();
            if (cliente != null || cliente != default)
                clienteRepositorio.Delete(cliente);

            await clienteService.Insert(dtoCliente).ConfigureAwait(false);

            var result = mapper.Map<IEnumerable<ClienteEntity>>(await clienteService.GetAll().ConfigureAwait(false));

            Assert.NotNull(result.ToString());
            Assert.True(result.Any());

            _ = await clienteService.Delete(dtoCliente).ConfigureAwait(false);
        }
        #endregion
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
                CodigoTipoDocumento = "000000001",
                TipoPersona = (global::Evaluacion.Aplicacion.Dto.Especificas.Personas.TipoPersona)TipoPersona.Juridico,
                FechaNacimiento = DateTimeOffset.Now,
                FechaRegistro = DateTimeOffset.Now,
                TipoDocumentoId = Guid.Parse("581E3E67-82E2-4F1F-B379-9BD870DB669E"),
            };
            var response = await clienteService.Insert(dtoCliente).ConfigureAwait(false);
            Assert.NotNull(response.ToString());
            Assert.NotEqual(default, response);

            var id = dtoCliente.Id;
            dtoCliente.Id = Guid.NewGuid();
            await Assert.ThrowsAsync<ClientenameAlreadyExistException>(() => clienteService.Insert(dtoCliente)).ConfigureAwait(false);

            dtoCliente.Nombre = "Fake2";
            await Assert.ThrowsAsync<ClienteCodigoTipoDocumentoException>(() => clienteService.Insert(dtoCliente)).ConfigureAwait(false);
            dtoCliente.Id = id;

            dtoCliente.CodigoTipoDocumento = "000000002";
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
    }
}
