using Evaluacion.Aplicacion.Core.IntegracionPersonas.Cofiguration;
using Evaluacion.Aplicacion.Core.IntegracionPersonas.Exceptions;
using Evaluacion.Aplicacion.Dto.Base;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Evaluacion.Aplicacion.Core.IntegracionPersonas
{
    public class IntegracionPersonaService : IIntegracionPersonaService
    {
        private readonly HttpClient _client;
        public IntegracionPersonaService(IOptions<IntegracionPersonaSettings> settings, HttpClient client)
        {
            if (settings == null)
                throw new UriFormatException();
            _client = client ?? throw new IntegracionPersonaNotDefinedException();
            _client.BaseAddress = settings.Value.GetServiceUrl();
        }
        public async Task<string> Export<TRequest>(string path, TRequest request) where TRequest : DataTransferObject
        {
            ValidatePath(path);
            var result = (JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json").ToString();

            string pathTxt = @"D:\MyTestClient.txt";

            // Create the file, or overwrite if the file exists.
            using (FileStream fs = File.Create(pathTxt))
            {
                byte[] info = new UTF8Encoding(true).GetBytes(result);
                // Add some information to the file.
                fs.Write(info, 0, info.Length);
            }

            // Open the stream and read it back.
            using (StreamReader sr = File.OpenText(pathTxt))
            {
                string s = "";
                while ((s = sr.ReadLine()) != null)
                {
                    Console.WriteLine(s);
                }
            }
            return result;
        }

        public async Task<TResponse> Import<TResponse, TRequest>(string path, TRequest request) where TResponse : DataTransferObject
        {
            var stringRequest = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = await _client.PostAsync(path, stringRequest).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            return JsonConvert.DeserializeObject<TResponse>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        }
        private static void ValidatePath(string path)
        {
            if (string.IsNullOrEmpty(path))
                throw new IntegracionPersonaArgumentPathException($"Parameter: {nameof(path)} required");
        }

    }
}
