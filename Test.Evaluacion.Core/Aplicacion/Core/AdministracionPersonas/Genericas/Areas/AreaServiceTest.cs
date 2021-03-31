﻿using AutoMapper;
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
                .Returns(true);

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
        public void No_Eliminar_areas_que_tengan_empleados_asociados_Full_IntegrationTest()
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
        public async void No_Eliminar_areas_que_tengan_empleados_asociados_Fail_IntegrationTest()
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
                NombreArea = "FakeArea3",
                EmpleadoResponsableId = Guid.NewGuid()
            };
            var empleado = areaRepositorio
                .SearchMatching<AreaEntity>(x => x.NombreArea == dtoArea.NombreArea)
                .FirstOrDefault();
            areaRepositorio.Delete(empleado);
            await areaService.Insert(dtoArea).ConfigureAwait(false);

            var dtoEmpleado = new EmpleadoRequestDto
            {
                Id = Guid.NewGuid(),
                Nombre = "Fake_Empleado_Area_1",
                Apellido = "Fake_Empleado_Area_1",
                FechaNacimiento = DateTimeOffset.Now,
                FechaRegistro = DateTimeOffset.Now,
                NumeroTelefono = 0,
                CorreoElectronico = "fake@fake.fake",
                TipoDocumentoId = Guid.Parse("581e3e67-82e2-4f1f-b379-9bd870db669e"),
                CodigoTipoDocumento = "12345678",
                Salario = 20000,
                AreaId = dtoArea.Id,
                CodigoEmpleado = "Prueba21"
            };

            await empleadoService.Insert(dtoEmpleado).ConfigureAwait(false);

            await Assert.ThrowsAsync<EmpleadoAreaAlreadyExistException>(() => areaService.Delete(dtoArea)).ConfigureAwait(false);

            await empleadoService.Delete(dtoEmpleado).ConfigureAwait(false);
            var empleadoEnd = areaRepositorio
                .SearchMatching<AreaEntity>(x => x.NombreArea == dtoArea.NombreArea)
                .FirstOrDefault();
            areaRepositorio.Delete(empleadoEnd);
        }
        #endregion
        //TODO: Las área deben tener una persona encargada
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
                .Returns(true);

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
                EmpleadoResponsableId = Guid.Empty
            };

            _ = await Assert.ThrowsAsync<AreaEmpleadoResponsableIdNullException>(() => areaService.Delete(dtoArea)).ConfigureAwait(false);

            dtoArea.EmpleadoResponsableId = Guid.NewGuid();
            _ = areaService.Insert(dtoArea);

            var response = areaService.Delete(dtoArea);

            Assert.NotNull(response);
            Assert.True(response.Result);
            Assert.NotEqual(default, response);
        }
        #endregion
        //Anotacion: Pruebas unitarias del crud: Insert, Update
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
        #region Updtade
        [Fact]
        [UnitTest]
        public async Task Area_Update_Test_Fail()
        {
            var areaRepoMock = new Mock<IAreaRepositorio>();
            areaRepoMock
                .Setup(m => m.SearchMatchingOneResult(It.IsAny<Expression<Func<AreaEntity, bool>>>()));

            var service = new ServiceCollection();

            service.AddTransient(_ => areaRepoMock.Object);

            service.ConfigureGenericasService(new DbSettings());
            var provider = service.BuildServiceProvider();
            var areaService = provider.GetRequiredService<IAreaService>();

            await Assert.ThrowsAsync<AreaNoExistException>(() => areaService.Update(new AreaRequestDto { NombreArea = "AreaPrueba", EmpleadoResponsableId = Guid.NewGuid() })).ConfigureAwait(false);
        }
        [Fact]
        [UnitTest]
        public async Task Area_Update_Test_Full()
        {
            var areaRepoMock = new Mock<IAreaRepositorio>();
            var entity = new AreaEntity
            {
                Id = Guid.NewGuid(),
                NombreArea = "AreaPrueba"
            };
            areaRepoMock
                .Setup(m => m.SearchMatchingOneResult(It.IsAny<Expression<Func<AreaEntity, bool>>>()))
                .Returns(entity);
            areaRepoMock
                //.Setup(m => m.Update(It.Is<AreaEntity>(x => x.Id == entity.Id)))
                .Setup(m => m.Update(It.IsAny<AreaEntity>()))
                .Returns(true);

            var service = new ServiceCollection();

            service.AddTransient(_ => areaRepoMock.Object);

            service.ConfigureGenericasService(new DbSettings());
            var provider = service.BuildServiceProvider();
            var areaService = provider.GetRequiredService<IAreaService>();

            var result = await areaService.Update(new AreaRequestDto
            {
                Id = Guid.NewGuid(),
                EmpleadoResponsableId = Guid.NewGuid()
            }).ConfigureAwait(false);

            Assert.NotNull(result.ToString());
            Assert.True(result);

        }
        [Fact]
        [IntegrationTest]
        public async void Area_Update_Test_IntegrationTest()
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

            var areaService = provider.GetRequiredService<IAreaService>();
            var areaRepositorio = provider.GetRequiredService<IAreaRepositorio>();

            var dtoArea = new AreaRequestDto
            {
                Id = Guid.NewGuid(),
                NombreArea = "FakeListEmpleado",
                EmpleadoResponsableId = Guid.NewGuid()
            };

            var area = areaRepositorio
                .SearchMatching<AreaEntity>(x => x.NombreArea == dtoArea.NombreArea)
                .FirstOrDefault();
            if (area != null || area != default)
                areaRepositorio.Delete(area);

            await areaService.Insert(dtoArea).ConfigureAwait(false);

            var dtoArea2 = new AreaRequestDto
            {
                Id = dtoArea.Id,
                EmpleadoResponsableId = Guid.NewGuid()
            };
            var result = await areaService.Update(dtoArea2).ConfigureAwait(false);

            Assert.True(result);

            //await areaService.GetAll().ConfigureAwait(false);

            await areaService.Delete(dtoArea).ConfigureAwait(false);
        }
        #endregion
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
                areaRepositorio.Delete(area);

            await areaService.Insert(dtoArea).ConfigureAwait(false);

            var dtoArea2 = new AreaRequestDto
            {
                Id = dtoArea.Id,
                EmpleadoResponsableId = Guid.NewGuid()
            };
            var result = await areaService.Get(dtoArea2).ConfigureAwait(false);

            Assert.Equal(dtoArea.Id, result.Id);
        }
        #endregion
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
                areaRepositorio.Delete(area);

            await areaService.Insert(dtoArea).ConfigureAwait(false);

            var result = mapper.Map<IEnumerable<AreaEntity>>(await areaService.GetAll().ConfigureAwait(false));

            Assert.NotNull(result.ToString());
            Assert.True(result.Any());

            await areaService.Delete(dtoArea).ConfigureAwait(false);
        }
        #endregion
        [Fact]
        [IntegrationTest]
        public async void No_Test_Get_Tabla_RelacionesAreas()
        {
            var service = new ServiceCollection();

            service.ConfigureGenericasService(new DbSettings
            {
                ConnectionString = "Data Source=DESKTOP-NE15I70\\BDDUVAN;Initial Catalog=evaluacion;User ID=sa;Password=3147073260"
            });
            service.ConfigurePersonasService(new DbSettings
            {
                ConnectionString = "Data Source=DESKTOP-NE15I70\\BDDUVAN;Initial Catalog=evaluacion;User ID=sa;Password=3147073260"
            });

            var provider = service.BuildServiceProvider();

            var areaService = provider.GetRequiredService<IAreaService>();
            var empleadoService = provider.GetRequiredService<IEmpleadoService>();

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
                Nombre = "Fake_Empleado_Area_2",
                Apellido = "Fake_Empleado_Area_2",
                FechaNacimiento = DateTimeOffset.Now,
                FechaRegistro = DateTimeOffset.Now,
                NumeroTelefono = 0,
                CorreoElectronico = "fake@fake.fake",
                TipoDocumentoId = Guid.Parse("581e3e67-82e2-4f1f-b379-9bd870db669e"),
                CodigoTipoDocumento = "12345678",
                Salario = 20000,
                AreaId = dtoArea.Id,
                CodigoEmpleado = "Prueba21"
            };
            await empleadoService.Insert(dtoEmpleado).ConfigureAwait(false);

            var dtoArea2 = new AreaRequestDto
            {
                Id = dtoArea.Id,
                EmpleadoResponsableId = Guid.NewGuid()
            };
            await areaService.Update(dtoArea2).ConfigureAwait(false);

            await areaService.GetAll().ConfigureAwait(false);

            await empleadoService.Delete(dtoEmpleado).ConfigureAwait(false);
            await areaService.Delete(dtoArea).ConfigureAwait(false);
        }
    }
}
