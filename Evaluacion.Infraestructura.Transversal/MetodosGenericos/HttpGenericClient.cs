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

        private readonly string urlBase;

        public HttpGenericClient(IOptions<HttpClientSettings> settings, HttpClient client)
        {
            if (settings == null)
                throw new UriFormatException();
            _client = client ?? throw new HttpClientNotDefinedException();
            urlBase = settings.Value.GetServiceUrl().ToString();
        }

        public async Task<T> Get<T>(string path, string request) where T : DataTransferObject
        {
            ValidatePath(path);
            var response = await _client.GetAsync($"{urlBase}{path}").ConfigureAwait(false);
            ValidateUserUnauthorized(response);
            response.EnsureSuccessStatusCode();
            return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        }

        public async Task<T> GetAll<T>(string path) where T : DataTransferObject
        {
            ValidatePath(path);
            var response = await _client.GetAsync($"{urlBase}{path}").ConfigureAwait(false);
            ValidateUserUnauthorized(response);
            response.EnsureSuccessStatusCode();
            return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        }

        public async Task<TResponse> Patch<TResponse, TRequest>(string path, TRequest request) where TRequest : DataTransferObject
        {
            ValidatePath(path);
            var stringRequest = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = await _client.PatchAsync(path, stringRequest).ConfigureAwait(false);
            ValidateUserUnauthorized(response);
            response.EnsureSuccessStatusCode();
            return JsonConvert.DeserializeObject<TResponse>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        }

        public async Task<TResponse> Post<TResponse, TRequest>(string path, TRequest request) where TRequest : DataTransferObject
        {
            ValidatePath(path);
            var stringRequest = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = await _client.PostAsync(path, stringRequest).ConfigureAwait(false);
            ValidateUserUnauthorized(response);
            response.EnsureSuccessStatusCode();
            return JsonConvert.DeserializeObject<TResponse>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        }

        public async Task<TResponse> Put<TResponse, TRequest>(string path, TRequest request) where TRequest : DataTransferObject
        {
            ValidatePath(path);
            var stringRequest = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = await _client.PutAsync(path, stringRequest).ConfigureAwait(false);
            ValidateUserUnauthorized(response);
            response.EnsureSuccessStatusCode();
            return JsonConvert.DeserializeObject<TResponse>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        }

        public async Task<T> Delete<T>(string path) where T : DataTransferObject
        {
            ValidatePath(path);
            var response = await _client.DeleteAsync(path).ConfigureAwait(false);
            ValidateUserUnauthorized(response);
            response.EnsureSuccessStatusCode();
            return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        }

        private static void ValidatePath(string path)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentPathException($"Parameter: {nameof(path)} required");
        }

        private static void ValidateUserUnauthorized(HttpResponseMessage response)
        {
            if (string.Equals(response.StatusCode.ToString(), "unauthorized", StringComparison.OrdinalIgnoreCase))
                throw new UserUnauthorizedException();
        }
    }
}
