using Evaluacion.Aplicacion.Core.IntegracionPersonas.Exceptions;
using Evaluacion.Aplicacion.Dto.Base;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Evaluacion.Aplicacion.Core.IntegracionPersonas
{
    public class IntegracionPersonaService : IIntegracionPersonaService
    {
        private readonly HttpClient _client;
        public IntegracionPersonaService(HttpClient client)
        {
            _client = client ?? throw new IntegracionPersonaNotDefinedException();
        }
        public async Task<string> ExportJson<TRequest>(string path, TRequest request) where TRequest : DataTransferObject
        {
            ValidatePath(path);
            var result = (JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json").ToString();

            string pathTxt = @"D:\" + path + ".txt";

            using (FileStream fs = File.Create(pathTxt))
            {
                byte[] info = new UTF8Encoding(true).GetBytes(result);
                fs.Write(info, 0, info.Length);
            }
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

        public Task<TResponse> ImportJson<TResponse>(string path) where TResponse : DataTransferObject
        {
            string pathTxt = @"D:\MyTestClient.txt";
            var request = "";
            using (StreamReader sr = File.OpenText(pathTxt))
            {
                string s = "";
                while ((s = sr.ReadLine()) != null)
                {
                    Console.WriteLine(s);
                    request = s;
                }
            }
            //var stringRequest = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            //_client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //var response = await _client.PostAsync(path, stringRequest).ConfigureAwait(false);
            //response.EnsureSuccessStatusCode();
            //return JsonConvert.DeserializeObject<TResponse>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
            var result = JsonConvert.DeserializeObject<TResponse>(request);
            return Task.FromResult(result);
        }
        private static void ValidatePath(string path)
        {
            if (string.IsNullOrEmpty(path))
                throw new IntegracionPersonaArgumentPathException($"Parameter: {nameof(path)} required");
        }

    }
}
