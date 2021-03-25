using Evaluacion.Aplicacion.Core.AdministracionPersonas.Personas.Configuration;
using Evaluacion.Aplicacion.Core.AdministracionPersonas.Personas.Empleados.Excepciones;
using Evaluacion.Aplicacion.Core.AdministracionPersonas.Personas.Empleados.Services;
using Evaluacion.Aplicacion.Dto.Especificas.Empleados;
using Evaluacion.Dominio.Core.Especificas.Empleados;
using Evaluacion.Infraestructura.Datos.Persistencia.Core.Base.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;
using Xunit.Categories;

namespace Test.Evaluacion.Core.Aplicacion.Core.AdministracionPersonas.Personas.Empleados
{
    public class EmpleadoServiceTest
    {
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
        [Fact]
        [UnitTest]
        public async Task No_Se_Repite_Identificacion_Empleado_Fail()
        {
            var empleadoRepoMock = new Mock<IEmpleadoRepositorio>();
            var empleadoidRepoMock = new Mock<IEmpleadoRepositorio>();

            empleadoRepoMock
                .Setup(m => m.SearchMatching(It.IsAny<Expression<Func<EmpleadoEntity, bool>>>()));
            empleadoidRepoMock
                .Setup(m => m.SearchMatching(It.IsAny<Expression<Func<EmpleadoEntity, bool>>>()))
                .Returns(new List<EmpleadoEntity> { new EmpleadoEntity
                {
                    Id = Guid.NewGuid(),
                    Nombre = "FakePrueba"
                }});

            var service = new ServiceCollection();

            service.AddTransient(_ => empleadoRepoMock.Object);
            service.AddTransient(_ => empleadoidRepoMock.Object);

            service.ConfigurePersonasService(new DbSettings());

            var provider = service.BuildServiceProvider();
            var empleadoService = provider.GetRequiredService<IEmpleadoService>();

            var empleadoDto = new EmpleadoRequestDto
            {
                Id = Guid.NewGuid(),
                Nombre = "FakePrueba"
            };

            await Assert.ThrowsAsync<EmpleadonameAlreadyExistException>(() => empleadoService.Insert(empleadoDto)).ConfigureAwait(false);
        }
        [Fact]
        [UnitTest]
        public async Task No_Se_Repite_Identificacion_Empleado_Full()
        {
            var empleadoGetRepoMock = new Mock<IEmpleadoRepositorio>();
            var empleadoInsertRepoMock = new Mock<IEmpleadoRepositorio>();

            empleadoGetRepoMock
                .Setup(m => m.SearchMatching(It.IsAny<Expression<Func<EmpleadoEntity, bool>>>()));
            empleadoInsertRepoMock
                .Setup(m => m.Insert(It.IsAny<EmpleadoEntity>()))
                .Returns(Task.FromResult(new EmpleadoEntity { Id = Guid.NewGuid() }));

            var service = new ServiceCollection();

            service.AddTransient(_ => empleadoGetRepoMock.Object);
            service.AddTransient(_ => empleadoInsertRepoMock.Object);

            service.ConfigurePersonasService(new DbSettings());
            var provider = service.BuildServiceProvider();
            var empleadoService = provider.GetRequiredService<IEmpleadoService>();
            var result = await empleadoService.Insert(new EmpleadoRequestDto
            {
                Nombre = "fake",
            }).ConfigureAwait(false);

            Assert.NotNull(result.ToString());
            Assert.NotEqual(default, result);
        }
        [Fact]
        [UnitTest]
        public async Task No_Se_Repite_Nombre_Empleado_Fail()
        {
            var empleadoRepoMock = new Mock<IEmpleadoRepositorio>();

            empleadoRepoMock
                .Setup(m => m.SearchMatching(It.IsAny<Expression<Func<EmpleadoEntity, bool>>>()))
                .Returns(new List<EmpleadoEntity> { new EmpleadoEntity
                {
                    Id = Guid.NewGuid(),
                    Nombre = "FakePrueba"
                }});

            var service = new ServiceCollection();

            service.AddTransient(_ => empleadoRepoMock.Object);

            service.ConfigurePersonasService(new DbSettings());

            var provider = service.BuildServiceProvider();
            var empleadoService = provider.GetRequiredService<IEmpleadoService>();

            var empleadoDto = new EmpleadoRequestDto
            {
                Nombre = "FakePrueba"
            };

            await Assert.ThrowsAsync<EmpleadonameAlreadyExistException>(() => empleadoService.Insert(empleadoDto)).ConfigureAwait(false);
        }
        [Fact]
        [UnitTest]
        public async Task No_Se_Repite_Nombre_Empleado_Full()
        {
            var empleadoGetRepoMock = new Mock<IEmpleadoRepositorio>();
            var empleadoInsertRepoMock = new Mock<IEmpleadoRepositorio>();

            empleadoGetRepoMock
                .Setup(m => m.SearchMatching(It.IsAny<Expression<Func<EmpleadoEntity, bool>>>()));
            empleadoInsertRepoMock
                .Setup(m => m.Insert(It.IsAny<EmpleadoEntity>()))
                .Returns(Task.FromResult(new EmpleadoEntity { Id = Guid.NewGuid() }));

            var service = new ServiceCollection();

            service.AddTransient(_ => empleadoGetRepoMock.Object);
            service.AddTransient(_ => empleadoInsertRepoMock.Object);

            service.ConfigurePersonasService(new DbSettings());
            var provider = service.BuildServiceProvider();
            var empleadoService = provider.GetRequiredService<IEmpleadoService>();
            var result = await empleadoService.Insert(new EmpleadoRequestDto
            {
                Nombre = "fake",
            }).ConfigureAwait(false);

            Assert.NotNull(result.ToString());
            Assert.NotEqual(default, result);
        }
    }
}
