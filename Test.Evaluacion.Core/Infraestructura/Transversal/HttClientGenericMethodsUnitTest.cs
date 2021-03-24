using Evaluacion.Infraestructura.Transversal.MetodosGenericos;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using Xunit.Categories;

namespace Test.Evaluacion.Core.Infraestructura.Transversal
{
    public class HttClientGenericMethodsUnitTest
    {
        [Fact]
        [UnitTest]
        public async Task Throws_UriFormatException_on_settings_null_or_empty()
        {
            _ = await Assert.ThrowsAsync<UriFormatException>(() => Task.FromResult(new HttpGenericClient(null, new HttpClient()))).ConfigureAwait(false);
        }
    }
}
