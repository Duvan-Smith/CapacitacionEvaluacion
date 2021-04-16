using AutoMapper;
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
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Categories;

namespace Test.Evaluacion.Core.Aplicacion.Core.AdministracionPersonas.Genericas.TipoDocumentos
{
    public class TipoDocumentoServiceGetAllTest
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
        public async Task Check_AllParameterNull_TipoDocumentoGetAll_Exception()
        {
            var service = new ServiceCollection();
            service.ConfigureGenericasService(new DbSettings());
            var provider = service.BuildServiceProvider();
            var tipoDocumentoService = provider.GetRequiredService<ITipoDocumentoService>();
            _ = await Assert.ThrowsAsync<TipoDocumentoRequestDtoNullException>(() => tipoDocumentoService.Update(null)).ConfigureAwait(false);
            _ = await Assert.ThrowsAsync<TipoDocumentoRequestDtoNullException>(() => tipoDocumentoService.Delete(null)).ConfigureAwait(false);
            _ = await Assert.ThrowsAsync<TipoDocumentoRequestDtoNullException>(() => tipoDocumentoService.Get(null)).ConfigureAwait(false);
            _ = await Assert.ThrowsAsync<TipoDocumentoRequestDtoNullException>(() => tipoDocumentoService.Insert(null)).ConfigureAwait(false);
        }
        #region GetAll
        [Fact]
        [UnitTest]
        public async Task TipoDocumento_GetAll_Test_Full()
        {
            var areaRepoMock = new Mock<ITipoDocumentoRepositorio>();
            var Listentity = new List<TipoDocumentoEntity>
            {
                new TipoDocumentoEntity
                {
                    Id = Guid.NewGuid(),
                    NombreTipoDocumento = "NombreTipoDocumento"
                }
            };
            areaRepoMock
                .Setup(m => m.GetAll<TipoDocumentoEntity>())
                .Returns(Listentity);

            var service = new ServiceCollection();

            service.AddTransient(_ => areaRepoMock.Object);

            service.ConfigureGenericasService(new DbSettings());
            var provider = service.BuildServiceProvider();
            var tipoDocumentoService = provider.GetRequiredService<ITipoDocumentoService>();
            var mapper = provider.GetRequiredService<IMapper>();

            var result = mapper.Map<IEnumerable<TipoDocumentoEntity>>(await tipoDocumentoService.GetAll().ConfigureAwait(false));

            Assert.NotNull(result.ToString());
            Assert.Equal(Listentity[0].Id, result.First().Id);
        }
        [Fact]
        [IntegrationTest]
        public async void TipoDocumento_GetAll_Test_IntegrationTest()
        {
            ServiceProvider provider = ServiceCollectionTipoDocumento();

            var tipoDocumentoService = provider.GetRequiredService<ITipoDocumentoService>();
            var areaRepositorio = provider.GetRequiredService<ITipoDocumentoRepositorio>();
            var mapper = provider.GetRequiredService<IMapper>();

            var dtoTipoDocumento = new TipoDocumentoRequestDto
            {
                Id = Guid.NewGuid(),
                NombreTipoDocumento = "FakeListTipoDocumento1GetAll",
            };

            var area = areaRepositorio
                .SearchMatching<TipoDocumentoEntity>(x => x.Id == dtoTipoDocumento.Id)
                .FirstOrDefault();
            if (area != null || area != default)
                _ = await areaRepositorio.Delete(area).ConfigureAwait(false);

            _ = await tipoDocumentoService.Insert(dtoTipoDocumento).ConfigureAwait(false);

            var result = mapper.Map<IEnumerable<TipoDocumentoEntity>>(await tipoDocumentoService.GetAll().ConfigureAwait(false));

            Assert.NotNull(result.ToString());
            Assert.True(result.Any());

            _ = await tipoDocumentoService.Delete(dtoTipoDocumento).ConfigureAwait(false);
        }
        #endregion
    }
}
