﻿using AutoMapper;
using Evaluacion.Aplicacion.Core.AdministracionPersonas.Genericas.Areas.Services;
using Evaluacion.Aplicacion.Core.AdministracionPersonas.Genericas.Configuration;
using Evaluacion.Aplicacion.Core.AdministracionPersonas.Personas.Configuration;
using Evaluacion.Aplicacion.Core.AdministracionPersonas.Personas.Empleados.Excepciones;
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

namespace Test.Evaluacion.Core.Aplicacion.Core.AdministracionPersonas.Personas.Empleados
{
    public enum TipoPersona
    {
        Natural = 1,
        Juridico = 2,
    }
    public class EmpleadoServiceTest
    {
        private static ServiceProvider ServiceCollectionEmpleado()
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
            return provider;
        }
        [Fact]
        [UnitTest]
        public async Task Check_AllParameterNull_Empleado_Exception()
        {
            var service = new ServiceCollection();

            service.ConfigurePersonasService(new DbSettings());
            var provider = service.BuildServiceProvider();
            var empleadoService = provider.GetRequiredService<IEmpleadoService>();

            await Assert.ThrowsAsync<EmpleadoRequestDtoNullException>(() => empleadoService.Update(null)).ConfigureAwait(false);
            await Assert.ThrowsAsync<EmpleadoRequestDtoNullException>(() => empleadoService.Delete(null)).ConfigureAwait(false);
            await Assert.ThrowsAsync<EmpleadoRequestDtoNullException>(() => empleadoService.Get(null)).ConfigureAwait(false);
            await Assert.ThrowsAsync<EmpleadoRequestDtoNullException>(() => empleadoService.Insert(null)).ConfigureAwait(false);
        }
        //TODO: Empleado, Debe poderse distinguir entre jurídicas y naturales - hace parte de los parametros
        //Preguntar que con esta prueba
        #region Validar_TipoPersona_Empleado
        [Fact]
        [UnitTest]
        public async Task Validar_TipoPersona_Empleado_Fail()
        {
            var empleadoDto = new EmpleadoRequestDto
            {
                Nombre = "fake",
                //TODO: Debe poderse distinguir entre jurídicas y naturales
                FechaNacimiento = DateTimeOffset.Now,
                FechaRegistro = DateTimeOffset.Now,
                TipoDocumentoId = Guid.Parse("581E3E67-82E2-4F1F-B379-9BD870DB669E"),
                CodigoEmpleado = "Prueba05",
                AreaId = Guid.NewGuid()
            };
            var empleadoRepoMock = new Mock<IEmpleadoRepositorio>();

            empleadoRepoMock
                .Setup(m => m.SearchMatching(It.IsIn<Expression<Func<EmpleadoEntity, bool>>>(x => x.CodigoEmpleado == empleadoDto.CodigoEmpleado)));
            empleadoRepoMock
                .Setup(m => m.SearchMatching(It.IsIn<Expression<Func<EmpleadoEntity, bool>>>(x => x.Nombre == empleadoDto.Nombre)))
                .Returns(new List<EmpleadoEntity> { new EmpleadoEntity
                {
                    Id = Guid.NewGuid(),
                    Nombre = "FakePrueba",
                    AreaId = Guid.NewGuid()
                }});

            var service = new ServiceCollection();

            service.AddTransient(_ => empleadoRepoMock.Object);

            service.ConfigurePersonasService(new DbSettings());

            var provider = service.BuildServiceProvider();
            var empleadoService = provider.GetRequiredService<IEmpleadoService>();

            //Estas validaciones no se va a sacar ya que el Empleado siempre sera Natural
            //await Assert.ThrowsAsync<EmpleadoTipoPersonaNullException>(() => empleadoService.Insert(empleadoDto)).ConfigureAwait(false);

            empleadoDto.TipoPersona = 0;
            //await Assert.ThrowsAsync<EmpleadoTipoPersonaNullException>(() => empleadoService.Insert(empleadoDto)).ConfigureAwait(false);
        }
        [Fact]
        [UnitTest]
        public async Task Validar_TipoPersona_Empleado_Full()
        {
            var empleadoReposMock = new Mock<IEmpleadoRepositorio>();

            empleadoReposMock
                .Setup(m => m.SearchMatching(It.IsAny<Expression<Func<EmpleadoEntity, bool>>>()));
            empleadoReposMock
                .Setup(m => m.Insert(It.IsAny<EmpleadoEntity>()))
                .Returns(Task.FromResult(new EmpleadoEntity { Id = Guid.NewGuid() }));

            var service = new ServiceCollection();

            service.AddTransient(_ => empleadoReposMock.Object);

            service.ConfigurePersonasService(new DbSettings());
            var provider = service.BuildServiceProvider();
            var empleadoService = provider.GetRequiredService<IEmpleadoService>();
            var result = await empleadoService.Insert(new EmpleadoRequestDto
            {
                Nombre = "fake",
                TipoPersona = (global::Evaluacion.Aplicacion.Dto.Especificas.Personas.TipoPersona)TipoPersona.Natural,
                FechaNacimiento = DateTimeOffset.Now,
                FechaRegistro = DateTimeOffset.Now,
                TipoDocumentoId = Guid.Parse("581E3E67-82E2-4F1F-B379-9BD870DB669E"),
                CodigoEmpleado = "Prueba06",
                AreaId = Guid.NewGuid()
            }).ConfigureAwait(false);

            Assert.NotNull(result.ToString());
            Assert.NotEqual(default, result);
        }
        [Fact]
        [IntegrationTest]
        public async void Validar_TipoPersona_Empleado_IntegrationTest()
        {
            ServiceProvider provider = ServiceCollectionEmpleado();

            var areaService = provider.GetRequiredService<IAreaService>();
            var empleadoService = provider.GetRequiredService<IEmpleadoService>();
            var areaRepositorio = provider.GetRequiredService<IAreaRepositorio>();
            var mapper = provider.GetRequiredService<IMapper>();

            var dtoArea = new AreaRequestDto
            {
                Id = Guid.Parse("11111111-5717-4562-b3fc-2c963f66afa1"),
                NombreArea = "FakeArea1",
                EmpleadoResponsableId = Guid.NewGuid()
            };
            var area = areaRepositorio
                .SearchMatching<AreaEntity>(x => x.NombreArea == dtoArea.NombreArea)
                .FirstOrDefault();
            if (area != null || area != default)
                areaRepositorio.Delete(area);
            var guidArea = await areaService.Insert(dtoArea).ConfigureAwait(false);

            var dtoEmpleado = new EmpleadoRequestDto
            {
                Id = Guid.NewGuid(),
                Nombre = "fake",
                Apellido = "fake",
                NumeroTelefono = 123456789,
                CorreoElectronico = "fake@fake.fake",
                CodigoTipoDocumento = "0000000010",
                FechaNacimiento = DateTimeOffset.Now,
                FechaRegistro = DateTimeOffset.Now,
                TipoDocumentoId = Guid.Parse("581E3E67-82E2-4F1F-B379-9BD870DB669E"),
                Salario = 20000,
                AreaId = guidArea,
                CodigoEmpleado = "Prueba07"
            };


            dtoEmpleado.TipoPersona = 0;
            //Esta validacion no se va a sacar ya que el Empleado siempre sera Natural
            //await Assert.ThrowsAsync<EmpleadoTipoPersonaNullException>(() => empleadoService.Insert(dtoEmpleado)).ConfigureAwait(false);

            dtoEmpleado.TipoPersona = (global::Evaluacion.Aplicacion.Dto.Especificas.Personas.TipoPersona)TipoPersona.Natural;
            var response = await empleadoService.Insert(dtoEmpleado).ConfigureAwait(false);
            Assert.NotNull(response.ToString());
            Assert.NotEqual(default, response);

            _ = empleadoService.Delete(dtoEmpleado);
            var areaEnd = areaRepositorio
                .SearchMatching<AreaEntity>(x => x.NombreArea == dtoArea.NombreArea)
                .FirstOrDefault();
            areaRepositorio.Delete(areaEnd);
        }
        #endregion
        //TODO: Empleado, No puede haber dos personas con el mismo numero y tipo de identificación
        #region No_Se_Repite_CodigoTipoDocumento_Empleado
        #region Test Funcional
        //Se debe comentar el SearchMatching de nombre para que funcione 
        //[Fact]
        //[UnitTest]
        //public async Task No_Se_Repite_CodigoTipoDocumento_Empleado_Fail()
        //{
        //    var empleadoRepoMock = new Mock<IEmpleadoRepositorio>();

        //    var empleadoDto = new EmpleadoRequestDto
        //    {
        //        Nombre = "fake",
        //        FechaNacimiento = DateTimeOffset.Now,
        //        FechaRegistro = DateTimeOffset.Now,
        //        CodigoTipoDocumento = "123456789",
        //        TipoDocumentoId = Guid.Parse("581E3E67-82E2-4F1F-B379-9BD870DB669E")
        //    };


        //    empleadoRepoMock
        //        .Setup(m => m.SearchMatching(It.IsIn<Expression<Func<EmpleadoEntity, bool>>>(x => x.Nombre == empleadoDto.Nombre)).Any());

        //    empleadoRepoMock
        //        .Setup(m => m.SearchMatching(It.IsIn<Expression<Func<EmpleadoEntity, bool>>>(x => x.CodigoTipoDocumento == empleadoDto.CodigoTipoDocumento && x.TipoDocumentoId == empleadoDto.TipoDocumentoId)))
        //        .Returns(new List<EmpleadoEntity> { new EmpleadoEntity
        //        {
        //            Id = Guid.NewGuid(),
        //            Nombre = "FakePrueba",
        //            FechaNacimiento = DateTimeOffset.Now,
        //            FechaRegistro = DateTimeOffset.Now,
        //            CodigoTipoDocumento="123456789",
        //            TipoDocumentoId = Guid.Parse("581E3E67-82E2-4F1F-B379-9BD870DB669E")
        //        }});

        //    var service = new ServiceCollection();

        //    service.AddTransient(_ => empleadoRepoMock.Object);

        //    service.ConfigurePersonasService(new DbSettings());

        //    var provider = service.BuildServiceProvider();
        //    var empleadoService = provider.GetRequiredService<IEmpleadoService>();

        //    await Assert.ThrowsAsync<EmpleadoCodigoTipoDocumentoException>(() => empleadoService.Insert(empleadoDto)).ConfigureAwait(false);
        //}
        #endregion
        [Fact]
        [UnitTest]
        public async Task No_Se_Repite_CodigoTipoDocumento_Empleado_Full()
        {
            var empleadoReposMock = new Mock<IEmpleadoRepositorio>();

            empleadoReposMock
                .Setup(m => m.SearchMatching(It.IsAny<Expression<Func<EmpleadoEntity, bool>>>()));
            empleadoReposMock
                .Setup(m => m.Insert(It.IsAny<EmpleadoEntity>()))
                .Returns(Task.FromResult(new EmpleadoEntity { Id = Guid.NewGuid() }));

            var service = new ServiceCollection();

            service.AddTransient(_ => empleadoReposMock.Object);

            service.ConfigurePersonasService(new DbSettings());
            var provider = service.BuildServiceProvider();
            var empleadoService = provider.GetRequiredService<IEmpleadoService>();
            var result = await empleadoService.Insert(new EmpleadoRequestDto
            {
                Nombre = "fake",
                TipoPersona = (global::Evaluacion.Aplicacion.Dto.Especificas.Personas.TipoPersona)TipoPersona.Natural,
                FechaNacimiento = DateTimeOffset.Now,
                FechaRegistro = DateTimeOffset.Now,
                TipoDocumentoId = Guid.Parse("581E3E67-82E2-4F1F-B379-9BD870DB669E"),
                CodigoEmpleado = "Prueba02",
                AreaId = Guid.NewGuid()
            }).ConfigureAwait(false);

            Assert.NotNull(result.ToString());
            Assert.NotEqual(default, result);
        }
        [Fact]
        [IntegrationTest]
        public async void No_Se_Repite_CodigoTipoDocumento_Empleado_Full_IntegrationTest()
        {
            ServiceProvider provider = ServiceCollectionEmpleado();

            var areaService = provider.GetRequiredService<IAreaService>();
            var empleadoService = provider.GetRequiredService<IEmpleadoService>();
            var areaRepositorio = provider.GetRequiredService<IAreaRepositorio>();
            var mapper = provider.GetRequiredService<IMapper>();

            var dtoArea = new AreaRequestDto
            {
                Id = Guid.NewGuid(),
                NombreArea = "FakeArea2",
                EmpleadoResponsableId = Guid.NewGuid()
            };
            var area = areaRepositorio
                .SearchMatching<AreaEntity>(x => x.NombreArea == dtoArea.NombreArea)
                .FirstOrDefault();
            if (area != null || area != default)
                areaRepositorio.Delete(area);
            var guidArea = await areaService.Insert(dtoArea).ConfigureAwait(false);

            var dtoEmpleado = new EmpleadoRequestDto
            {
                Id = Guid.NewGuid(),
                Nombre = "Empleado_fake_1",
                Apellido = "Empleado_fake_1",
                NumeroTelefono = 123456789,
                CorreoElectronico = "fake@fake.fake",
                CodigoTipoDocumento = "000000009",
                TipoPersona = (global::Evaluacion.Aplicacion.Dto.Especificas.Personas.TipoPersona)TipoPersona.Natural,
                FechaNacimiento = DateTimeOffset.Now,
                FechaRegistro = DateTimeOffset.Now,
                TipoDocumentoId = Guid.Parse("581E3E67-82E2-4F1F-B379-9BD870DB669E"),
                Salario = 20000,
                AreaId = guidArea,
                CodigoEmpleado = "Prueba03"
            };
            var response = await empleadoService.Insert(dtoEmpleado).ConfigureAwait(false);
            Assert.NotNull(response.ToString());
            Assert.NotEqual(default, response);

            dtoEmpleado.Nombre = "Empleado_fake_Throws_1";
            dtoEmpleado.CodigoEmpleado = "Throws03";
            dtoEmpleado.AreaId = Guid.NewGuid();
            await Assert.ThrowsAsync<EmpleadoCodigoTipoDocumentoException>(() => empleadoService.Insert(dtoEmpleado)).ConfigureAwait(false);

            var dtoEmpleadoI2 = new EmpleadoRequestDto
            {
                Id = Guid.NewGuid(),
                Nombre = "Empleado_fake_2",
                Apellido = "Empleado_fake_2",
                NumeroTelefono = 223456789,
                CorreoElectronico = "fake2@fake2.fake2",
                CodigoTipoDocumento = "000000009",
                TipoPersona = (global::Evaluacion.Aplicacion.Dto.Especificas.Personas.TipoPersona)TipoPersona.Natural,
                FechaNacimiento = DateTimeOffset.Now,
                FechaRegistro = DateTimeOffset.Now,
                TipoDocumentoId = Guid.Parse("12427378-28E4-48CB-8ED7-097116F8064E"),
                Salario = 20000,
                AreaId = guidArea,
                CodigoEmpleado = "Prueba04"
            };

            var responseI2 = await empleadoService.Insert(dtoEmpleadoI2).ConfigureAwait(false);
            Assert.NotNull(responseI2.ToString());
            Assert.NotEqual(default, responseI2);

            _ = empleadoService.Delete(dtoEmpleado);
            _ = empleadoService.Delete(dtoEmpleadoI2);
            var areaEnd = areaRepositorio
                .SearchMatching<AreaEntity>(x => x.NombreArea == dtoArea.NombreArea)
                .FirstOrDefault();
            areaRepositorio.Delete(areaEnd);
        }
        #endregion
        //TODO: Empleado, No puede haber dos personas con el mismo nombre / razón social
        #region No_Se_Repite_Nombre_Empleado
        [Fact]
        [UnitTest]
        public async Task No_Se_Repite_Nombre_Empleado_Fail()
        {
            var empleadoDto = new EmpleadoRequestDto
            {
                Nombre = "fake",
                //TODO: Debe poderse distinguir entre jurídicas y naturales
                TipoPersona = (global::Evaluacion.Aplicacion.Dto.Especificas.Personas.TipoPersona)TipoPersona.Natural,
                FechaNacimiento = DateTimeOffset.Now,
                FechaRegistro = DateTimeOffset.Now,
                TipoDocumentoId = Guid.Parse("581E3E67-82E2-4F1F-B379-9BD870DB669E"),
                CodigoEmpleado = "Prueba05",
                AreaId = Guid.NewGuid()
            };
            var empleadoRepoMock = new Mock<IEmpleadoRepositorio>();

            empleadoRepoMock
                .Setup(m => m.SearchMatching(It.IsIn<Expression<Func<EmpleadoEntity, bool>>>(x => x.Id == empleadoDto.Id && x.AreaId == empleadoDto.AreaId)));
            empleadoRepoMock
                .Setup(m => m.SearchMatching(It.IsIn<Expression<Func<EmpleadoEntity, bool>>>(x => x.CodigoTipoDocumento == empleadoDto.CodigoTipoDocumento && x.TipoDocumentoId == empleadoDto.TipoDocumentoId)));
            empleadoRepoMock
                .Setup(m => m.SearchMatching(It.IsIn<Expression<Func<EmpleadoEntity, bool>>>(x => x.CodigoEmpleado == empleadoDto.CodigoEmpleado)));
            empleadoRepoMock
                .Setup(m => m.SearchMatching(It.IsIn<Expression<Func<EmpleadoEntity, bool>>>(x => x.Nombre == empleadoDto.Nombre)))
                .Returns(new List<EmpleadoEntity> { new EmpleadoEntity
                {
                    Id = Guid.NewGuid(),
                    Nombre = "FakePrueba",
                    AreaId = Guid.NewGuid()
                }});

            var service = new ServiceCollection();

            service.AddTransient(_ => empleadoRepoMock.Object);

            service.ConfigurePersonasService(new DbSettings());

            var provider = service.BuildServiceProvider();
            var empleadoService = provider.GetRequiredService<IEmpleadoService>();

            await Assert.ThrowsAsync<EmpleadonameAlreadyExistException>(() => empleadoService.Insert(empleadoDto)).ConfigureAwait(false);
        }
        [Fact]
        [UnitTest]
        public async Task No_Se_Repite_Nombre_Empleado_Full()
        {
            var empleadoReposMock = new Mock<IEmpleadoRepositorio>();

            empleadoReposMock
                .Setup(m => m.SearchMatching(It.IsAny<Expression<Func<EmpleadoEntity, bool>>>()));
            empleadoReposMock
                .Setup(m => m.Insert(It.IsAny<EmpleadoEntity>()))
                .Returns(Task.FromResult(new EmpleadoEntity { Id = Guid.NewGuid() }));

            var service = new ServiceCollection();

            service.AddTransient(_ => empleadoReposMock.Object);

            service.ConfigurePersonasService(new DbSettings());
            var provider = service.BuildServiceProvider();
            var empleadoService = provider.GetRequiredService<IEmpleadoService>();
            var result = await empleadoService.Insert(new EmpleadoRequestDto
            {
                Nombre = "fake",
                TipoPersona = (global::Evaluacion.Aplicacion.Dto.Especificas.Personas.TipoPersona)TipoPersona.Natural,
                FechaNacimiento = DateTimeOffset.Now,
                FechaRegistro = DateTimeOffset.Now,
                TipoDocumentoId = Guid.Parse("581E3E67-82E2-4F1F-B379-9BD870DB669E"),
                CodigoEmpleado = "Prueba06",
                AreaId = Guid.NewGuid()
            }).ConfigureAwait(false);

            Assert.NotNull(result.ToString());
            Assert.NotEqual(default, result);
        }
        [Fact]
        [IntegrationTest]
        public async void No_Se_Repite_Nombre_Empleado_Full_IntegrationTest()
        {
            ServiceProvider provider = ServiceCollectionEmpleado();

            var areaService = provider.GetRequiredService<IAreaService>();
            var empleadoService = provider.GetRequiredService<IEmpleadoService>();
            var areaRepositorio = provider.GetRequiredService<IAreaRepositorio>();
            var mapper = provider.GetRequiredService<IMapper>();

            var dtoArea = new AreaRequestDto
            {
                Id = Guid.Parse("11111111-5717-4562-b3fc-2c963f66afa1"),
                NombreArea = "FakeArea1",
                EmpleadoResponsableId = Guid.NewGuid()
            };
            var area = areaRepositorio
                .SearchMatching<AreaEntity>(x => x.NombreArea == dtoArea.NombreArea)
                .FirstOrDefault();
            if (area != null || area != default)
                areaRepositorio.Delete(area);
            var guidArea = await areaService.Insert(dtoArea).ConfigureAwait(false);

            var dtoEmpleado = new EmpleadoRequestDto
            {
                Id = Guid.NewGuid(),
                Nombre = "fake",
                Apellido = "fake",
                NumeroTelefono = 123456789,
                CorreoElectronico = "fake@fake.fake",
                CodigoTipoDocumento = "000000008",
                TipoPersona = (global::Evaluacion.Aplicacion.Dto.Especificas.Personas.TipoPersona)TipoPersona.Natural,
                FechaNacimiento = DateTimeOffset.Now,
                FechaRegistro = DateTimeOffset.Now,
                TipoDocumentoId = Guid.Parse("581E3E67-82E2-4F1F-B379-9BD870DB669E"),
                Salario = 20000,
                AreaId = guidArea,
                CodigoEmpleado = "Prueba07"
            };
            var response = await empleadoService.Insert(dtoEmpleado).ConfigureAwait(false);
            Assert.NotNull(response.ToString());
            Assert.NotEqual(default, response);
            dtoEmpleado.CodigoEmpleado = "Otro01";
            dtoEmpleado.AreaId = Guid.NewGuid();
            await Assert.ThrowsAsync<EmpleadonameAlreadyExistException>(() => empleadoService.Insert(dtoEmpleado)).ConfigureAwait(false);

            _ = empleadoService.Delete(dtoEmpleado);
            var areaEnd = areaRepositorio
                .SearchMatching<AreaEntity>(x => x.NombreArea == dtoArea.NombreArea)
                .FirstOrDefault();
            areaRepositorio.Delete(areaEnd);
        }
        #endregion
        //TODO: Empleado, La fecha de nacimiento / creación es obligatoria
        #region Empleado_Validar_Fechas
        [Fact]
        [UnitTest]
        public async Task Empleado_Validar_Fechas_Fail()
        {
            var empleadoReposMock = new Mock<IEmpleadoRepositorio>();

            empleadoReposMock
                .Setup(m => m.SearchMatching(It.IsAny<Expression<Func<EmpleadoEntity, bool>>>()));
            empleadoReposMock
                .Setup(m => m.Insert(It.IsAny<EmpleadoEntity>()))
                .Returns(Task.FromResult(new EmpleadoEntity { Id = Guid.NewGuid() }));

            var service = new ServiceCollection();

            service.AddTransient(_ => empleadoReposMock.Object);

            service.ConfigurePersonasService(new DbSettings());
            var provider = service.BuildServiceProvider();
            var empleadoService = provider.GetRequiredService<IEmpleadoService>();

            var empleadoDto = new EmpleadoRequestDto
            {
                Nombre = "FakePrueba",
                TipoPersona = (global::Evaluacion.Aplicacion.Dto.Especificas.Personas.TipoPersona)TipoPersona.Natural,
                FechaNacimiento = DateTimeOffset.Now,
                FechaRegistro = DateTimeOffset.Now,
                TipoDocumentoId = Guid.Parse("581E3E67-82E2-4F1F-B379-9BD870DB669E"),
                CodigoEmpleado = "Prueba08",
                AreaId = Guid.NewGuid()
            };

            empleadoDto.FechaNacimiento = default;
            await Assert.ThrowsAsync<EmpleadoFechaNacimientoException>(() => empleadoService.Insert(empleadoDto)).ConfigureAwait(false);

            empleadoDto.FechaNacimiento = DateTimeOffset.Now;
            empleadoDto.FechaRegistro = default;
            await Assert.ThrowsAsync<EmpleadoFechaRegistroException>(() => empleadoService.Insert(empleadoDto)).ConfigureAwait(false);
        }
        [Fact]
        [UnitTest]
        public async Task Empleado_Validar_Fechas_Full()
        {
            var empleadoReposMock = new Mock<IEmpleadoRepositorio>();

            empleadoReposMock
                .Setup(m => m.SearchMatching(It.IsAny<Expression<Func<EmpleadoEntity, bool>>>()));
            empleadoReposMock
                .Setup(m => m.Insert(It.IsAny<EmpleadoEntity>()))
                .Returns(Task.FromResult(new EmpleadoEntity { Id = Guid.NewGuid() }));

            var service = new ServiceCollection();

            service.AddTransient(_ => empleadoReposMock.Object);

            service.ConfigurePersonasService(new DbSettings());
            var provider = service.BuildServiceProvider();
            var empleadoService = provider.GetRequiredService<IEmpleadoService>();
            var result = await empleadoService.Insert(new EmpleadoRequestDto
            {
                Nombre = "fake",
                TipoPersona = (global::Evaluacion.Aplicacion.Dto.Especificas.Personas.TipoPersona)TipoPersona.Natural,
                FechaNacimiento = DateTimeOffset.Now,
                FechaRegistro = DateTimeOffset.Now,
                TipoDocumentoId = Guid.Parse("581E3E67-82E2-4F1F-B379-9BD870DB669E"),
                AreaId = Guid.NewGuid()
            }).ConfigureAwait(false);

            Assert.NotNull(result.ToString());
            Assert.NotEqual(default, result);
        }
        [Fact]
        [IntegrationTest]
        public async void Empleado_Validar_Fechas_Full_IntegrationTest()
        {
            ServiceProvider provider = ServiceCollectionEmpleado();

            var areaService = provider.GetRequiredService<IAreaService>();
            var empleadoService = provider.GetRequiredService<IEmpleadoService>();
            var areaRepositorio = provider.GetRequiredService<IAreaRepositorio>();
            var mapper = provider.GetRequiredService<IMapper>();

            var dtoArea = new AreaRequestDto
            {
                Id = Guid.Parse("11111111-5717-4562-b3fc-2c963f66afa1"),
                NombreArea = "FakeArea1",
                EmpleadoResponsableId = Guid.NewGuid()
            };
            var area = areaRepositorio
                .SearchMatching<AreaEntity>(x => x.NombreArea == dtoArea.NombreArea)
                .FirstOrDefault();
            if (area != null || area != default)
                areaRepositorio.Delete(area);
            var guidArea = await areaService.Insert(dtoArea).ConfigureAwait(false);

            var dtoEmpleado = new EmpleadoRequestDto
            {
                Id = Guid.NewGuid(),
                Nombre = "fake",
                Apellido = "fake",
                NumeroTelefono = 123456789,
                CorreoElectronico = "fake@fake.fake",
                CodigoTipoDocumento = "000000007",
                TipoPersona = (global::Evaluacion.Aplicacion.Dto.Especificas.Personas.TipoPersona)TipoPersona.Natural,
                FechaNacimiento = DateTimeOffset.Now,
                FechaRegistro = DateTimeOffset.Now,
                TipoDocumentoId = Guid.Parse("581E3E67-82E2-4F1F-B379-9BD870DB669E"),
                Salario = 20000,
                AreaId = guidArea,
                CodigoEmpleado = "Prueba10"
            };

            dtoEmpleado.FechaNacimiento = default;
            await Assert.ThrowsAsync<EmpleadoFechaNacimientoException>(() => empleadoService.Insert(dtoEmpleado)).ConfigureAwait(false);

            dtoEmpleado.FechaNacimiento = DateTimeOffset.Now;
            dtoEmpleado.FechaRegistro = default;
            await Assert.ThrowsAsync<EmpleadoFechaRegistroException>(() => empleadoService.Insert(dtoEmpleado)).ConfigureAwait(false);

            dtoEmpleado.FechaRegistro = DateTimeOffset.Now;
            var response = await empleadoService.Insert(dtoEmpleado).ConfigureAwait(false);
            Assert.NotNull(response.ToString());
            Assert.NotEqual(default, response);

            _ = empleadoService.Delete(dtoEmpleado);
            var areaEnd = areaRepositorio
                .SearchMatching<AreaEntity>(x => x.NombreArea == dtoArea.NombreArea)
                .FirstOrDefault();
            areaRepositorio.Delete(areaEnd);
        }
        #endregion
        //TODO: Empleado, Un empleado no puede ser persona jurídica
        #region Empleado_Tipo_no_juridica
        [Fact]
        [UnitTest]
        public async Task Empleado_Tipo_juridica()
        {
            var empleadoReposMock = new Mock<IEmpleadoRepositorio>();

            empleadoReposMock
                .Setup(m => m.SearchMatching(It.IsAny<Expression<Func<EmpleadoEntity, bool>>>()));
            empleadoReposMock
                .Setup(m => m.Insert(It.IsAny<EmpleadoEntity>()))
                .Returns(Task.FromResult(new EmpleadoEntity { Id = Guid.NewGuid() }));

            var service = new ServiceCollection();

            service.AddTransient(_ => empleadoReposMock.Object);

            service.ConfigurePersonasService(new DbSettings());
            var provider = service.BuildServiceProvider();
            var empleadoService = provider.GetRequiredService<IEmpleadoService>();

            var empleadoDto = new EmpleadoRequestDto
            {
                Nombre = "FakePrueba",
                TipoPersona = (global::Evaluacion.Aplicacion.Dto.Especificas.Personas.TipoPersona)TipoPersona.Juridico
            };
            Assert.Equal(nameof(TipoPersona.Natural), empleadoDto.TipoPersona.ToString());
        }
        [Fact]
        [UnitTest]
        public async Task Empleado_Tipo_no_juridica()
        {
            var empleadoReposMock = new Mock<IEmpleadoRepositorio>();

            empleadoReposMock
                .Setup(m => m.SearchMatching(It.IsAny<Expression<Func<EmpleadoEntity, bool>>>()));
            empleadoReposMock
                .Setup(m => m.Insert(It.IsAny<EmpleadoEntity>()))
                .Returns(Task.FromResult(new EmpleadoEntity { Id = Guid.NewGuid(), AreaId = Guid.NewGuid() }));

            var service = new ServiceCollection();

            service.AddTransient(_ => empleadoReposMock.Object);

            service.ConfigurePersonasService(new DbSettings());
            var provider = service.BuildServiceProvider();
            var empleadoService = provider.GetRequiredService<IEmpleadoService>();
            var result = await empleadoService.Insert(new EmpleadoRequestDto
            {
                Nombre = "fake",
                TipoPersona = (global::Evaluacion.Aplicacion.Dto.Especificas.Personas.TipoPersona)TipoPersona.Natural,
                FechaNacimiento = DateTimeOffset.Now,
                FechaRegistro = DateTimeOffset.Now,
                TipoDocumentoId = Guid.Parse("581E3E67-82E2-4F1F-B379-9BD870DB669E"),
                CodigoEmpleado = "Prueba11",
                AreaId = Guid.NewGuid()
            }).ConfigureAwait(false);

            Assert.NotNull(result.ToString());
            Assert.NotEqual(default, result);
        }
        [Fact]
        [IntegrationTest]
        public async void Empleado_Tipo_no_juridica_IntegrationTest()
        {
            ServiceProvider provider = ServiceCollectionEmpleado();

            var areaService = provider.GetRequiredService<IAreaService>();
            var empleadoService = provider.GetRequiredService<IEmpleadoService>();
            var areaRepositorio = provider.GetRequiredService<IAreaRepositorio>();
            var empleadoRepositorio = provider.GetRequiredService<IEmpleadoRepositorio>();
            var mapper = provider.GetRequiredService<IMapper>();

            var dtoArea = new AreaRequestDto
            {
                Id = Guid.Parse("11111111-5717-4562-b3fc-2c963f66afa1"),
                NombreArea = "FakeArea1",
                EmpleadoResponsableId = Guid.NewGuid()
            };
            var area = areaRepositorio
                .SearchMatching<AreaEntity>(x => x.NombreArea == dtoArea.NombreArea)
                .FirstOrDefault();
            if (area != null || area != default)
                areaRepositorio.Delete(area);
            var guidArea = await areaService.Insert(dtoArea).ConfigureAwait(false);

            var dtoEmpleado = new EmpleadoRequestDto
            {
                Id = Guid.NewGuid(),
                Nombre = "fake",
                Apellido = "fake",
                NumeroTelefono = 123456789,
                CorreoElectronico = "fake@fake.fake",
                CodigoTipoDocumento = "000000006",
                TipoPersona = (global::Evaluacion.Aplicacion.Dto.Especificas.Personas.TipoPersona)TipoPersona.Juridico,//El empleado siempre es Natural
                FechaNacimiento = DateTimeOffset.Now,
                FechaRegistro = DateTimeOffset.Now,
                TipoDocumentoId = Guid.Parse("581E3E67-82E2-4F1F-B379-9BD870DB669E"),
                Salario = 20000,
                AreaId = guidArea,
                CodigoEmpleado = "Prueba12"
            };
            var response = await empleadoService.Insert(dtoEmpleado).ConfigureAwait(false);
            Assert.NotNull(response.ToString());
            Assert.NotEqual(default, response);

            var empleadoR = empleadoRepositorio
                .SearchMatching<EmpleadoEntity>(x => x.Id == dtoEmpleado.Id)
                .FirstOrDefault();

            Assert.NotNull(empleadoR);
            Assert.NotEqual(TipoPersona.Juridico.ToString(), empleadoR.TipoPersona.ToString());

            _ = empleadoService.Delete(dtoEmpleado);
            var areaEnd = areaRepositorio
                .SearchMatching<AreaEntity>(x => x.NombreArea == dtoArea.NombreArea)
                .FirstOrDefault();
            areaRepositorio.Delete(areaEnd);
        }
        #endregion
        //TODO: Una persona no puede tener el tipo de documento nit 
        #region Empleado_Validar_NIT
        [Fact]
        [UnitTest]
        public async Task Empleado_Validar_NIT_Fail()
        {
            var empleadoReposMock = new Mock<IEmpleadoRepositorio>();

            empleadoReposMock
                .Setup(m => m.SearchMatching(It.IsAny<Expression<Func<EmpleadoEntity, bool>>>()));
            empleadoReposMock
                .Setup(m => m.Insert(It.IsAny<EmpleadoEntity>()))
                .Returns(Task.FromResult(new EmpleadoEntity { Id = Guid.NewGuid() }));

            var service = new ServiceCollection();

            service.AddTransient(_ => empleadoReposMock.Object);

            service.ConfigurePersonasService(new DbSettings());
            var provider = service.BuildServiceProvider();
            var empleadoService = provider.GetRequiredService<IEmpleadoService>();

            await Assert.ThrowsAsync<EmpleadoTipoDocumentoException>(() => empleadoService.Insert(new EmpleadoRequestDto
            {
                Nombre = "FakePrueba",
                TipoPersona = (global::Evaluacion.Aplicacion.Dto.Especificas.Personas.TipoPersona)TipoPersona.Natural,
                FechaNacimiento = DateTimeOffset.Now,
                FechaRegistro = DateTimeOffset.Now,
                TipoDocumentoId = Guid.Parse("A89DAA40-149F-439A-8A08-7842E09D7376"),
                CodigoEmpleado = "Prueba13",
                AreaId = Guid.NewGuid()
            })).ConfigureAwait(false);
        }
        [Fact]
        [UnitTest]
        public async Task Empleado_Validar_NIT_Full()
        {
            var empleadoReposMock = new Mock<IEmpleadoRepositorio>();

            empleadoReposMock
                .Setup(m => m.SearchMatching(It.IsAny<Expression<Func<EmpleadoEntity, bool>>>()));
            empleadoReposMock
                .Setup(m => m.Insert(It.IsAny<EmpleadoEntity>()))
                .Returns(Task.FromResult(new EmpleadoEntity { Id = Guid.NewGuid() }));

            var service = new ServiceCollection();

            service.AddTransient(_ => empleadoReposMock.Object);

            service.ConfigurePersonasService(new DbSettings());
            var provider = service.BuildServiceProvider();
            var empleadoService = provider.GetRequiredService<IEmpleadoService>();

            var result = await empleadoService.Insert(new EmpleadoRequestDto
            {
                Nombre = "fake",
                TipoPersona = (global::Evaluacion.Aplicacion.Dto.Especificas.Personas.TipoPersona)TipoPersona.Natural,
                FechaNacimiento = DateTimeOffset.Now,
                FechaRegistro = DateTimeOffset.Now,
                TipoDocumentoId = Guid.Parse("581E3E67-82E2-4F1F-B379-9BD870DB669E"),
                CodigoEmpleado = "Prueba06",
                AreaId = Guid.NewGuid()
            }).ConfigureAwait(false);
            Assert.NotNull(result.ToString());
            Assert.NotEqual(default, result);
        }
        [Fact]
        [IntegrationTest]
        public async void Empleado_Validar_NIT_Full_IntegrationTest()
        {
            ServiceProvider provider = ServiceCollectionEmpleado();

            var areaService = provider.GetRequiredService<IAreaService>();
            var empleadoService = provider.GetRequiredService<IEmpleadoService>();
            var areaRepositorio = provider.GetRequiredService<IAreaRepositorio>();
            var mapper = provider.GetRequiredService<IMapper>();

            var dtoArea = new AreaRequestDto
            {
                Id = Guid.Parse("11111111-5717-4562-b3fc-2c963f66afa1"),
                NombreArea = "FakeArea1",
                EmpleadoResponsableId = Guid.NewGuid()
            };
            var area = areaRepositorio
                .SearchMatching<AreaEntity>(x => x.NombreArea == dtoArea.NombreArea)
                .FirstOrDefault();
            if (area != null || area != default)
                areaRepositorio.Delete(area);
            var guidArea = await areaService.Insert(dtoArea).ConfigureAwait(false);

            var dtoEmpleado = new EmpleadoRequestDto
            {
                Id = Guid.NewGuid(),
                Nombre = "fake",
                Apellido = "fake",
                NumeroTelefono = 123456789,
                CorreoElectronico = "fake@fake.fake",
                CodigoTipoDocumento = "000000005",
                TipoPersona = (global::Evaluacion.Aplicacion.Dto.Especificas.Personas.TipoPersona)TipoPersona.Natural,
                FechaNacimiento = DateTimeOffset.Now,
                FechaRegistro = DateTimeOffset.Now,
                TipoDocumentoId = Guid.Parse("581E3E67-82E2-4F1F-B379-9BD870DB669E"),
                Salario = 20000,
                AreaId = guidArea,
                CodigoEmpleado = "Prueba15"
            };

            dtoEmpleado.TipoDocumentoId = Guid.Parse("A89DAA40-149F-439A-8A08-7842E09D7376");//Nit
            await Assert.ThrowsAsync<EmpleadoTipoDocumentoException>(() => empleadoService.Insert(dtoEmpleado)).ConfigureAwait(false);

            dtoEmpleado.TipoDocumentoId = Guid.Parse("581E3E67-82E2-4F1F-B379-9BD870DB669E");//Cedula
            var response = await empleadoService.Insert(dtoEmpleado).ConfigureAwait(false);
            Assert.NotNull(response.ToString());
            Assert.NotEqual(default, response);

            _ = empleadoService.Delete(dtoEmpleado);
            var areaEnd = areaRepositorio
                .SearchMatching<AreaEntity>(x => x.NombreArea == dtoArea.NombreArea)
                .FirstOrDefault();
            areaRepositorio.Delete(areaEnd);
        }
        #endregion
        //TODO: Deben tener un código único y estar asociados a un área.
        #region Emplado_Asociado_Area_Codigo_Unico
        [Fact]
        [UnitTest]
        public async Task Emplado_Codigo_Unico_Fail()
        {
            //TODO: Solucionar
            var empleadoDto = new EmpleadoRequestDto
            {
                Nombre = "FakePrueba",
                TipoPersona = (global::Evaluacion.Aplicacion.Dto.Especificas.Personas.TipoPersona)TipoPersona.Natural,
                FechaNacimiento = DateTimeOffset.Now,
                FechaRegistro = DateTimeOffset.Now,
                TipoDocumentoId = Guid.Parse("12427378-28e4-48cb-8ed7-097116f8064e"),
                CodigoEmpleado = "123456789",
                AreaId = Guid.NewGuid()
            };
            var empleadoReposMock = new Mock<IEmpleadoRepositorio>();

            empleadoReposMock
                .Setup(m => m.SearchMatching(It.IsIn<Expression<Func<EmpleadoEntity, bool>>>(x => x.Nombre == empleadoDto.Nombre)));
            empleadoReposMock
                .Setup(m => m.SearchMatching(It.IsIn<Expression<Func<EmpleadoEntity, bool>>>(x => x.Id == empleadoDto.Id && x.AreaId == empleadoDto.AreaId)));
            empleadoReposMock
                .Setup(m => m.SearchMatching(It.IsIn<Expression<Func<EmpleadoEntity, bool>>>(x => x.CodigoTipoDocumento == empleadoDto.CodigoTipoDocumento && x.TipoDocumentoId == empleadoDto.TipoDocumentoId)));
            empleadoReposMock
                .Setup(m => m.SearchMatching(It.IsIn<Expression<Func<EmpleadoEntity, bool>>>(x => x.CodigoEmpleado == empleadoDto.CodigoEmpleado)))
                .Returns(new List<EmpleadoEntity> { new EmpleadoEntity
                {
                    Id = Guid.NewGuid(),
                    Nombre = "FakePrueba",
                    CodigoEmpleado = "Prueba16"
                }});
            empleadoReposMock
                .Setup(m => m.Insert(It.IsAny<EmpleadoEntity>()))
                .Returns(Task.FromResult(new EmpleadoEntity { Id = Guid.NewGuid() }));

            var service = new ServiceCollection();

            service.AddTransient(_ => empleadoReposMock.Object);

            service.ConfigurePersonasService(new DbSettings());
            var provider = service.BuildServiceProvider();
            var empleadoService = provider.GetRequiredService<IEmpleadoService>();

            await Assert.ThrowsAsync<EmpleadocodeAlreadyExistException>(() => empleadoService.Insert(empleadoDto)).ConfigureAwait(false);
        }
        [Fact]
        [UnitTest]
        public async void Emplado_Codigo_Unico_Full()
        {
            var empleadoDto = new EmpleadoRequestDto
            {
                Nombre = "FakePrueba",
                TipoPersona = (global::Evaluacion.Aplicacion.Dto.Especificas.Personas.TipoPersona)TipoPersona.Natural,
                FechaNacimiento = DateTimeOffset.Now,
                FechaRegistro = DateTimeOffset.Now,
                TipoDocumentoId = Guid.Parse("12427378-28e4-48cb-8ed7-097116f8064e"),
                CodigoEmpleado = "123456789",
                AreaId = Guid.NewGuid()
            };
            var empleadoReposMock = new Mock<IEmpleadoRepositorio>();

            empleadoReposMock
                .Setup(m => m.SearchMatching(It.IsIn<Expression<Func<EmpleadoEntity, bool>>>(x => x.Nombre == empleadoDto.Nombre)));
            empleadoReposMock
                .Setup(m => m.SearchMatching(It.IsIn<Expression<Func<EmpleadoEntity, bool>>>(x => x.CodigoEmpleado == empleadoDto.CodigoEmpleado)));
            empleadoReposMock
                .Setup(m => m.Insert(It.IsAny<EmpleadoEntity>()))
                .Returns(Task.FromResult(new EmpleadoEntity { Id = Guid.NewGuid() }));

            var service = new ServiceCollection();

            service.AddTransient(_ => empleadoReposMock.Object);

            service.ConfigurePersonasService(new DbSettings());
            var provider = service.BuildServiceProvider();
            var empleadoService = provider.GetRequiredService<IEmpleadoService>();
            var result = await empleadoService.Insert(new EmpleadoRequestDto
            {
                Nombre = "fake",
                TipoPersona = (global::Evaluacion.Aplicacion.Dto.Especificas.Personas.TipoPersona)TipoPersona.Natural,
                FechaNacimiento = DateTimeOffset.Now,
                FechaRegistro = DateTimeOffset.Now,
                TipoDocumentoId = Guid.Parse("581E3E67-82E2-4F1F-B379-9BD870DB669E"),
                CodigoEmpleado = "123456789",
                AreaId = Guid.NewGuid()
            }).ConfigureAwait(false);

            Assert.NotNull(result.ToString());
            Assert.NotEqual(default, result);
        }
        [Fact]
        [UnitTest]
        public async Task Emplado_Asociado_Area_Fail()
        {
            var empleadoDto = new EmpleadoRequestDto
            {
                Nombre = "FakePrueba",
                TipoPersona = (global::Evaluacion.Aplicacion.Dto.Especificas.Personas.TipoPersona)TipoPersona.Natural,
                FechaNacimiento = DateTimeOffset.Now,
                FechaRegistro = DateTimeOffset.Now,
                TipoDocumentoId = Guid.Parse("12427378-28e4-48cb-8ed7-097116f8064e")
            };
            var empleadoReposMock = new Mock<IEmpleadoRepositorio>();

            empleadoReposMock
                .Setup(m => m.SearchMatching(It.IsIn<Expression<Func<EmpleadoEntity, bool>>>(x => x.Nombre == empleadoDto.Nombre)));
            empleadoReposMock
                .Setup(m => m.SearchMatching(It.IsIn<Expression<Func<EmpleadoEntity, bool>>>(x => x.CodigoEmpleado == empleadoDto.CodigoEmpleado)))
                .Returns(new List<EmpleadoEntity> { new EmpleadoEntity
                {
                    Id = Guid.NewGuid(),
                    Nombre = "FakePrueba",
                    CodigoEmpleado = "Prueba17"
                }});
            empleadoReposMock
                .Setup(m => m.Insert(It.IsAny<EmpleadoEntity>()))
                .Returns(Task.FromResult(new EmpleadoEntity { Id = Guid.NewGuid() }));

            var service = new ServiceCollection();

            service.AddTransient(_ => empleadoReposMock.Object);

            service.ConfigurePersonasService(new DbSettings());
            var provider = service.BuildServiceProvider();
            var empleadoService = provider.GetRequiredService<IEmpleadoService>();

            await Assert.ThrowsAsync<EmpleadoAreaIdNullException>(() => empleadoService.Insert(empleadoDto)).ConfigureAwait(false);
        }
        [Fact]
        [UnitTest]
        public async Task Emplado_Asociado_Area_Unica_Fail()
        {
            var empleadoDto = new EmpleadoRequestDto
            {
                Nombre = "FakePrueba",
                TipoPersona = (global::Evaluacion.Aplicacion.Dto.Especificas.Personas.TipoPersona)TipoPersona.Natural,
                FechaNacimiento = DateTimeOffset.Now,
                FechaRegistro = DateTimeOffset.Now,
                TipoDocumentoId = Guid.Parse("12427378-28e4-48cb-8ed7-097116f8064e"),
                AreaId = Guid.NewGuid()
            };
            var empleadoReposMock = new Mock<IEmpleadoRepositorio>();

            empleadoReposMock
                .Setup(m => m.SearchMatching(It.IsIn<Expression<Func<EmpleadoEntity, bool>>>(x => x.Nombre == empleadoDto.Nombre)));
            empleadoReposMock
                .Setup(m => m.SearchMatching(It.IsIn<Expression<Func<EmpleadoEntity, bool>>>(x => x.CodigoTipoDocumento == empleadoDto.CodigoTipoDocumento && x.TipoDocumentoId == empleadoDto.TipoDocumentoId)));
            empleadoReposMock
                .Setup(m => m.SearchMatching(It.IsIn<Expression<Func<EmpleadoEntity, bool>>>(x => x.CodigoEmpleado == empleadoDto.CodigoEmpleado)));
            empleadoReposMock
                .Setup(m => m.SearchMatching(It.IsIn<Expression<Func<EmpleadoEntity, bool>>>(x => x.Id == empleadoDto.Id && x.AreaId == empleadoDto.AreaId)))
                .Returns(new List<EmpleadoEntity> { new EmpleadoEntity
                {
                    Id = Guid.NewGuid(),
                    Nombre = "FakePrueba",
                    CodigoEmpleado = "Prueba17"
                }});
            empleadoReposMock
                .Setup(m => m.Insert(It.IsAny<EmpleadoEntity>()))
                .Returns(Task.FromResult(new EmpleadoEntity { Id = Guid.NewGuid() }));

            var service = new ServiceCollection();

            service.AddTransient(_ => empleadoReposMock.Object);

            service.ConfigurePersonasService(new DbSettings());
            var provider = service.BuildServiceProvider();
            var empleadoService = provider.GetRequiredService<IEmpleadoService>();

            await Assert.ThrowsAsync<EmpleadoAreaIdAlreadyExistException>(() => empleadoService.Insert(empleadoDto)).ConfigureAwait(false);
        }
        [Fact]
        [UnitTest]
        public async void Emplado_Asociado_Area_Full()
        {
            var empleadoDto = new EmpleadoRequestDto
            {
                Nombre = "FakePrueba",
                TipoPersona = (global::Evaluacion.Aplicacion.Dto.Especificas.Personas.TipoPersona)TipoPersona.Natural,
                FechaNacimiento = DateTimeOffset.Now,
                FechaRegistro = DateTimeOffset.Now,
                TipoDocumentoId = Guid.Parse("12427378-28e4-48cb-8ed7-097116f8064e"),
                AreaId = Guid.NewGuid()
            };
            var empleadoReposMock = new Mock<IEmpleadoRepositorio>();

            empleadoReposMock
                .Setup(m => m.SearchMatching(It.IsIn<Expression<Func<EmpleadoEntity, bool>>>(x => x.Nombre == empleadoDto.Nombre)));
            empleadoReposMock
                .Setup(m => m.SearchMatching(It.IsIn<Expression<Func<EmpleadoEntity, bool>>>(x => x.CodigoEmpleado == empleadoDto.CodigoEmpleado)))
                .Returns(new List<EmpleadoEntity> { new EmpleadoEntity
                {
                    Id = Guid.NewGuid(),
                    Nombre = "FakePrueba",
                    CodigoEmpleado = "Prueba18"
                }});
            empleadoReposMock
                .Setup(m => m.Insert(It.IsAny<EmpleadoEntity>()))
                .Returns(Task.FromResult(new EmpleadoEntity { Id = Guid.NewGuid() }));

            var service = new ServiceCollection();

            service.AddTransient(_ => empleadoReposMock.Object);

            service.ConfigurePersonasService(new DbSettings());
            var provider = service.BuildServiceProvider();
            var empleadoService = provider.GetRequiredService<IEmpleadoService>();
            var result = await empleadoService.Insert(empleadoDto).ConfigureAwait(false);

            Assert.NotNull(result.ToString());
            Assert.NotEqual(default, result);
        }
        [Fact]
        [IntegrationTest]
        public async void Emplado_Asociado_Codigo_Unico_IntegrationTest()
        {
            ServiceProvider provider = ServiceCollectionEmpleado();

            var areaService = provider.GetRequiredService<IAreaService>();
            var empleadoService = provider.GetRequiredService<IEmpleadoService>();
            var areaRepositorio = provider.GetRequiredService<IAreaRepositorio>();
            var mapper = provider.GetRequiredService<IMapper>();

            var dtoArea = new AreaRequestDto
            {
                Id = Guid.Parse("11111111-5717-4562-b3fc-2c963f66afa1"),
                NombreArea = "FakeArea1",
                EmpleadoResponsableId = Guid.NewGuid()
            };
            var area = areaRepositorio
                .SearchMatching<AreaEntity>(x => x.NombreArea == dtoArea.NombreArea)
                .FirstOrDefault();
            if (area != null || area != default)
                areaRepositorio.Delete(area);
            var guidArea = await areaService.Insert(dtoArea).ConfigureAwait(false);

            var dtoEmpleado = new EmpleadoRequestDto
            {
                Id = Guid.NewGuid(),
                Nombre = "fake_empleado_area_1",
                Apellido = "fake_empleado_area_1",
                NumeroTelefono = 123456789,
                CorreoElectronico = "fake@fake.fake",
                CodigoTipoDocumento = "000000004",
                TipoPersona = (global::Evaluacion.Aplicacion.Dto.Especificas.Personas.TipoPersona)TipoPersona.Natural,
                FechaNacimiento = DateTimeOffset.Now,
                FechaRegistro = DateTimeOffset.Now,
                TipoDocumentoId = Guid.Parse("581E3E67-82E2-4F1F-B379-9BD870DB669E"),
                Salario = 20000,
                CodigoEmpleado = "Prueba19"
            };

            await Assert.ThrowsAsync<EmpleadoAreaIdNullException>(() => empleadoService.Insert(dtoEmpleado)).ConfigureAwait(false);

            dtoEmpleado.AreaId = guidArea;
            var response = await empleadoService.Insert(dtoEmpleado).ConfigureAwait(false);
            Assert.NotNull(response.ToString());
            Assert.NotEqual(default, response);

            await Assert.ThrowsAsync<EmpleadocodeAlreadyExistException>(() => empleadoService.Insert(dtoEmpleado)).ConfigureAwait(false);

            _ = empleadoService.Delete(dtoEmpleado);
            var areaEnd = areaRepositorio
                .SearchMatching<AreaEntity>(x => x.NombreArea == dtoArea.NombreArea)
                .FirstOrDefault();
            areaRepositorio.Delete(areaEnd);
        }
        [Fact]
        [IntegrationTest]
        public async void Emplado_Asociado_Area_Unica_IntegrationTest()
        {
            ServiceProvider provider = ServiceCollectionEmpleado();

            var areaService = provider.GetRequiredService<IAreaService>();
            var empleadoService = provider.GetRequiredService<IEmpleadoService>();
            var areaRepositorio = provider.GetRequiredService<IAreaRepositorio>();
            var mapper = provider.GetRequiredService<IMapper>();

            var dtoArea = new AreaRequestDto
            {
                Id = Guid.Parse("11111111-5717-4562-b3fc-2c963f66afa1"),
                NombreArea = "FakeArea1",
                EmpleadoResponsableId = Guid.NewGuid()
            };
            var area = areaRepositorio
                .SearchMatching<AreaEntity>(x => x.NombreArea == dtoArea.NombreArea)
                .FirstOrDefault();
            if (area != null || area != default)
                areaRepositorio.Delete(area);
            var guidArea = await areaService.Insert(dtoArea).ConfigureAwait(false);

            var dtoEmpleado = new EmpleadoRequestDto
            {
                Id = Guid.NewGuid(),
                Nombre = "fake_empleado_area_1",
                Apellido = "fake_empleado_area_1",
                NumeroTelefono = 123456789,
                CorreoElectronico = "fake@fake.fake",
                CodigoTipoDocumento = "000000003",
                TipoPersona = (global::Evaluacion.Aplicacion.Dto.Especificas.Personas.TipoPersona)TipoPersona.Natural,
                FechaNacimiento = DateTimeOffset.Now,
                FechaRegistro = DateTimeOffset.Now,
                TipoDocumentoId = Guid.Parse("581E3E67-82E2-4F1F-B379-9BD870DB669E"),
                Salario = 20000,
                CodigoEmpleado = "Prueba19"
            };

            await Assert.ThrowsAsync<EmpleadoAreaIdNullException>(() => empleadoService.Insert(dtoEmpleado)).ConfigureAwait(false);

            dtoEmpleado.AreaId = guidArea;
            var response = await empleadoService.Insert(dtoEmpleado).ConfigureAwait(false);
            Assert.NotNull(response.ToString());
            Assert.NotEqual(default, response);

            dtoEmpleado.CodigoEmpleado = "Prueba19_Throws";
            await Assert.ThrowsAsync<EmpleadoAreaIdAlreadyExistException>(() => empleadoService.Insert(dtoEmpleado)).ConfigureAwait(false);

            _ = empleadoService.Delete(dtoEmpleado);
            var areaEnd = areaRepositorio
                .SearchMatching<AreaEntity>(x => x.NombreArea == dtoArea.NombreArea)
                .FirstOrDefault();
            areaRepositorio.Delete(areaEnd);
        }
        #endregion
        //TODO: Test de integracion para empleado
        [Fact]
        [IntegrationTest]
        public async void Validacion_Parametros_Empleado_Integration()
        {
            ServiceProvider provider = ServiceCollectionEmpleado();

            var areaService = provider.GetRequiredService<IAreaService>();
            var empleadoService = provider.GetRequiredService<IEmpleadoService>();
            var areaRepositorio = provider.GetRequiredService<IAreaRepositorio>();
            var mapper = provider.GetRequiredService<IMapper>();

            var dtoArea = new AreaRequestDto
            {
                Id = Guid.Parse("11111111-5717-4562-b3fc-2c963f66afa1"),
                NombreArea = "FakeArea1",
                EmpleadoResponsableId = Guid.NewGuid()
            };
            var area = areaRepositorio
                .SearchMatching<AreaEntity>(x => x.NombreArea == dtoArea.NombreArea)
                .FirstOrDefault();
            if (area != null || area != default)
                areaRepositorio.Delete(area);
            var guidArea = await areaService.Insert(dtoArea).ConfigureAwait(false);

            var dtoEmpleado = new EmpleadoRequestDto
            {
                Id = Guid.NewGuid(),
                Nombre = "fake",
                Apellido = "fake",
                NumeroTelefono = 123456789,
                CorreoElectronico = "fake@fake.fake",
                CodigoTipoDocumento = "000000001",
                TipoPersona = (global::Evaluacion.Aplicacion.Dto.Especificas.Personas.TipoPersona)TipoPersona.Natural,
                FechaNacimiento = DateTimeOffset.Now,
                FechaRegistro = DateTimeOffset.Now,
                TipoDocumentoId = Guid.Parse("581E3E67-82E2-4F1F-B379-9BD870DB669E"),
                Salario = 20000,
                AreaId = guidArea,
                CodigoEmpleado = "Prueba19"
            };
            var response = await empleadoService.Insert(dtoEmpleado).ConfigureAwait(false);
            Assert.NotNull(response.ToString());
            Assert.NotEqual(default, response);

            dtoEmpleado.CodigoEmpleado = "Prueba19_Throws";
            await Assert.ThrowsAsync<EmpleadoAreaIdAlreadyExistException>(() => empleadoService.Insert(dtoEmpleado)).ConfigureAwait(false);

            dtoEmpleado.AreaId = Guid.NewGuid();
            dtoEmpleado.CodigoEmpleado = "Otro01";
            await Assert.ThrowsAsync<EmpleadonameAlreadyExistException>(() => empleadoService.Insert(dtoEmpleado)).ConfigureAwait(false);

            dtoEmpleado.Nombre = "Fake2";
            await Assert.ThrowsAsync<EmpleadoCodigoTipoDocumentoException>(() => empleadoService.Insert(dtoEmpleado)).ConfigureAwait(false);

            dtoEmpleado.CodigoTipoDocumento = "000000002";
            dtoEmpleado.FechaNacimiento = default;
            await Assert.ThrowsAsync<EmpleadoFechaNacimientoException>(() => empleadoService.Insert(dtoEmpleado)).ConfigureAwait(false);

            dtoEmpleado.FechaNacimiento = DateTimeOffset.Now;
            dtoEmpleado.FechaRegistro = default;
            await Assert.ThrowsAsync<EmpleadoFechaRegistroException>(() => empleadoService.Insert(dtoEmpleado)).ConfigureAwait(false);

            dtoEmpleado.FechaRegistro = DateTimeOffset.Now;
            dtoEmpleado.TipoDocumentoId = Guid.Parse("A89DAA40-149F-439A-8A08-7842E09D7376");
            await Assert.ThrowsAsync<EmpleadoTipoDocumentoException>(() => empleadoService.Insert(dtoEmpleado)).ConfigureAwait(false);

            _ = empleadoService.Delete(dtoEmpleado);
            var areaEnd = areaRepositorio
                .SearchMatching<AreaEntity>(x => x.NombreArea == dtoArea.NombreArea)
                .FirstOrDefault();
            areaRepositorio.Delete(areaEnd);
        }
    }
}
