using Evaluacion.Infraestructura.Transversal.Exceptions;
using Evaluacion.Infraestructura.Transversal.MetodosGenericos.Cofiguration;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
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
            if (client.BaseAddress == null)
                throw new HttpClientNotDefinedException();
            _client = client;
            _client.BaseAddress = settings.Value.GetServiceUrl();
        }
        public Task<T> Get<T>(string path, string request) where T : class
        {
            throw new System.NotImplementedException();
        }

        public Task<T> GetAll<T>(string path) where T : class
        {
            throw new System.NotImplementedException();
        }

        public Task<T> Patch<T>(string path, T request) where T : class
        {
            throw new System.NotImplementedException();
        }

        public Task<TResponse> Post<TResponse, TRequest>(string path, TRequest request) where TResponse : class
        {
            throw new System.NotImplementedException();
        }

        public Task<T> Put<T>(string path, T request) where T : class
        {
            throw new System.NotImplementedException();
        }
    }
}
