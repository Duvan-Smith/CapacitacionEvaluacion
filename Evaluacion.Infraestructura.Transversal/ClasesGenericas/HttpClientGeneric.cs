using Evaluacion.Aplicacion.Dto.Base;
using Evaluacion.Infraestructura.Transversal.Exceptions;
using Evaluacion.Infraestructura.Transversal.MetodosGenericos.Cofiguration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Evaluacion.Infraestructura.Transversal.ClasesGenericas
{
    public abstract class HttpClientGeneric<TRequest> : IHttpClientGeneric<TRequest> where TRequest : DataTransferObject
    {
        protected abstract string Controller { get; }

        private readonly HttpClient _client;
        private readonly string baseUrl;

        public HttpClientGeneric(
            HttpClient client,
            IOptions<HttpClientSettings> settings)
        {
            _client = client ?? throw new HttpClientNotDefinedException();
            if (settings.Value.GetServiceUrl() == null) throw new UriFormatException();
            baseUrl = settings.Value.GetServiceUrl().ToString();
        }

        public async Task<IEnumerable<TRequest>> Get(string path)
        {
            ValidateNotNullPath(path);
            var response = await _client.GetAsync($"{baseUrl}{Controller}/{path}").ConfigureAwait(false);
            ValidateUserUnauthorized(response);
            response.EnsureSuccessStatusCode();
            return JsonConvert.DeserializeObject<IEnumerable<TRequest>>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        }

        public async Task<TRequest> Patch(TRequest request)
        {
            ValidateNotNullPath(Controller);
            var stringRequest = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = await _client.PatchAsync(Controller, stringRequest).ConfigureAwait(false);
            ValidateUserUnauthorized(response);
            response.EnsureSuccessStatusCode();
            return JsonConvert.DeserializeObject<TRequest>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        }

        public async Task<TResponse> Post<TResponse>(string path, TRequest request)
        {
            ValidateNotNullPath(Controller);
            var stringRequest = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = await _client.PostAsync($"{baseUrl}{Controller}/{path}", stringRequest).ConfigureAwait(false);
            ValidateUserUnauthorized(response);
            response.EnsureSuccessStatusCode();
            return JsonConvert.DeserializeObject<TResponse>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        }

        public async Task<TRequest> Put(TRequest request)
        {
            ValidateNotNullPath(Controller);
            var stringRequest = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = await _client.PutAsync(Controller, stringRequest).ConfigureAwait(false);
            ValidateUserUnauthorized(response);
            response.EnsureSuccessStatusCode();
            return JsonConvert.DeserializeObject<TRequest>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        }
        public async Task<TRequest> Delete()
        {
            ValidateNotNullPath(Controller);
            var response = await _client.DeleteAsync(Controller).ConfigureAwait(false);
            ValidateUserUnauthorized(response);
            response.EnsureSuccessStatusCode();
            return JsonConvert.DeserializeObject<TRequest>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        }

        #region Validations
        private static void ValidateNotNullPath(string path)
        {
            if (string.IsNullOrEmpty(path)) throw new ArgumentPathException($"Parameter: {nameof(path)} required");
        }
        private static void ValidateUserUnauthorized(HttpResponseMessage response)
        {
            if (string.Equals(response.StatusCode.ToString(), "unauthorized", StringComparison.OrdinalIgnoreCase))
                throw new UserUnauthorizedException();
        }
        #endregion
    }
}
