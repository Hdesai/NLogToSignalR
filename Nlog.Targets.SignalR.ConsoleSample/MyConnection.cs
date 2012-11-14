using System.Threading.Tasks;
using SignalR;

namespace Nlog.Targets.SignalR.ConsoleSample
{
    internal class MyConnection : PersistentConnection
    {
        protected override Task OnReceivedAsync(IRequest request, string connectionId, string data)
        {
            return Connection.Broadcast(data);
        }
    }
}