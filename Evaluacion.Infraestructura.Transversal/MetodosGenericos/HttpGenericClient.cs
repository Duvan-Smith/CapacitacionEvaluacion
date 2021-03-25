using Evaluacion.Aplicacion.Dto.Base;
using Evaluacion.Infraestructura.Transversal.Exceptions;
using Evaluacion.Infraestructura.Transversal.MetodosGenericos.Cofiguration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Evaluacion.Infraestructura.Transversal.MetodosGenericos
{
    public class HttpGenericClient : IHttpGenericClient
    {
        private readonly HttpClient _client;
        public HttpGenericClient(IOptions<HttpClientSettings> settings, HttpClient client)
        {
            if (settings == null)
                throw new UriFormatException();
            _client = client ?? throw new HttpClientNotDefinedException();
            _client.BaseAddress = settings.Value.GetServiceUrl();
        }
        public async Task<T> Get<T>(string path, string request) where T : DataTransferObject
        {
            ValidatePath(path);
            var response = await _client.GetAsync(path).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        }

        public async Task<T> GetAll<T>(string path) where T : DataTransferObject
        {
            ValidatePath(path);
            var response = await _client.GetAsync(path).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        }
        //TODO: impementar metodo
        public Task<T> Patch<T>(string path, T request) where T : DataTransferObject
        {
            ValidatePath(path);
#pragma warning disable RCS1079 // Throwing of new NotImplementedException.
            throw new System.NotImplementedException();
#pragma warning restore RCS1079 // Throwing of new NotImplementedException.
        }

        public async Task<TResponse> Post<TResponse, TRequest>(string path, TRequest request) where TResponse : DataTransferObject
        {
            ValidatePath(path);
            var stringRequest = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = await _client.PostAsync(path, stringRequest).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            return JsonConvert.DeserializeObject<TResponse>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        }
        //TODO: impementar metodo
        public Task<T> Put<T>(string path, T request) where T : DataTransferObject
        {
            ValidatePath(path);
#pragma warning disable RCS1079 // Throwing of new NotImplementedException.
            throw new System.NotImplementedException();
#pragma warning restore RCS1079 // Throwing of new NotImplementedException.
        }
        private static void ValidatePath(string path)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentPathException($"Parameter: {nameof(path)} required");
        }
    }
}
