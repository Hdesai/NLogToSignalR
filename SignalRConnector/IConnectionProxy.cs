using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.AspNet.SignalR.Client.Http;
using Microsoft.AspNet.SignalR.Client.Transports;

namespace SignalRConnector
{
    public interface IConnectionProxy:IConnection
    {
        Task StartConnection(IClientTransport clientTransport);
        Task StartConnection(IHttpClient client);
    }
}