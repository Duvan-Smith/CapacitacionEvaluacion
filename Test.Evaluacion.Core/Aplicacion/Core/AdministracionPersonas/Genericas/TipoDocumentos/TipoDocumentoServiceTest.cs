using AutoMapper;
using Evaluacion.Aplicacion.Core.AdministracionPersonas.Genericas.Configuration;
using Evaluacion.Aplicacion.Core.AdministracionPersonas.Genericas.TipoDocumentos.Excepciones;
using Evaluacion.Aplicacion.Core.AdministracionPersonas.Genericas.TipoDocumentos.Services;
using Evaluacion.Aplicacion.Dto.Genericas.TipoDocumentos;
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

namespace Test.Evaluacion.Core.Aplicacion.Core.AdministracionPersonas.Genericas.TipoDocumentos
{
    public class TipoDocumentoServiceTest
    {
        [Fact]
        [UnitTest]
        public async Task Check_AllParameterNull_TipoDocumento_Exception()
        {
            var service = new ServiceCollection();
            service.ConfigureGenericasService(new DbSettings());
            var provider = service.BuildServiceProvider();
            var tipoDocumentoService = provider.GetRequiredService<ITipoDocumentoService>();
            await Assert.ThrowsAsync<TipoDocumentoRequestDtoNullException>(() => tipoDocumentoService.Update(null)).ConfigureAwait(false);
            await Assert.ThrowsAsync<TipoDocumentoRequestDtoNullException>(() => tipoDocumentoService.Delete(null)).ConfigureAwait(false);
            await Assert.ThrowsAsync<TipoDocumentoRequestDtoNullException>(() => tipoDocumentoService.Get(null)).ConfigureAwait(false);
            await Assert.ThrowsAsync<TipoDocumentoRequestDtoNullException>(() => tipoDocumentoService.Insert(null)).ConfigureAwait(false);
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
            var areaInsertRepoMock = new Mock<ITipoDocumentoRepositorio>();

            areaRepoMock
                .Setup(m => m.SearchMatching(It.IsAny<Expression<Func<TipoDocumentoEntity, bool>>>()));
            areaInsertRepoMock
                .Setup(m => m.Insert(It.IsAny<TipoDocumentoEntity>()))
                .Returns(Task.FromResult(new TipoDocumentoEntity { Id = Guid.NewGuid() }));

            var service = new ServiceCollection();

            service.AddTransient(_ => areaRepoMock.Object);
            service.AddTransient(_ => areaInsertRepoMock.Object);

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
            var service = new ServiceCollection();

            service.ConfigureGenericasService(new DbSettings
            {
                ConnectionString = "Data Source=DESKTOP-NE15I70\\BDDUVAN;Initial Catalog=evaluacion;User ID=sa;Password=3147073260"
            });

            var provider = service.BuildServiceProvider();

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
        #region Delect
        [Fact]
        [UnitTest]
        public async Task TipoDocumento_Delete_Test_Fail()
        {
            var areaRepoMock = new Mock<ITipoDocumentoRepositorio>();
            areaRepoMock
                .Setup(m => m.SearchMatchingOneResult(It.IsAny<Expression<Func<TipoDocumentoEntity, bool>>>()));

            var service = new ServiceCollection();

            service.AddTransient(_ => areaRepoMock.Object);

            service.ConfigureGenericasService(new DbSettings());
            var provider = service.BuildServiceProvider();
            var tipoDocumentoService = provider.GetRequiredService<ITipoDocumentoService>();

            await Assert.ThrowsAsync<TipoDocumentoNoExistException>(() => tipoDocumentoService.Delete(new TipoDocumentoRequestDto { NombreTipoDocumento = "NombreTipoDocumento", Id = Guid.NewGuid() })).ConfigureAwait(false);
        }
        [Fact]
        [UnitTest]
        public async Task TipoDocumento_Delete_Test_Full()
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
            areaRepoMock
                .Setup(m => m.Delete(It.IsAny<TipoDocumentoEntity>()))
                .Returns(true);

            var service = new ServiceCollection();

            service.AddTransient(_ => areaRepoMock.Object);

            service.ConfigureGenericasService(new DbSettings());
            var provider = service.BuildServiceProvider();
            var tipoDocumentoService = provider.GetRequiredService<ITipoDocumentoService>();

            var result = await tipoDocumentoService.Delete(new TipoDocumentoRequestDto
            {
                Id = Guid.NewGuid(),
            }).ConfigureAwait(false);

            Assert.NotNull(result.ToString());
            Assert.True(result);
        }
        [Fact]
        [IntegrationTest]
        public async void TipoDocumento_Delete_Test_IntegrationTest()
        {
            var service = new ServiceCollection();

            service.ConfigureGenericasService(new DbSettings
            {
                ConnectionString = "Data Source=DESKTOP-NE15I70\\BDDUVAN;Initial Catalog=evaluacion;User ID=sa;Password=3147073260"
            });
            service.ConfigureBaseRepository(new DbSettings
            {
                ConnectionString = "Data Source=DESKTOP-NE15I70\\BDDUVAN;Initial Catalog=evaluacion;User ID=sa;Password=3147073260"
            });
            var provider = service.BuildServiceProvider();

            var tipoDocumentoService = provider.GetRequiredService<ITipoDocumentoService>();
            var areaRepositorio = provider.GetRequiredService<ITipoDocumentoRepositorio>();

            var dtoTipoDocumento = new TipoDocumentoRequestDto
            {
                Id = Guid.NewGuid(),
                NombreTipoDocumento = "FakeListTipoDocumento1",
            };

            var area = areaRepositorio
                .SearchMatching<TipoDocumentoEntity>(x => x.NombreTipoDocumento == dtoTipoDocumento.NombreTipoDocumento)
                .FirstOrDefault();
            if (area != null || area != default)
                areaRepositorio.Delete(area);

            await tipoDocumentoService.Insert(dtoTipoDocumento).ConfigureAwait(false);

            var dtoTipoDocumento2 = new TipoDocumentoRequestDto
            {
                Id = dtoTipoDocumento.Id,
            };
            var result = await tipoDocumentoService.Delete(dtoTipoDocumento2).ConfigureAwait(false);

            Assert.True(result);
        }
        #endregion
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
            var areaRepoMock = new Mock<ITipoDocumentoRepositorio>();
            var entity = new TipoDocumentoEntity
            {
                Id = Guid.NewGuid(),
                NombreTipoDocumento = "NombreTipoDocumento"
            };
            areaRepoMock
                .Setup(m => m.SearchMatchingOneResult(It.IsAny<Expression<Func<TipoDocumentoEntity, bool>>>()))
                .Returns(entity);
            areaRepoMock
                .Setup(m => m.Update(It.IsAny<TipoDocumentoEntity>()))
                .Returns(true);

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
            var service = new ServiceCollection();

            service.ConfigureGenericasService(new DbSettings
            {
                ConnectionString = "Data Source=DESKTOP-NE15I70\\BDDUVAN;Initial Catalog=evaluacion;User ID=sa;Password=3147073260"
            });
            service.ConfigureBaseRepository(new DbSettings
            {
                ConnectionString = "Data Source=DESKTOP-NE15I70\\BDDUVAN;Initial Catalog=evaluacion;User ID=sa;Password=3147073260"
            });
            var provider = service.BuildServiceProvider();

            var tipoDocumentoService = provider.GetRequiredService<ITipoDocumentoService>();
            var areaRepositorio = provider.GetRequiredService<ITipoDocumentoRepositorio>();

            var dtoTipoDocumento = new TipoDocumentoRequestDto
            {
                Id = Guid.NewGuid(),
                NombreTipoDocumento = "FakeListTipoDocumento1",
            };

            var area = areaRepositorio
                .SearchMatching<TipoDocumentoEntity>(x => x.NombreTipoDocumento == dtoTipoDocumento.NombreTipoDocumento)
                .FirstOrDefault();
            if (area != null || area != default)
                areaRepositorio.Delete(area);

            await tipoDocumentoService.Insert(dtoTipoDocumento).ConfigureAwait(false);

            var dtoTipoDocumento2 = new TipoDocumentoRequestDto
            {
                Id = dtoTipoDocumento.Id,
            };
            var result = await tipoDocumentoService.Update(dtoTipoDocumento2).ConfigureAwait(false);

            Assert.True(result);

            await tipoDocumentoService.Delete(dtoTipoDocumento).ConfigureAwait(false);
        }
        #endregion
        #region Get
        [Fact]
        [UnitTest]
        public async Task TipoDocumento_Get_Test_Fail()
        {
            var areaRepoMock = new Mock<ITipoDocumentoRepositorio>();
            areaRepoMock
                .Setup(m => m.SearchMatchingOneResult(It.IsAny<Expression<Func<TipoDocumentoEntity, bool>>>()));

            var service = new ServiceCollection();

            service.AddTransient(_ => areaRepoMock.Object);

            service.ConfigureGenericasService(new DbSettings());
            var provider = service.BuildServiceProvider();
            var tipoDocumentoService = provider.GetRequiredService<ITipoDocumentoService>();

            await Assert.ThrowsAsync<TipoDocumentoNoExistException>(() => tipoDocumentoService.Get(new TipoDocumentoRequestDto { NombreTipoDocumento = "NombreTipoDocumento", Id = Guid.NewGuid() })).ConfigureAwait(false);
        }
        [Fact]
        [UnitTest]
        public async Task TipoDocumento_Get_Test_Full()
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

            var service = new ServiceCollection();

            service.AddTransient(_ => areaRepoMock.Object);

            service.ConfigureGenericasService(new DbSettings());
            var provider = service.BuildServiceProvider();
            var tipoDocumentoService = provider.GetRequiredService<ITipoDocumentoService>();

            var result = await tipoDocumentoService.Get(new TipoDocumentoRequestDto
            {
                Id = Guid.NewGuid(),
            }).ConfigureAwait(false);

            Assert.NotNull(result.ToString());
            Assert.Equal(entity.Id, result.Id);

        }
        [Fact]
        [IntegrationTest]
        public async void TipoDocumento_Get_Test_IntegrationTest()
        {
            var service = new ServiceCollection();

            service.ConfigureGenericasService(new DbSettings
            {
                ConnectionString = "Data Source=DESKTOP-NE15I70\\BDDUVAN;Initial Catalog=evaluacion;User ID=sa;Password=3147073260"
            });
            service.ConfigureBaseRepository(new DbSettings
            {
                ConnectionString = "Data Source=DESKTOP-NE15I70\\BDDUVAN;Initial Catalog=evaluacion;User ID=sa;Password=3147073260"
            });
            var provider = service.BuildServiceProvider();

            var tipoDocumentoService = provider.GetRequiredService<ITipoDocumentoService>();
            var areaRepositorio = provider.GetRequiredService<ITipoDocumentoRepositorio>();

            var dtoTipoDocumento = new TipoDocumentoRequestDto
            {
                Id = Guid.NewGuid(),
                NombreTipoDocumento = "FakeListTipoDocumento1",
            };

            var area = areaRepositorio
                .SearchMatching<TipoDocumentoEntity>(x => x.NombreTipoDocumento == dtoTipoDocumento.NombreTipoDocumento)
                .FirstOrDefault();
            if (area != null || area != default)
                areaRepositorio.Delete(area);

            await tipoDocumentoService.Insert(dtoTipoDocumento).ConfigureAwait(false);

            var dtoTipoDocumento2 = new TipoDocumentoRequestDto
            {
                Id = dtoTipoDocumento.Id,
            };
            var result = await tipoDocumentoService.Get(dtoTipoDocumento2).ConfigureAwait(false);

            Assert.Equal(dtoTipoDocumento.Id, result.Id);
        }
        #endregion
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
            var service = new ServiceCollection();

            service.ConfigureGenericasService(new DbSettings
            {
                ConnectionString = "Data Source=DESKTOP-NE15I70\\BDDUVAN;Initial Catalog=evaluacion;User ID=sa;Password=3147073260"
            });
            service.ConfigureBaseRepository(new DbSettings
            {
                ConnectionString = "Data Source=DESKTOP-NE15I70\\BDDUVAN;Initial Catalog=evaluacion;User ID=sa;Password=3147073260"
            });
            var provider = service.BuildServiceProvider();

            var tipoDocumentoService = provider.GetRequiredService<ITipoDocumentoService>();
            var areaRepositorio = provider.GetRequiredService<ITipoDocumentoRepositorio>();
            var mapper = provider.GetRequiredService<IMapper>();

            var dtoTipoDocumento = new TipoDocumentoRequestDto
            {
                Id = Guid.Parse("45c2a9b5-1eac-48d3-83a4-ff692326e4f7"),
                NombreTipoDocumento = "FakeListTipoDocumento1",
            };

            var area = areaRepositorio
                .SearchMatching<TipoDocumentoEntity>(x => x.Id == dtoTipoDocumento.Id)
                .FirstOrDefault();
            if (area != null || area != default)
                areaRepositorio.Delete(area);

            await tipoDocumentoService.Insert(dtoTipoDocumento).ConfigureAwait(false);

            var result = mapper.Map<IEnumerable<TipoDocumentoEntity>>(await tipoDocumentoService.GetAll().ConfigureAwait(false));

            Assert.NotNull(result.ToString());
            Assert.True(result.Any());

            await tipoDocumentoService.Delete(dtoTipoDocumento).ConfigureAwait(false);
        }
        #endregion
    }
}
