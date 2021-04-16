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
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;
using Xunit.Categories;

namespace Test.Evaluacion.Core.Aplicacion.Core.AdministracionPersonas.Genericas.TipoDocumentos
{
    public class TipoDocumentoServiceInsertTest
    {
        private static ServiceProvider ServiceCollectionTipoDocumento()
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
        public async Task Check_AllParameterNull_TipoDocumentoInsert_Exception()
        {
            var service = new ServiceCollection();
            service.ConfigureGenericasService(new DbSettings());
            var provider = service.BuildServiceProvider();
            var tipoDocumentoService = provider.GetRequiredService<ITipoDocumentoService>();
            _ = await Assert.ThrowsAsync<TipoDocumentoRequestDtoNullException>(() => tipoDocumentoService.Insert(null)).ConfigureAwait(false);
        }
        #region Insert
        [Fact]
        [UnitTest]
        public async Task TipoDocumento_Insert_Test_Fail()
        {
            var areaRepoMock = new Mock<ITipoDocumentoRepositorio>();
            areaRepoMock
                .Setup(m => m.SearchMatching(It.IsAny<Expression<Func<TipoDocumentoEntity, bool>>>()))
                .Returns(new List<TipoDocumentoEntity> { new TipoDocumentoEntity
                {
                    Id=Guid.NewGuid(),
                }});

            var service = new ServiceCollection();

            service.AddTransient(_ => areaRepoMock.Object);

            service.ConfigureGenericasService(new DbSettings());
            var provider = service.BuildServiceProvider();
            var tipoDocumentoService = provider.GetRequiredService<ITipoDocumentoService>();

            await Assert.ThrowsAsync<TipoDocumentonameAlreadyExistException>(() => tipoDocumentoService.Insert(new TipoDocumentoRequestDto { Id = Guid.NewGuid() })).ConfigureAwait(false);
        }
        [Fact]
        [UnitTest]
        public async Task TipoDocumento_Insert_Test_Full()
        {
            var areaRepoMock = new Mock<ITipoDocumentoRepositorio>();

            areaRepoMock
                .Setup(m => m.SearchMatching(It.IsAny<Expression<Func<TipoDocumentoEntity, bool>>>()));
            areaRepoMock
                .Setup(m => m.Insert(It.IsAny<TipoDocumentoEntity>()))
                .Returns(Task.FromResult(new TipoDocumentoEntity { Id = Guid.NewGuid() }));

            var service = new ServiceCollection();

            service.AddTransient(_ => areaRepoMock.Object);

            service.ConfigureGenericasService(new DbSettings());
            var provider = service.BuildServiceProvider();
            var tipoDocumentoService = provider.GetRequiredService<ITipoDocumentoService>();

            var result = await tipoDocumentoService.Insert(new TipoDocumentoRequestDto { Id = Guid.NewGuid() }).ConfigureAwait(false);

            Assert.NotNull(result.ToString());
            Assert.NotEqual(default, result);
        }
        [Fact]
        [IntegrationTest]
        public async void TipoDocumento_Insert_Test_IntegrationTest()
        {
            ServiceProvider provider = ServiceCollectionTipoDocumento();

            var tipoDocumentoService = provider.GetRequiredService<ITipoDocumentoService>();

            var dtoTipoDocumento = new TipoDocumentoRequestDto
            {
                Id = Guid.NewGuid(),
                NombreTipoDocumento = "FakeListTipoDocumento",
            };
            var result = await tipoDocumentoService.Insert(dtoTipoDocumento).ConfigureAwait(false);

            Assert.NotEqual(default, result);
            Assert.Equal(dtoTipoDocumento.Id, result);
            _ = await Assert.ThrowsAsync<TipoDocumentonameAlreadyExistException>(() => tipoDocumentoService.Insert(dtoTipoDocumento)).ConfigureAwait(false);
            _ = await tipoDocumentoService.Delete(dtoTipoDocumento).ConfigureAwait(false);
        }
        #endregion
    }
}
