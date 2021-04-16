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
    public class AreaServiceInsertTest
    {
        private static ServiceProvider ServiceCollectionArea()
        {
            var service = new ServiceCollection();

            service.ConfigureGenericasService(new DbSettings
            {
                ConnectionString = "Data Source=DSMITH;Initial Catalog=evaluacion;Integrated Security=True"
            });
            service.ConfigurePersonasService(new DbSettings
            {
                ConnectionString = "Data Source=DSMITH;Initial Catalog=evaluacion;Integrated Security=True"
            });

            var provider = service.BuildServiceProvider();
            return provider;
        }
        [Fact]
        [UnitTest]
        public async Task Check_AllParameterNull_AreaInsert_Exception()
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
        #region Insert
        [Fact]
        [UnitTest]
        public async Task Area_Insert_Test_Fail()
        {
            var areaRepoMock = new Mock<IAreaRepositorio>();
            areaRepoMock
                .Setup(m => m.SearchMatching(It.IsAny<Expression<Func<AreaEntity, bool>>>()))
                .Returns(new List<AreaEntity> { new AreaEntity
                {
                    Id=Guid.NewGuid(),
                }});

            var service = new ServiceCollection();

            service.AddTransient(_ => areaRepoMock.Object);

            service.ConfigureGenericasService(new DbSettings());
            var provider = service.BuildServiceProvider();
            var areaService = provider.GetRequiredService<IAreaService>();

            await Assert.ThrowsAsync<AreanameAlreadyExistException>(() => areaService.Insert(new AreaRequestDto { EmpleadoResponsableId = Guid.NewGuid() })).ConfigureAwait(false);
        }
        [Fact]
        [UnitTest]
        public async Task Area_Insert_Test_Full()
        {
            var areaRepoMock = new Mock<IAreaRepositorio>();
            var areaInsertRepoMock = new Mock<IAreaRepositorio>();

            areaRepoMock
                .Setup(m => m.SearchMatching(It.IsAny<Expression<Func<AreaEntity, bool>>>()));
            areaInsertRepoMock
                .Setup(m => m.Insert(It.IsAny<AreaEntity>()))
                .Returns(Task.FromResult(new AreaEntity { Id = Guid.NewGuid() }));

            var service = new ServiceCollection();

            service.AddTransient(_ => areaRepoMock.Object);
            service.AddTransient(_ => areaInsertRepoMock.Object);

            service.ConfigureGenericasService(new DbSettings());
            var provider = service.BuildServiceProvider();
            var areaService = provider.GetRequiredService<IAreaService>();

            var result = await areaService.Insert(new AreaRequestDto { EmpleadoResponsableId = Guid.NewGuid() }).ConfigureAwait(false);

            Assert.NotNull(result.ToString());
            Assert.NotEqual(default, result);

        }
        [Fact]
        [IntegrationTest]
        public async void Area_Insert_Test_IntegrationTest()
        {
            ServiceProvider provider = ServiceCollectionArea();

            var areaService = provider.GetRequiredService<IAreaService>();

            var dtoArea = new AreaRequestDto
            {
                Id = Guid.NewGuid(),
                NombreArea = "FakeListEmpleado",
                EmpleadoResponsableId = Guid.NewGuid()
            };
            var result = await areaService.Insert(dtoArea).ConfigureAwait(false);

            Assert.NotEqual(default, result);
            Assert.Equal(dtoArea.Id, result);
            _ = await Assert.ThrowsAsync<AreanameAlreadyExistException>(() => areaService.Insert(dtoArea)).ConfigureAwait(false);
            _ = await areaService.Delete(dtoArea).ConfigureAwait(false);
        }
        #endregion
        [Fact]
        [IntegrationTest]
        public async void No_Test_Get_Tabla_RelacionesAreas()
        {
            ServiceProvider provider = ServiceCollectionArea();

            var areaService = provider.GetRequiredService<IAreaService>();
            var areaRepo = provider.GetRequiredService<IAreaRepositorio>();
            var empleadoService = provider.GetRequiredService<IEmpleadoService>();
            var empleadoRepo = provider.GetRequiredService<IEmpleadoRepositorio>();
            var documentoService = provider.GetRequiredService<ITipoDocumentoService>();
            var documentoRepo = provider.GetRequiredService<ITipoDocumentoRepositorio>();

            var dtoDocumento = new TipoDocumentoRequestDto
            {
                Id = Guid.NewGuid(),
                NombreTipoDocumento = "fakeCEDULAfakeAreaInsert",
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
                NombreArea = "FakeListEmpleado",
                EmpleadoResponsableId = Guid.NewGuid()
            };
            var area = areaRepo
                .SearchMatching<AreaEntity>(x => x.NombreArea == dtoArea.NombreArea || x.Id == dtoArea.Id)
                .FirstOrDefault();
            if (area != null || area != default)
                _ = await areaRepo.Delete(area).ConfigureAwait(false);

            _ = await areaService.Insert(dtoArea).ConfigureAwait(false);

            var dtoEmpleado = new EmpleadoRequestDto
            {
                Id = Guid.NewGuid(),
                Nombre = "Fake_Empleado_Area_2",
                Apellido = "Fake_Empleado_Area_2",
                FechaNacimiento = DateTimeOffset.Now,
                FechaRegistro = DateTimeOffset.Now,
                NumeroTelefono = 0,
                CorreoElectronico = "fake@fake.fake",
                TipoDocumentoId = dtoDocumento.Id,
                CodigoTipoDocumento = "12345678",
                Salario = 20000,
                AreaId = dtoArea.Id,
                CodigoEmpleado = "Prueba21",
            };

            var empleado = empleadoRepo
                .SearchMatching<EmpleadoEntity>(x => x.Nombre == dtoEmpleado.Nombre || x.Id == dtoEmpleado.Id)
                .FirstOrDefault();
            if (empleado != null || empleado != default)
                _ = await empleadoRepo.Delete(empleado).ConfigureAwait(false);

            _ = await empleadoService.Insert(dtoEmpleado).ConfigureAwait(false);

            var dtoArea2 = new AreaRequestDto
            {
                Id = dtoArea.Id,
                EmpleadoResponsableId = Guid.NewGuid()
            };
            _ = await areaService.Update(dtoArea2).ConfigureAwait(false);

            _ = await areaService.GetAll().ConfigureAwait(false);

            _ = await empleadoService.Delete(dtoEmpleado).ConfigureAwait(false);
            _ = await areaService.Delete(dtoArea).ConfigureAwait(false);
            _ = await documentoService.Delete(dtoDocumento).ConfigureAwait(false);
        }
    }
}
