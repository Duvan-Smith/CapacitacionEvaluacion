using AutoMapper;
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
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Categories;

namespace Test.Evaluacion.Core.Aplicacion.Core.AdministracionPersonas.Genericas.Areas
{
    public class AreaServiceGetAllTest
    {
        private static ServiceProvider ServiceCollectionArea()
        {
            var service = new ServiceCollection();

            service.ConfigureGenericasService(new DbSettings
            {
                ConnectionString = "Data Source=Asus;Initial Catalog=evaluacion;Integrated Security=True"
            });
            service.ConfigurePersonasService(new DbSettings
            {
                ConnectionString = "Data Source=Asus;Initial Catalog=evaluacion;Integrated Security=True"
            });

            var provider = service.BuildServiceProvider();
            return provider;
        }
        [Fact]
        [UnitTest]
        public async Task Check_AllParameterNull_AreaGetAll_Exception()
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
        #region GetAll
        [Fact]
        [UnitTest]
        public async Task Area_GetAll_Test_Full()
        {
            var areaRepoMock = new Mock<IAreaRepositorio>();
            var Listentity = new List<AreaEntity>
            {
                new AreaEntity
                {
                    Id = Guid.NewGuid(),
                    NombreArea = "NombreArea",
                    EmpleadoResponsableId = Guid.NewGuid(),
                }
            };
            areaRepoMock
                .Setup(m => m.GetAll<AreaEntity>())
                .Returns(Listentity);

            var service = new ServiceCollection();

            service.AddTransient(_ => areaRepoMock.Object);

            service.ConfigureGenericasService(new DbSettings());
            var provider = service.BuildServiceProvider();
            var areaService = provider.GetRequiredService<IAreaService>();
            var mapper = provider.GetRequiredService<IMapper>();

            var result = mapper.Map<IEnumerable<AreaEntity>>(await areaService.GetAll().ConfigureAwait(false));

            Assert.NotNull(result.ToString());
            Assert.Equal(Listentity[0].Id, result.First().Id);

        }
        [Fact]
        [IntegrationTest]
        public async void Area_GetAll_Test_IntegrationTest()
        {
            ServiceProvider provider = ServiceCollectionArea();

            var areaService = provider.GetRequiredService<IAreaService>();
            var areaRepositorio = provider.GetRequiredService<IAreaRepositorio>();
            var mapper = provider.GetRequiredService<IMapper>();

            var dtoArea = new AreaRequestDto
            {
                Id = Guid.NewGuid(),
                NombreArea = "Area_GetAll_Test_ITest",
                EmpleadoResponsableId = Guid.NewGuid(),
            };

            var area = areaRepositorio
                .SearchMatching<AreaEntity>(x => x.Id == dtoArea.Id)
                .FirstOrDefault();
            if (area != null || area != default)
                _ = await areaRepositorio.Delete(area).ConfigureAwait(false);

            _ = await areaService.Insert(dtoArea).ConfigureAwait(false);

            var result = mapper.Map<IEnumerable<AreaEntity>>(await areaService.GetAll().ConfigureAwait(false));

            Assert.NotNull(result.ToString());
            Assert.True(result.Any());

            _ = await areaService.Delete(dtoArea).ConfigureAwait(false);
        }
        #endregion
    }
}
