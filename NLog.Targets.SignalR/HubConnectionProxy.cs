using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client.Http;
using Microsoft.AspNet.SignalR.Client.Hubs;
using Microsoft.AspNet.SignalR.Client.Transports;

namespace NLog.Targets.SignalR
{
    public class HubConnectionProxy:HubConnection,IHubConnectionProxy
    {
        public HubConnectionProxy(string url) : base(url)
        {
        }

        public HubConnectionProxy(string url, bool useDefaultUrl) : base(url, useDefaultUrl)
        {
        }

        public HubConnectionProxy(string url, string queryString) : base(url, queryString)
        {
        }

        public HubConnectionProxy(string url, string queryString, bool useDefaultUrl) : base(url, queryString, useDefaultUrl)
        {
        }

        public HubConnectionProxy(string url, IDictionary<string, string> queryString) : base(url, queryString)
        {
        }

        public HubConnectionProxy(string url, IDictionary<string, string> queryString, bool useDefaultUrl) : base(url, queryString, useDefaultUrl)
        {
        }

        public Task StartConnection(IClientTransport clientTransport)
        {
            return base.Start(clientTransport);
        }

        public Task StartConnection(IHttpClient client)
        {
            return base.Start(client);
        }

        public IHubProxy CreateProxy(string hubName)
        {
            return base.CreateHubProxy(hubName);
        }
    }
}