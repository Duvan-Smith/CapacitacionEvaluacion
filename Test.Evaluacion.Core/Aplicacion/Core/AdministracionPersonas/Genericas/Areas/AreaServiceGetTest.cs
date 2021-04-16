using Evaluacion.Aplicacion.Core.AdministracionPersonas.Genericas.Areas.Excepciones;
using Evaluacion.Aplicacion.Core.AdministracionPersonas.Genericas.Areas.Services;
using Evaluacion.Aplicacion.Core.AdministracionPersonas.Genericas.Configuration;
using Evaluacion.Aplicacion.Core.AdministracionPersonas.Personas.Configuration;
using Evaluacion.Aplicacion.Dto.Genericas.Areas;
using Evaluacion.Dominio.Core.Genericas.Areas;
using Evaluacion.Infraestructura.Datos.Persistencia.Core.Base.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;
using Xunit.Categories;

namespace Test.Evaluacion.Core.Aplicacion.Core.AdministracionPersonas.Genericas.Areas
{
    public class AreaServiceGetTest
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
        public async Task Check_AllParameterNull_AreaGet_Exception()
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
        #region Get
        [Fact]
        [UnitTest]
        public async Task Area_Get_Test_Fail()
        {
            var areaRepoMock = new Mock<IAreaRepositorio>();
            areaRepoMock
                .Setup(m => m.SearchMatchingOneResult(It.IsAny<Expression<Func<AreaEntity, bool>>>()));

            var service = new ServiceCollection();

            service.AddTransient(_ => areaRepoMock.Object);

            service.ConfigureGenericasService(new DbSettings());
            var provider = service.BuildServiceProvider();
            var areaService = provider.GetRequiredService<IAreaService>();

            await Assert.ThrowsAsync<AreaNoExistException>(() => areaService.Get(new AreaRequestDto
            {
                NombreArea = "NombreArea",
                Id = Guid.NewGuid(),
                EmpleadoResponsableId = Guid.NewGuid()
            })).ConfigureAwait(false);
        }
        [Fact]
        [UnitTest]
        public async Task Area_Get_Test_Full()
        {
            var areaRepoMock = new Mock<IAreaRepositorio>();
            var entity = new AreaEntity
            {
                Id = Guid.NewGuid(),
                NombreArea = "NombreArea",
                EmpleadoResponsableId = Guid.NewGuid(),
            };
            areaRepoMock
                .Setup(m => m.SearchMatchingOneResult(It.IsAny<Expression<Func<AreaEntity, bool>>>()))
                .Returns(entity);

            var service = new ServiceCollection();

            service.AddTransient(_ => areaRepoMock.Object);

            service.ConfigureGenericasService(new DbSettings());
            var provider = service.BuildServiceProvider();
            var areaService = provider.GetRequiredService<IAreaService>();

            var result = await areaService.Get(new AreaRequestDto
            {
                NombreArea = "NombreArea",
                Id = Guid.NewGuid(),
                EmpleadoResponsableId = Guid.NewGuid()
            }).ConfigureAwait(false);

            Assert.NotNull(result.ToString());
            Assert.Equal(entity.Id, result.Id);

        }
        [Fact]
        [IntegrationTest]
        public async void Area_Get_Test_IntegrationTest()
        {
            ServiceProvider provider = ServiceCollectionArea();

            var areaService = provider.GetRequiredService<IAreaService>();
            var areaRepositorio = provider.GetRequiredService<IAreaRepositorio>();

            var dtoArea = new AreaRequestDto
            {
                Id = Guid.NewGuid(),
                NombreArea = "FakeListTipoDocumento1",
                EmpleadoResponsableId = Guid.NewGuid()
            };

            var area = areaRepositorio
                .SearchMatching<AreaEntity>(x => x.NombreArea == dtoArea.NombreArea)
                .FirstOrDefault();
            if (area != null || area != default)
                _ = await areaRepositorio.Delete(area).ConfigureAwait(false);

            _ = await areaService.Insert(dtoArea).ConfigureAwait(false);

            var dtoArea2 = new AreaRequestDto
            {
                Id = dtoArea.Id,
                EmpleadoResponsableId = Guid.NewGuid()
            };
            var result = await areaService.Get(dtoArea2).ConfigureAwait(false);

            Assert.Equal(dtoArea.Id, result.Id);
        }
        #endregion
    }
}
