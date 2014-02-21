using Microsoft.AspNet.SignalR.Client;

namespace SignalRConnector
{
    public interface IHubConnectionProxy:IConnectionProxy
    {
        IHubProxy CreateProxy(string hubName);

    }
}