using Evaluacion.Aplicacion.Core.AdministracionPersonas.Genericas.Areas.Services;
using Evaluacion.Aplicacion.Core.AdministracionPersonas.Genericas.Configuration;
using Evaluacion.Infraestructura.Datos.Persistencia.Core.Base.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Xunit;
using Xunit.Categories;

namespace Test.Evaluacion.Core.Aplicacion.Core.AdministracionPersonas.Genericas.Areas
{
    public class AreaServiceTest
    {
        [Fact]
        [UnitTest]
        public async Task Check_AllParameterNull_UpdateArea_Exception()
        {
            var service = new ServiceCollection();
            service.ConfigureGenericasService(new DbSettings());
            var provider = service.BuildServiceProvider();
            var areaService = provider.GetRequiredService<IAreaService>();
            await Assert.ThrowsAsync<AreaRequestDtoNullException>(() => Task.FromResult(areaService.UpdateArea(null))).ConfigureAwait(false);
        }
    }
}
