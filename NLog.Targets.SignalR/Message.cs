namespace NLog.Targets.SignalR
{
    public class Message
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public int Duration { get; set; }
        public bool IsError { get; set; }
        public bool IsWarning { get; set; }
        public bool IsInformation { get; set; }
    }
}

