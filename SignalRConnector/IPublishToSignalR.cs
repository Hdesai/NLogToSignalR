using Microsoft.AspNet.SignalR.Client.Http;

namespace SignalRConnector
{
    public interface IPublishToSignalR
    {
        void Connect(IHttpClient httpClient);
        void WriteToQueue(LogLevel logLevel, string message);
    }

    public enum LogLevel
    {
        Debug=1,
        Info=2,
        Warn=3,
        Error=4,
        Critical=5
    }
}