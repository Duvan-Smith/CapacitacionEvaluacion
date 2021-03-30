using System;

namespace Evaluacion.Aplicacion.Core.IntegracionPersonas.Cofiguration
{
    public class IntegracionPersonaSettings
    {
        public string ServiceProtocol { get; set; }
        public string Hostname { get; set; }
        public int Port { get; set; }
        public string Context { get; set; }

        public void CopyFrom(IntegracionPersonaSettings settings)
        {
            ServiceProtocol = settings.ServiceProtocol;
            Port = settings.Port;
            Context = settings.Context;
            Hostname = settings.Hostname;
        }

        public Uri GetServiceUrl() => new UriBuilder
        {
            Host = Hostname,
            Port = Port,
            Path = Context,
            Scheme = ServiceProtocol
        }.Uri;
    }
}
