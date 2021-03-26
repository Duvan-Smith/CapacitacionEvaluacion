using AutoMapper;
using Evaluacion.Aplicacion.Core.AdministracionPersonas.Genericas.Areas.Services;
using Evaluacion.Aplicacion.Core.AdministracionPersonas.Genericas.Configuration;
using Evaluacion.Aplicacion.Core.AdministracionPersonas.Personas.Configuration;
using Evaluacion.Aplicacion.Core.AdministracionPersonas.Personas.Empleados.Excepciones;
using Evaluacion.Aplicacion.Core.AdministracionPersonas.Personas.Empleados.Services;
using Evaluacion.Aplicacion.Dto.Especificas.Empleados;
using Evaluacion.Aplicacion.Dto.Genericas.Areas;
using Evaluacion.Dominio.Core.Especificas.Empleados;
using Evaluacion.Dominio.Core.Genericas.Areas;
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

namespace Test.Evaluacion.Core.Aplicacion.Core.AdministracionPersonas.Personas.Empleados
{
    public enum TipoPersona
    {
        Natural = 1,
        Juridico = 2,
    }
    public class EmpleadoServiceTest
    {
        [Fact]
        [UnitTest]
        public async Task Check_AllParameterNull_Empleado_Exception()
        {
            var service = new ServiceCollection();

            service.ConfigurePersonasService(new DbSettings());
            var provider = service.BuildServiceProvider();
            var empleadoService = provider.GetRequiredService<IEmpleadoService>();

            await Assert.ThrowsAsync<EmpleadoRequestDtoNullException>(() => empleadoService.Update(null)).ConfigureAwait(false);
            await Assert.ThrowsAsync<EmpleadoRequestDtoNullException>(() => empleadoService.Delete(null)).ConfigureAwait(false);
            await Assert.ThrowsAsync<EmpleadoRequestDtoNullException>(() => empleadoService.Get(null)).ConfigureAwait(false);
            await Assert.ThrowsAsync<EmpleadoRequestDtoNullException>(() => empleadoService.Insert(null)).ConfigureAwait(false);
        }
        //TODO: Empleado, No puede haber dos personas con el mismo nombre / razón social
        [Fact]
        [UnitTest]
        public async Task No_Se_Repite_Nombre_Empleado_Fail()
        {
            var empleadoRepoMock = new Mock<IEmpleadoRepositorio>();

            empleadoRepoMock
                .Setup(m => m.SearchMatching(It.IsAny<Expression<Func<EmpleadoEntity, bool>>>()))
                .Returns(new List<EmpleadoEntity> { new EmpleadoEntity
                {
                    Id = Guid.NewGuid(),
                    Nombre = "FakePrueba"
                }});

            var service = new ServiceCollection();

            service.AddTransient(_ => empleadoRepoMock.Object);

            service.ConfigurePersonasService(new DbSettings());

            var provider = service.BuildServiceProvider();
            var empleadoService = provider.GetRequiredService<IEmpleadoService>();

            var empleadoDto = new EmpleadoRequestDto
            {
                Nombre = "fake",
                TipoPersona = (global::Evaluacion.Aplicacion.Dto.Especificas.Personas.TipoPersona)TipoPersona.Natural,
                FechaNacimiento = DateTimeOffset.Now,
                FechaRegistro = DateTimeOffset.Now,
                TipoDocumentoId = Guid.Parse("581E3E67-82E2-4F1F-B379-9BD870DB669E")
            };

            await Assert.ThrowsAsync<EmpleadonameAlreadyExistException>(() => empleadoService.Insert(empleadoDto)).ConfigureAwait(false);
        }
        [Fact]
        [UnitTest]
        public async Task No_Se_Repite_Nombre_Empleado_Full()
        {
            var empleadoGetRepoMock = new Mock<IEmpleadoRepositorio>();
            var empleadoInsertRepoMock = new Mock<IEmpleadoRepositorio>();

            empleadoGetRepoMock
                .Setup(m => m.SearchMatching(It.IsAny<Expression<Func<EmpleadoEntity, bool>>>()));
            empleadoInsertRepoMock
                .Setup(m => m.Insert(It.IsAny<EmpleadoEntity>()))
                .Returns(Task.FromResult(new EmpleadoEntity { Id = Guid.NewGuid() }));

            var service = new ServiceCollection();

            service.AddTransient(_ => empleadoGetRepoMock.Object);
            service.AddTransient(_ => empleadoInsertRepoMock.Object);

            service.ConfigurePersonasService(new DbSettings());
            var provider = service.BuildServiceProvider();
            var empleadoService = provider.GetRequiredService<IEmpleadoService>();
            var result = await empleadoService.Insert(new EmpleadoRequestDto
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
        //TODO: Empleado, No puede haber dos personas con el mismo numero y tipo de identificación
        #region Test Funcional
        //Se debe comentar el SearchMatching de nombre para que funcione 
        //[Fact]
        //[UnitTest]
        //public async Task No_Se_Repite_Id_Empleado_Fail()
        //{
        //    var empleadoRepoMock = new Mock<IEmpleadoRepositorio>();

        //    empleadoRepoMock
        //        .Setup(m => m.SearchMatching(It.IsAny<Expression<Func<EmpleadoEntity, bool>>>()))
        //        .Returns(new List<EmpleadoEntity> { new EmpleadoEntity
        //        {
        //            Id = Guid.NewGuid(),
        //            Nombre = "FakePrueba"
        //        }});

        //    var service = new ServiceCollection();

        //    service.AddTransient(_ => empleadoRepoMock.Object);

        //    service.ConfigurePersonasService(new DbSettings());

        //    var provider = service.BuildServiceProvider();
        //    var empleadoService = provider.GetRequiredService<IEmpleadoService>();

        //    var empleadoDto = new EmpleadoRequestDto
        //    {
        //        Nombre = "FakePrueba"
        //    };

        //    await Assert.ThrowsAsync<EmpleadoidAlreadyExistException>(() => empleadoService.Insert(empleadoDto)).ConfigureAwait(false);
        //}
        #endregion
        [Fact]
        [UnitTest]
        public async Task No_Se_Repite_Id_Empleado_Full()
        {
            var empleadoGetRepoMock = new Mock<IEmpleadoRepositorio>();
            var empleadoInsertRepoMock = new Mock<IEmpleadoRepositorio>();

            empleadoGetRepoMock
                .Setup(m => m.SearchMatching(It.IsAny<Expression<Func<EmpleadoEntity, bool>>>()));
            empleadoInsertRepoMock
                .Setup(m => m.Insert(It.IsAny<EmpleadoEntity>()))
                .Returns(Task.FromResult(new EmpleadoEntity { Id = Guid.NewGuid() }));

            var service = new ServiceCollection();

            service.AddTransient(_ => empleadoGetRepoMock.Object);
            service.AddTransient(_ => empleadoInsertRepoMock.Object);

            service.ConfigurePersonasService(new DbSettings());
            var provider = service.BuildServiceProvider();
            var empleadoService = provider.GetRequiredService<IEmpleadoService>();
            var result = await empleadoService.Insert(new EmpleadoRequestDto
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
        //TODO: Empleado, Un empleado no puede ser persona jurídica
        [Fact]
        [UnitTest]
        public async Task Empleado_Tipo_juridica()
        {
            var empleadoGetRepoMock = new Mock<IEmpleadoRepositorio>();
            var empleadoInsertRepoMock = new Mock<IEmpleadoRepositorio>();

            empleadoGetRepoMock
                .Setup(m => m.SearchMatching(It.IsAny<Expression<Func<EmpleadoEntity, bool>>>()));
            empleadoInsertRepoMock
                .Setup(m => m.Insert(It.IsAny<EmpleadoEntity>()))
                .Returns(Task.FromResult(new EmpleadoEntity { Id = Guid.NewGuid() }));

            var service = new ServiceCollection();

            service.AddTransient(_ => empleadoGetRepoMock.Object);
            service.AddTransient(_ => empleadoInsertRepoMock.Object);

            service.ConfigurePersonasService(new DbSettings());
            var provider = service.BuildServiceProvider();
            var empleadoService = provider.GetRequiredService<IEmpleadoService>();

            var empleadoDto = new EmpleadoRequestDto
            {
                Nombre = "FakePrueba",
                TipoPersona = (global::Evaluacion.Aplicacion.Dto.Especificas.Personas.TipoPersona)TipoPersona.Juridico
            };
            Assert.Equal(nameof(TipoPersona.Natural), empleadoDto.TipoPersona.ToString());
            //await Assert.ThrowsAsync<EmpleadoErrorTipoPersonaException>(() => empleadoService.Insert(empleadoDto)).ConfigureAwait(false);
        }
        [Fact]
        [UnitTest]
        public async Task Empleado_no_Tipo_juridica()
        {
            var empleadoGetRepoMock = new Mock<IEmpleadoRepositorio>();
            var empleadoInsertRepoMock = new Mock<IEmpleadoRepositorio>();

            empleadoGetRepoMock
                .Setup(m => m.SearchMatching(It.IsAny<Expression<Func<EmpleadoEntity, bool>>>()));
            empleadoInsertRepoMock
                .Setup(m => m.Insert(It.IsAny<EmpleadoEntity>()))
                .Returns(Task.FromResult(new EmpleadoEntity { Id = Guid.NewGuid() }));

            var service = new ServiceCollection();

            service.AddTransient(_ => empleadoGetRepoMock.Object);
            service.AddTransient(_ => empleadoInsertRepoMock.Object);

            service.ConfigurePersonasService(new DbSettings());
            var provider = service.BuildServiceProvider();
            var empleadoService = provider.GetRequiredService<IEmpleadoService>();
            var result = await empleadoService.Insert(new EmpleadoRequestDto
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
        //TODO: Empleado, La fecha de nacimiento / creación es obligatoria
        [Fact]
        [UnitTest]
        public async Task Empleado_Validar_Fechas_Fail()
        {
            var empleadoGetRepoMock = new Mock<IEmpleadoRepositorio>();
            var empleadoInsertRepoMock = new Mock<IEmpleadoRepositorio>();

            empleadoGetRepoMock
                .Setup(m => m.SearchMatching(It.IsAny<Expression<Func<EmpleadoEntity, bool>>>()));
            empleadoInsertRepoMock
                .Setup(m => m.Insert(It.IsAny<EmpleadoEntity>()))
                .Returns(Task.FromResult(new EmpleadoEntity { Id = Guid.NewGuid() }));

            var service = new ServiceCollection();

            service.AddTransient(_ => empleadoGetRepoMock.Object);
            service.AddTransient(_ => empleadoInsertRepoMock.Object);

            service.ConfigurePersonasService(new DbSettings());
            var provider = service.BuildServiceProvider();
            var empleadoService = provider.GetRequiredService<IEmpleadoService>();

            await Assert.ThrowsAsync<EmpleadoFechaNacimientoException>(() => empleadoService.Insert(new EmpleadoRequestDto
            {
                Nombre = "FakePrueba",
                TipoPersona = (global::Evaluacion.Aplicacion.Dto.Especificas.Personas.TipoPersona)TipoPersona.Natural,
                FechaNacimiento = default,
                FechaRegistro = DateTimeOffset.Now,
                TipoDocumentoId = Guid.Parse("581E3E67-82E2-4F1F-B379-9BD870DB669E")
            })).ConfigureAwait(false);
            await Assert.ThrowsAsync<EmpleadoFechaRegistroException>(() => empleadoService.Insert(new EmpleadoRequestDto
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
        public async Task Empleado_Validar_Fechas_Full()
        {
            var empleadoGetRepoMock = new Mock<IEmpleadoRepositorio>();
            var empleadoInsertRepoMock = new Mock<IEmpleadoRepositorio>();

            empleadoGetRepoMock
                .Setup(m => m.SearchMatching(It.IsAny<Expression<Func<EmpleadoEntity, bool>>>()));
            empleadoInsertRepoMock
                .Setup(m => m.Insert(It.IsAny<EmpleadoEntity>()))
                .Returns(Task.FromResult(new EmpleadoEntity { Id = Guid.NewGuid() }));

            var service = new ServiceCollection();

            service.AddTransient(_ => empleadoGetRepoMock.Object);
            service.AddTransient(_ => empleadoInsertRepoMock.Object);

            service.ConfigurePersonasService(new DbSettings());
            var provider = service.BuildServiceProvider();
            var empleadoService = provider.GetRequiredService<IEmpleadoService>();
            var result = await empleadoService.Insert(new EmpleadoRequestDto
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
        //TODO: Una persona no puede tener el tipo de documento nit 
        [Fact]
        [UnitTest]
        public async Task Empleado_Validar_NIT_Fail()
        {
            var empleadoGetRepoMock = new Mock<IEmpleadoRepositorio>();
            var empleadoInsertRepoMock = new Mock<IEmpleadoRepositorio>();

            empleadoGetRepoMock
                .Setup(m => m.SearchMatching(It.IsAny<Expression<Func<EmpleadoEntity, bool>>>()));
            empleadoInsertRepoMock
                .Setup(m => m.Insert(It.IsAny<EmpleadoEntity>()))
                .Returns(Task.FromResult(new EmpleadoEntity { Id = Guid.NewGuid() }));

            var service = new ServiceCollection();

            service.AddTransient(_ => empleadoGetRepoMock.Object);
            service.AddTransient(_ => empleadoInsertRepoMock.Object);

            service.ConfigurePersonasService(new DbSettings());
            var provider = service.BuildServiceProvider();
            var empleadoService = provider.GetRequiredService<IEmpleadoService>();

            await Assert.ThrowsAsync<EmpleadoTipoDocumentoException>(() => empleadoService.Insert(new EmpleadoRequestDto
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
        public async Task Empleado_Validar_NIT_Full()
        {
            var empleadoGetRepoMock = new Mock<IEmpleadoRepositorio>();
            var empleadoInsertRepoMock = new Mock<IEmpleadoRepositorio>();

            empleadoGetRepoMock
                .Setup(m => m.SearchMatching(It.IsAny<Expression<Func<EmpleadoEntity, bool>>>()));
            empleadoInsertRepoMock
                .Setup(m => m.Insert(It.IsAny<EmpleadoEntity>()))
                .Returns(Task.FromResult(new EmpleadoEntity { Id = Guid.NewGuid() }));

            var service = new ServiceCollection();

            service.AddTransient(_ => empleadoGetRepoMock.Object);
            service.AddTransient(_ => empleadoInsertRepoMock.Object);

            service.ConfigurePersonasService(new DbSettings());
            var provider = service.BuildServiceProvider();
            var empleadoService = provider.GetRequiredService<IEmpleadoService>();

            await Assert.ThrowsAsync<EmpleadoFechaNacimientoException>(() => empleadoService.Insert(new EmpleadoRequestDto
            {
                Nombre = "FakePrueba",
                TipoPersona = (global::Evaluacion.Aplicacion.Dto.Especificas.Personas.TipoPersona)TipoPersona.Natural,
                FechaNacimiento = default,
                FechaRegistro = DateTimeOffset.Now,
                TipoDocumentoId = Guid.Parse("581E3E67-82E2-4F1F-B379-9BD870DB669E")
            })).ConfigureAwait(false);
        }
        //TODO: Test de integracion para empleado
        [Fact]
        [IntegrationTest]
        public async void No_Se_Repite_Nombre_Empleado_Integration()
        {
            var service = new ServiceCollection();
            var serviceP = new ServiceCollection();

            service.ConfigureGenericasService(new DbSettings
            {
                ConnectionString = "Data Source=DESKTOP-NE15I70\\BDDUVAN;Initial Catalog=evaluacion;User ID=sa;Password=3147073260"
            });
            serviceP.ConfigurePersonasService(new DbSettings
            {
                ConnectionString = "Data Source=DESKTOP-NE15I70\\BDDUVAN;Initial Catalog=evaluacion;User ID=sa;Password=3147073260"
            });

            var provider = service.BuildServiceProvider();
            var providerP = serviceP.BuildServiceProvider();

            var areaService = provider.GetRequiredService<IAreaService>();
            var empleadoService = providerP.GetRequiredService<IEmpleadoService>();
            var areaRepositorio = providerP.GetRequiredService<IAreaRepositorio>();
            var mapper = providerP.GetRequiredService<IMapper>();

            var dtoArea = new AreaRequestDto
            {
                Id = Guid.NewGuid(),
                NombreArea = "FakeArea",
                EmpleadoResponsableId = Guid.NewGuid()
            };
            var area = areaRepositorio
                .SearchMatching<AreaEntity>(x => x.NombreArea == dtoArea.NombreArea)
                .FirstOrDefault();
            areaRepositorio.Delete(area);
            var guidArea = await areaService.Insert(dtoArea).ConfigureAwait(false);

            var dtoEmpleado = new EmpleadoRequestDto
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
                Salario = 20000,
                AreaId = guidArea
            };
            var response = await empleadoService.Insert(dtoEmpleado).ConfigureAwait(false);
            Assert.NotNull(response.ToString());
            Assert.NotEqual(default, response);

            await Assert.ThrowsAsync<EmpleadonameAlreadyExistException>(() => empleadoService.Insert(dtoEmpleado)).ConfigureAwait(false);

            dtoEmpleado.Nombre = "Fake2";
            await Assert.ThrowsAsync<EmpleadoCodigoTipoDocumentoException>(() => empleadoService.Insert(dtoEmpleado)).ConfigureAwait(false);

            dtoEmpleado.CodigoTipoDocumento = "345678912";
            dtoEmpleado.FechaNacimiento = default;
            await Assert.ThrowsAsync<EmpleadoFechaNacimientoException>(() => empleadoService.Insert(dtoEmpleado)).ConfigureAwait(false);

            dtoEmpleado.FechaNacimiento = DateTimeOffset.Now;
            dtoEmpleado.FechaRegistro = default;
            await Assert.ThrowsAsync<EmpleadoFechaRegistroException>(() => empleadoService.Insert(dtoEmpleado)).ConfigureAwait(false);

            dtoEmpleado.FechaRegistro = DateTimeOffset.Now;
            dtoEmpleado.TipoDocumentoId = Guid.Parse("A89DAA40-149F-439A-8A08-7842E09D7376");
            await Assert.ThrowsAsync<EmpleadoTipoDocumentoException>(() => empleadoService.Insert(dtoEmpleado)).ConfigureAwait(false);

            _ = empleadoService.Delete(dtoEmpleado);
            var areaEnd = areaRepositorio
                .SearchMatching<AreaEntity>(x => x.NombreArea == dtoArea.NombreArea)
                .FirstOrDefault();
            areaRepositorio.Delete(areaEnd);
        }
        [Fact]
        [IntegrationTest]
        public async void Validar_Identificacion_Tipo_Empleado_Integration()
        {
            var service = new ServiceCollection();
            var serviceP = new ServiceCollection();

            service.ConfigureGenericasService(new DbSettings
            {
                ConnectionString = "Data Source=DESKTOP-NE15I70\\BDDUVAN;Initial Catalog=evaluacion;User ID=sa;Password=3147073260"
            });
            serviceP.ConfigurePersonasService(new DbSettings
            {
                ConnectionString = "Data Source=DESKTOP-NE15I70\\BDDUVAN;Initial Catalog=evaluacion;User ID=sa;Password=3147073260"
            });

            var provider = service.BuildServiceProvider();
            var providerP = serviceP.BuildServiceProvider();

            var areaService = provider.GetRequiredService<IAreaService>();
            var empleadoService = providerP.GetRequiredService<IEmpleadoService>();
            var areaRepositorio = providerP.GetRequiredService<IAreaRepositorio>();
            var mapper = providerP.GetRequiredService<IMapper>();

            var dtoArea = new AreaRequestDto
            {
                Id = Guid.NewGuid(),
                NombreArea = "FakeArea",
                EmpleadoResponsableId = Guid.NewGuid()
            };
            var area = areaRepositorio
                .SearchMatching<AreaEntity>(x => x.NombreArea == dtoArea.NombreArea)
                .FirstOrDefault();
            areaRepositorio.Delete(area);
            var guidArea = await areaService.Insert(dtoArea).ConfigureAwait(false);

            var dtoEmpleado = new EmpleadoRequestDto
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
                Salario = 20000,
                AreaId = guidArea
            };
            var response = await empleadoService.Insert(dtoEmpleado).ConfigureAwait(false);
            Assert.NotNull(response.ToString());
            Assert.NotEqual(default, response);

            var dtoEmpleadoI2 = new EmpleadoRequestDto
            {
                Id = Guid.NewGuid(),
                Nombre = "fake2",
                Apellido = "fake2",
                NumeroTelefono = 223456789,
                CorreoElectronico = "fake2@fake2.fake2",
                CodigoTipoDocumento = "123456789",
                TipoPersona = (global::Evaluacion.Aplicacion.Dto.Especificas.Personas.TipoPersona)TipoPersona.Natural,
                FechaNacimiento = DateTimeOffset.Now,
                FechaRegistro = DateTimeOffset.Now,
                TipoDocumentoId = Guid.Parse("12427378-28E4-48CB-8ED7-097116F8064E"),
                Salario = 20000,
                AreaId = guidArea
            };

            var responseI2 = await empleadoService.Insert(dtoEmpleadoI2).ConfigureAwait(false);
            Assert.NotNull(responseI2.ToString());
            Assert.NotEqual(default, responseI2);


            _ = empleadoService.Delete(dtoEmpleado);
            _ = empleadoService.Delete(dtoEmpleadoI2);
            var areaEnd = areaRepositorio
                .SearchMatching<AreaEntity>(x => x.NombreArea == dtoArea.NombreArea)
                .FirstOrDefault();
            areaRepositorio.Delete(areaEnd);
        }
    }
}
