using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.AspNet.SignalR.Client.Http;
using Microsoft.AspNet.SignalR.Client.Transports;

namespace NLog.Targets.SignalR
{
    public interface IConnectionProxy:IConnection
    {
        Task StartConnection(IClientTransport clientTransport);
        Task StartConnection(IHttpClient client);
    }
}