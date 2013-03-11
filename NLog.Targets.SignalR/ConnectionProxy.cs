using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.AspNet.SignalR.Client.Http;
using Microsoft.AspNet.SignalR.Client.Transports;

namespace NLog.Targets.SignalR
{
    public class ConnectionProxy:Connection,IConnectionProxy
    {
        public ConnectionProxy(string url) : base(url)
        {
        }

        public ConnectionProxy(string url, IDictionary<string, string> queryString) : base(url, queryString)
        {
        }

        public ConnectionProxy(string url, string queryString) : base(url, queryString)
        {
        }

        public Task StartConnection(IClientTransport clientTransport )
        {
            return base.Start(clientTransport);
        }

        public Task StartConnection(IHttpClient httpClient)
        {
            return base.Start(httpClient);

        }
    }
}