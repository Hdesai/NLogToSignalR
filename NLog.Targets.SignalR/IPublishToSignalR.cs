using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client.Http;

namespace NLog.Targets.SignalR
{
    public interface IPublishToSignalR
    {
        void Connect(IHttpClient httpClient);
        void WriteToQueue(LogLevel logLevel, string message);
    }
}