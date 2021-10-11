using AutoMapper;
using Evaluacion.Aplicacion.Core.AdministracionPersonas.Genericas.Areas.Excepciones;
using Evaluacion.Aplicacion.Core.AdministracionPersonas.Genericas.Areas.Services;
using Evaluacion.Aplicacion.Core.AdministracionPersonas.Genericas.Configuration;
using Evaluacion.Aplicacion.Core.AdministracionPersonas.Genericas.TipoDocumentos.Services;
using Evaluacion.Aplicacion.Core.AdministracionPersonas.Personas.Configuration;
using Evaluacion.Aplicacion.Core.AdministracionPersonas.Personas.Empleados.Services;
using Evaluacion.Aplicacion.Dto.Especificas.Empleados;
using Evaluacion.Aplicacion.Dto.Genericas.Areas;
using Evaluacion.Aplicacion.Dto.Genericas.TipoDocumentos;
using Evaluacion.Dominio.Core.Especificas.Empleados;
using Evaluacion.Dominio.Core.Genericas.Areas;
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

namespace Test.Evaluacion.Core.Aplicacion.Core.AdministracionPersonas.Genericas.Areas
{
    public class AreaServiceDeleteTest
    {
        private static ServiceProvider ServiceCollectionArea()
        {
            var service = new ServiceCollection();

            service.ConfigureGenericasService(new DbSettings
            {
                ConnectionString = "Data Source=DSMITHMR;Initial Catalog=prueba;Integrated Security=True"
            });
            service.ConfigurePersonasService(new DbSettings
            {
                ConnectionString = "Data Source=DSMITHMR;Initial Catalog=prueba;Integrated Security=True"
            });

            var provider = service.BuildServiceProvider();
            return provider;
        }
        [Fact]
        [UnitTest]
        public async Task Check_AllParameterNull_AreaDelete_Exception()
        {
            var service = new ServiceCollection();
            service.ConfigureGenericasService(new DbSettings());
            var provider = service.BuildServiceProvider();
            var areaService = provider.GetRequiredService<IAreaService>();
            _ = await Assert.ThrowsAsync<AreaRequestDtoNullException>(() => areaService.Update(null)).ConfigureAwait(false);
            _ = await Assert.ThrowsAsync<AreaRequestDtoNullException>(() => areaService.Delete(null)).ConfigureAwait(false);
            _ = await Assert.ThrowsAsync<AreaRequestDtoNullException>(() => areaService.Get(null)).ConfigureAwait(false);
            _ = await Assert.ThrowsAsync<AreaRequestDtoNullException>(() => areaService.Insert(null)).ConfigureAwait(false);
        }
        //Area, No se pueden eliminar áreas que tengan empleados asociados
        #region No_se_pueden_eliminar_areas_que_tengan_empleados_asociados
        [Fact]
        [UnitTest]
        public async Task No_Eliminar_areas_que_tengan_empleados_asociados_Fail()
        {
            var empleadoRepoMock = new Mock<IEmpleadoRepositorio>();
            empleadoRepoMock
                .Setup(m => m.SearchMatching(It.IsAny<Expression<Func<EmpleadoEntity, bool>>>()))
                .Returns(new List<EmpleadoEntity> { new EmpleadoEntity
                {
                    Id=Guid.NewGuid(),
                    Nombre="fake"
                }});

            var service = new ServiceCollection();

            service.AddTransient(_ => empleadoRepoMock.Object);

            service.ConfigureGenericasService(new DbSettings());
            var provider = service.BuildServiceProvider();
            var areaService = provider.GetRequiredService<IAreaService>();

            await Assert.ThrowsAsync<EmpleadoAreaAlreadyExistException>(() => areaService.Delete(new AreaRequestDto
            {
                Id = Guid.NewGuid(),
                NombreArea = "Fake_Area_1",
                EmpleadoResponsableId = Guid.NewGuid()
            })).ConfigureAwait(false);
        }
        [Fact]
        [UnitTest]
        public async Task No_Eliminar_areas_que_tengan_empleados_asociados_Full()
        {
            var empleadoReposMock = new Mock<IEmpleadoRepositorio>();
            var areaDeleteRepoMock = new Mock<IAreaRepositorio>();

            empleadoReposMock
                .Setup(m => m.SearchMatching(It.IsAny<Expression<Func<EmpleadoEntity, bool>>>()));
            areaDeleteRepoMock
                .Setup(m => m.Delete(It.IsAny<AreaEntity>()))
                .Returns(Task.FromResult(true));

            var service = new ServiceCollection();

            service.AddTransient(_ => empleadoReposMock.Object);
            service.AddTransient(_ => areaDeleteRepoMock.Object);

            service.ConfigureGenericasService(new DbSettings());
            var provider = service.BuildServiceProvider();
            var areaService = provider.GetRequiredService<IAreaService>();

            Assert.True(await areaService.Delete(new AreaRequestDto
            {
                Id = Guid.NewGuid(),
                NombreArea = "Fake_Area_1",
                EmpleadoResponsableId = Guid.NewGuid()
            }).ConfigureAwait(false));
        }
        [Fact]
        [IntegrationTest]
        public async void No_Eliminar_areas_que_tengan_empleados_asociados_Full_IntegrationTest()
        {
            ServiceProvider provider = ServiceCollectionArea();

            var areaService = provider.GetRequiredService<IAreaService>();

            var dtoArea = new AreaRequestDto
            {
                Id = Guid.NewGuid(),
                NombreArea = "FakeAreaOk",
                EmpleadoResponsableId = Guid.NewGuid()
            };
            _ = await areaService.Insert(dtoArea).ConfigureAwait(false);

            bool response = await areaService.Delete(dtoArea).ConfigureAwait(false);

            Assert.NotEqual(default, response);
            Assert.True(response);
        }
        [Fact]
        [IntegrationTest]
        public async void No_Eliminar_areas_que_tengan_empleados_asociados_Fail_IntegrationTest()
        {
            ServiceProvider provider = ServiceCollectionArea();

            var areaService = provider.GetRequiredService<IAreaService>();
            var empleadoService = provider.GetRequiredService<IEmpleadoService>();
            var areaRepositorio = provider.GetRequiredService<IAreaRepositorio>();
            var mapper = provider.GetRequiredService<IMapper>();
            var documentoService = provider.GetRequiredService<ITipoDocumentoService>();
            var documentoRepo = provider.GetRequiredService<ITipoDocumentoRepositorio>();

            var dtoDocumento = new TipoDocumentoRequestDto
            {
                Id = Guid.NewGuid(),
                NombreTipoDocumento = "FakeCEDULAFake",
            };
            var documento = documentoRepo
                .SearchMatching<TipoDocumentoEntity>(x => x.NombreTipoDocumento == dtoDocumento.NombreTipoDocumento || x.Id == dtoDocumento.Id)
                .FirstOrDefault();
            if (documento != null || documento != default)
                _ = await documentoRepo.Delete(documento).ConfigureAwait(false);

            _ = await documentoService.Insert(dtoDocumento).ConfigureAwait(false);

            var dtoArea = new AreaRequestDto
            {
                Id = Guid.NewGuid(),
                NombreArea = "FakeArea3",
                EmpleadoResponsableId = Guid.NewGuid()
            };
            var empleado = areaRepositorio
                .SearchMatching<AreaEntity>(x => x.NombreArea == dtoArea.NombreArea)
                .FirstOrDefault();
            _ = await areaRepositorio.Delete(empleado).ConfigureAwait(false);
            _ = await areaService.Insert(dtoArea).ConfigureAwait(false);

            var dtoEmpleado = new EmpleadoRequestDto
            {
                Id = Guid.NewGuid(),
                Nombre = "Fake_Empleado_Area_1",
                Apellido = "Fake_Empleado_Area_1",
                FechaNacimiento = DateTimeOffset.Now,
                FechaRegistro = DateTimeOffset.Now,
                NumeroTelefono = 0,
                CorreoElectronico = "fake@fake.fake",
                TipoDocumentoId = dtoDocumento.Id,
                CodigoTipoDocumento = "12345678",
                Salario = 20000,
                AreaId = dtoArea.Id,
                CodigoEmpleado = "Prueba21"
            };

            _ = await empleadoService.Insert(dtoEmpleado).ConfigureAwait(false);

            _ = await Assert.ThrowsAsync<EmpleadoAreaAlreadyExistException>(() => areaService.Delete(dtoArea)).ConfigureAwait(false);

            _ = await empleadoService.Delete(dtoEmpleado).ConfigureAwait(false);
            var empleadoEnd = areaRepositorio
                .SearchMatching<AreaEntity>(x => x.NombreArea == dtoArea.NombreArea)
                .FirstOrDefault();
            _ = await areaRepositorio.Delete(empleadoEnd).ConfigureAwait(false);
            _ = await documentoService.Delete(dtoDocumento).ConfigureAwait(false);
        }
        #endregion
        //Las área deben tener una persona encargada
        #region Empleado_Encargado_Area
        [Fact]
        [UnitTest]
        public async Task Empleado_Encargado_Area_Fail()
        {
            var empleadoRepoMock = new Mock<IEmpleadoRepositorio>();
            empleadoRepoMock
                .Setup(m => m.SearchMatching(It.IsAny<Expression<Func<EmpleadoEntity, bool>>>()));

            var service = new ServiceCollection();

            service.AddTransient(_ => empleadoRepoMock.Object);

            service.ConfigureGenericasService(new DbSettings());
            var provider = service.BuildServiceProvider();
            var areaService = provider.GetRequiredService<IAreaService>();

            await Assert.ThrowsAsync<AreaEmpleadoResponsableIdNullException>(() => areaService.Delete(new AreaRequestDto
            {
                Id = Guid.NewGuid(),
                NombreArea = "Fake_Area_1",
                EmpleadoResponsableId = Guid.Empty
            })).ConfigureAwait(false);
        }
        [Fact]
        [UnitTest]
        public async Task Empleado_Encargado_Area_Full()
        {
            var empleadoReposMock = new Mock<IEmpleadoRepositorio>();
            var areaDeleteRepoMock = new Mock<IAreaRepositorio>();

            empleadoReposMock
                .Setup(m => m.SearchMatching(It.IsAny<Expression<Func<EmpleadoEntity, bool>>>()));
            areaDeleteRepoMock
                .Setup(m => m.Delete(It.IsAny<AreaEntity>()))
                .Returns(Task.FromResult(true));

            var service = new ServiceCollection();

            service.AddTransient(_ => empleadoReposMock.Object);
            service.AddTransient(_ => areaDeleteRepoMock.Object);

            service.ConfigureGenericasService(new DbSettings());
            var provider = service.BuildServiceProvider();
            var areaService = provider.GetRequiredService<IAreaService>();

            Assert.True(await areaService.Delete(new AreaRequestDto
            {
                Id = Guid.NewGuid(),
                NombreArea = "Fake_Area_1",
                EmpleadoResponsableId = Guid.NewGuid()
            }).ConfigureAwait(false));
        }
        [Fact]
        [IntegrationTest]
        public async void Empleado_Encargado_Area_IntegrationTest()
        {
            ServiceProvider provider = ServiceCollectionArea();

            var areaService = provider.GetRequiredService<IAreaService>();

            var dtoArea = new AreaRequestDto
            {
                Id = Guid.NewGuid(),
                NombreArea = "FakeAreaOk",
                EmpleadoResponsableId = Guid.Empty
            };

            _ = await Assert.ThrowsAsync<AreaEmpleadoResponsableIdNullException>(() => areaService.Delete(dtoArea)).ConfigureAwait(false);

            dtoArea.EmpleadoResponsableId = Guid.NewGuid();
            _ = await areaService.Insert(dtoArea).ConfigureAwait(false);

            bool response = await areaService.Delete(dtoArea).ConfigureAwait(false);

            Assert.NotEqual(default, response);
            Assert.True(response);
        }
        #endregion
    }
}
