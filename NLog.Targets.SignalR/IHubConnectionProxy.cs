using Microsoft.AspNet.SignalR.Client;
using Microsoft.AspNet.SignalR.Client.Hubs;

namespace NLog.Targets.SignalR
{
    public interface IHubConnectionProxy:IConnectionProxy
    {
        IHubProxy CreateProxy(string hubName);

    }
}