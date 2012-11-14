namespace NLog.Targets.SignalR
{
    public interface IPublishToSignalR
    {
        void Connect();
        void WriteToQueue(LogLevel logLevel, string message);
    }
}