using Evaluacion.Aplicacion.Core.AdministracionPersonas.Genericas.Configuration;
using Evaluacion.Aplicacion.Core.AdministracionPersonas.Genericas.TipoDocumentos.Excepciones;
using Evaluacion.Aplicacion.Core.AdministracionPersonas.Genericas.TipoDocumentos.Services;
using Evaluacion.Aplicacion.Core.AdministracionPersonas.Personas.Configuration;
using Evaluacion.Aplicacion.Dto.Genericas.TipoDocumentos;
using Evaluacion.Dominio.Core.Genericas.TipoDocumentos;
using Evaluacion.Infraestructura.Datos.Persistencia.Core.Base.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;
using Xunit.Categories;

namespace Test.Evaluacion.Core.Aplicacion.Core.AdministracionPersonas.Genericas.TipoDocumentos
{
    public class TipoDocumentoServiceUpdateTest
    {
        private static Mock<ITipoDocumentoRepositorio> MockTipoDocumentoFull()
        {
            var areaRepoMock = new Mock<ITipoDocumentoRepositorio>();
            var entity = new TipoDocumentoEntity
            {
                Id = Guid.NewGuid(),
                NombreTipoDocumento = "NombreTipoDocumento"
            };
            areaRepoMock
                .Setup(m => m.SearchMatchingOneResult(It.IsAny<Expression<Func<TipoDocumentoEntity, bool>>>()))
                .Returns(entity);
            return areaRepoMock;
        }
        private static ServiceProvider ServiceCollectionTipoDocumento()
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
        public async Task Check_AllParameterNull_TipoDocumentoUpdate_Exception()
        {
            var service = new ServiceCollection();
            service.ConfigureGenericasService(new DbSettings());
            var provider = service.BuildServiceProvider();
            var tipoDocumentoService = provider.GetRequiredService<ITipoDocumentoService>();
            _ = await Assert.ThrowsAsync<TipoDocumentoRequestDtoNullException>(() => tipoDocumentoService.Update(null)).ConfigureAwait(false);
        }
        #region Update
        [Fact]
        [UnitTest]
        public async Task TipoDocumento_Update_Test_Fail()
        {
            var areaRepoMock = new Mock<ITipoDocumentoRepositorio>();
            areaRepoMock
                .Setup(m => m.SearchMatchingOneResult(It.IsAny<Expression<Func<TipoDocumentoEntity, bool>>>()));

            var service = new ServiceCollection();

            service.AddTransient(_ => areaRepoMock.Object);

            service.ConfigureGenericasService(new DbSettings());
            var provider = service.BuildServiceProvider();
            var tipoDocumentoService = provider.GetRequiredService<ITipoDocumentoService>();

            await Assert.ThrowsAsync<TipoDocumentoNoExistException>(() => tipoDocumentoService.Update(new TipoDocumentoRequestDto { NombreTipoDocumento = "NombreTipoDocumento", Id = Guid.NewGuid() })).ConfigureAwait(false);
        }
        [Fact]
        [UnitTest]
        public async Task TipoDocumento_Update_Test_Full()
        {
            Mock<ITipoDocumentoRepositorio> areaRepoMock = MockTipoDocumentoFull();

            areaRepoMock
                .Setup(m => m.Update(It.IsAny<TipoDocumentoEntity>()))
                .Returns(Task.FromResult(true));

            var service = new ServiceCollection();

            service.AddTransient(_ => areaRepoMock.Object);

            service.ConfigureGenericasService(new DbSettings());
            var provider = service.BuildServiceProvider();
            var tipoDocumentoService = provider.GetRequiredService<ITipoDocumentoService>();

            var result = await tipoDocumentoService.Update(new TipoDocumentoRequestDto
            {
                Id = Guid.NewGuid(),
            }).ConfigureAwait(false);

            Assert.NotNull(result.ToString());
            Assert.True(result);
        }
        [Fact]
        [IntegrationTest]
        public async void TipoDocumento_Update_Test_IntegrationTest()
        {
            ServiceProvider provider = ServiceCollectionTipoDocumento();

            var tipoDocumentoService = provider.GetRequiredService<ITipoDocumentoService>();
            var areaRepositorio = provider.GetRequiredService<ITipoDocumentoRepositorio>();

            var dtoTipoDocumento = new TipoDocumentoRequestDto
            {
                Id = Guid.NewGuid(),
                NombreTipoDocumento = "FakeListTipoDocumentoUpdateI1",
            };

            var area = areaRepositorio
                .SearchMatching<TipoDocumentoEntity>(x => x.NombreTipoDocumento == dtoTipoDocumento.NombreTipoDocumento)
                .FirstOrDefault();
            if (area != null || area != default)
                _ = await areaRepositorio.Delete(area).ConfigureAwait(false);

            _ = await tipoDocumentoService.Insert(dtoTipoDocumento).ConfigureAwait(false);

            var dtoTipoDocumento2 = new TipoDocumentoRequestDto
            {
                Id = dtoTipoDocumento.Id,
            };
            var result = await tipoDocumentoService.Update(dtoTipoDocumento2).ConfigureAwait(false);

            Assert.True(result);

            _ = await tipoDocumentoService.Delete(dtoTipoDocumento).ConfigureAwait(false);
        }
        #endregion
    }
}
