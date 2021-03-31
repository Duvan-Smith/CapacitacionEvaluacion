﻿using Evaluacion.Aplicacion.Core.IntegracionPersonas.Exceptions;
using Evaluacion.Aplicacion.Dto.Base;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Evaluacion.Aplicacion.Core.IntegracionPersonas
{
    public class IntegracionPersonaService : IIntegracionPersonaService
    {
        public Task<string> ExportJson<TRequest>(string path, TRequest request) where TRequest : IEnumerable<DataTransferObject>
        {
            ValidatePath(path);
            var result = JsonConvert.SerializeObject(request);

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
            return Task.FromResult(result);
        }
        public Task<TResponse> ImportJson<TResponse>(string path) where TResponse : IEnumerable<DataTransferObject>
        {
            var request = "";
            string pathTxt = @"D:\" + path + ".txt";

            using (StreamReader sr = File.OpenText(pathTxt))
            {
                string s = "";
                while ((s = sr.ReadLine()) != null)
                {
                    Console.WriteLine(s);
                    request = s;
                }
            }
            return Task.FromResult(JsonConvert.DeserializeObject<TResponse>(request));
        }
        private static void ValidatePath(string path)
        {
            if (string.IsNullOrEmpty(path))
                throw new IntegracionPersonaArgumentPathException($"Parameter: {nameof(path)} required");
        }

    }
}
