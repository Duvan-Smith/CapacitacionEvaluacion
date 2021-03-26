using AutoMapper;
using Evaluacion.Aplicacion.Core.AdministracionPersonas.Genericas.Areas.Excepciones;
using Evaluacion.Aplicacion.Core.AdministracionPersonas.Genericas.Areas.Services;
using Evaluacion.Aplicacion.Core.AdministracionPersonas.Genericas.Configuration;
using Evaluacion.Aplicacion.Core.AdministracionPersonas.Personas.Configuration;
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

namespace Test.Evaluacion.Core.Aplicacion.Core.AdministracionPersonas.Genericas.Areas
{
    public class AreaServiceTest
    {
        [Fact]
        [UnitTest]
        public async Task Check_AllParameterNull_Area_Exception()
        {
            var service = new ServiceCollection();
            service.ConfigureGenericasService(new DbSettings());
            var provider = service.BuildServiceProvider();
            var areaService = provider.GetRequiredService<IAreaService>();
            await Assert.ThrowsAsync<AreaRequestDtoNullException>(() => areaService.Update(null)).ConfigureAwait(false);
            await Assert.ThrowsAsync<AreaRequestDtoNullException>(() => areaService.Delete(null)).ConfigureAwait(false);
            await Assert.ThrowsAsync<AreaRequestDtoNullException>(() => areaService.Get(null)).ConfigureAwait(false);
            await Assert.ThrowsAsync<AreaRequestDtoNullException>(() => areaService.Insert(null)).ConfigureAwait(false);
        }
        //TODO: Area, No se pueden eliminar áreas que tengan empleados asociados
        [Fact]
        [UnitTest]
        public async Task No_se_pueden_eliminar_areas_que_tengan_empleados_asociados()
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

            await Assert.ThrowsAsync<EmpleadoAreaAlreadyExistException>(() => areaService.Delete(new AreaRequestDto())).ConfigureAwait(false);
        }
        [Fact]
        [UnitTest]
        public async Task Si_se_pueden_eliminar_areas_que_no_tengan_empleados_asociados()
        {
            var empleadoGetRepoMock = new Mock<IEmpleadoRepositorio>();
            var areaDeleteRepoMock = new Mock<IAreaRepositorio>();

            empleadoGetRepoMock
                .Setup(m => m.SearchMatching(It.IsAny<Expression<Func<EmpleadoEntity, bool>>>()));
            areaDeleteRepoMock
                .Setup(m => m.Delete(It.IsAny<AreaEntity>()))
                .Returns(true);

            var service = new ServiceCollection();

            service.AddTransient(_ => empleadoGetRepoMock.Object);
            service.AddTransient(_ => areaDeleteRepoMock.Object);

            service.ConfigureGenericasService(new DbSettings());
            var provider = service.BuildServiceProvider();
            var areaService = provider.GetRequiredService<IAreaService>();

            Assert.True(await areaService.Delete(new AreaRequestDto()).ConfigureAwait(false));
        }
        [Fact]
        [IntegrationTest]
        public void Eliminar_areas_que_no_tengan_empleados_asociados()
        {
            var service = new ServiceCollection();
            service.ConfigureGenericasService(new DbSettings
            {
                ConnectionString = "Data Source=DESKTOP-NE15I70\\BDDUVAN;Initial Catalog=evaluacion;User ID=sa;Password=3147073260"
            });
            var provider = service.BuildServiceProvider();
            var areaService = provider.GetRequiredService<IAreaService>();

            var dtoArea = new AreaRequestDto
            {
                Id = Guid.NewGuid(),
                NombreArea = "FakeAreaOk",
                EmpleadoResponsableId = Guid.NewGuid()
            };
            areaService.Insert(dtoArea);

            var response = areaService.Delete(dtoArea);

            Assert.NotNull(response);
            Assert.True(response.Result);
            Assert.NotEqual(default, response);
        }
        [Fact]
        [IntegrationTest]
        public async void Eliminar_areas_que_tengan_empleados_asociados()
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
            var empleado = areaRepositorio
                .SearchMatching<AreaEntity>(x => x.NombreArea == dtoArea.NombreArea)
                .FirstOrDefault();
            //var entity = mapper.Map<AreaEntity>(dtoArea);
            areaRepositorio.Delete(empleado);
            await areaService.Insert(dtoArea).ConfigureAwait(false);

            var dtoEmpleado = new EmpleadoRequestDto
            {
                Id = Guid.NewGuid(),
                Nombre = "fake",
                Apellido = "Fake",
                FechaNacimiento = DateTimeOffset.Now,
                FechaRegistro = DateTimeOffset.Now,
                NumeroTelefono = 0,
                CorreoElectronico = "fake@fake.fake",
                TipoDocumentoId = Guid.Parse("581e3e67-82e2-4f1f-b379-9bd870db669e"),
                CodigoTipoDocumento = "12345678",
                Salario = 20000,
                AreaId = dtoArea.Id
            };

            await empleadoService.Insert(dtoEmpleado).ConfigureAwait(false);

            await Assert.ThrowsAsync<EmpleadoAreaAlreadyExistException>(() => areaService.Delete(dtoArea)).ConfigureAwait(false);

            await empleadoService.Delete(dtoEmpleado).ConfigureAwait(false);
            var empleadoEnd = areaRepositorio
                .SearchMatching<AreaEntity>(x => x.NombreArea == dtoArea.NombreArea)
                .FirstOrDefault();
            areaRepositorio.Delete(empleadoEnd);
        }
        [Fact]
        [IntegrationTest]
        public async void No_Test_Get_Tabla_Relaciones()
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

            var dtoArea = new AreaRequestDto
            {
                Id = Guid.NewGuid(),
                NombreArea = "FakeListEmpleado",
                EmpleadoResponsableId = Guid.NewGuid()
            };
            await areaService.Insert(dtoArea).ConfigureAwait(false);

            var dtoEmpleado = new EmpleadoRequestDto
            {
                Id = Guid.NewGuid(),
                Nombre = "fake",
                Apellido = "Fake",
                FechaNacimiento = DateTimeOffset.Now,
                FechaRegistro = DateTimeOffset.Now,
                NumeroTelefono = 0,
                CorreoElectronico = "fake@fake.fake",
                TipoDocumentoId = Guid.Parse("581e3e67-82e2-4f1f-b379-9bd870db669e"),
                CodigoTipoDocumento = "12345678",
                Salario = 20000,
                AreaId = dtoArea.Id
            };
            await empleadoService.Insert(dtoEmpleado).ConfigureAwait(false);

            await areaService.GetAll().ConfigureAwait(false);

            await empleadoService.Delete(dtoEmpleado).ConfigureAwait(false);
            await areaService.Delete(dtoArea).ConfigureAwait(false);
        }
        //TODO: Hacer primero Empleado para hacer las llamadas desde area

    }
}
