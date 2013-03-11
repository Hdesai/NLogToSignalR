using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NLog.Targets.SignalR
{
    public abstract class PublisherBase
    {
        protected readonly ConcurrentQueue<Action> FunctionsToExecute = new ConcurrentQueue<Action>();

        private bool _isConnected;

        public bool SentToSignalR { get; protected set; }

        public bool IsConnected
        {
            get { return _isConnected; }

            set { _isConnected = value; }
        }

        protected virtual void StartProcessing()
        {
            while (FunctionsToExecute.Count > 0)
            {
                Action functionToExecute;
                if (FunctionsToExecute.TryDequeue(out functionToExecute))
                {
                    functionToExecute.Invoke();
                }
            }
        }

        public void WriteToQueue(LogLevel logLevel, string message)
        {
            if (!_isConnected)
            {
                //If it is not connected-queue the Message
                FunctionsToExecute.Enqueue(() => SendTheMessageToRemoteHost(logLevel, new[] {message}));
            }
            else
            {
                //Just send the message to remote host
                SendTheMessageToRemoteHost(logLevel, new[] {message});
            }
        }

        protected virtual Task GetSendTheMessageToRemoteHostTask(LogLevel level, IEnumerable<string> messages)
        {
            var task = new Task(() =>
                {
                    var builder = new StringBuilder();
                    foreach (string message in messages)
                    {
                        builder.AppendLine(message);
                    }

                    string allTexts = builder.ToString();

                    var signalRMessage = new Message {Title = level.Name, Content = allTexts};

                    if (level == LogLevel.Info)
                    {
                        signalRMessage.IsInformation = true;
                    }

                    if (level == LogLevel.Warn)
                    {
                        signalRMessage.IsWarning = true;
                    }

                    if (level == LogLevel.Error)
                    {
                        signalRMessage.IsError = true;
                    }

                    SendToSignalR(signalRMessage);
                });

            return task;
        }

        protected virtual void SendTheMessageToRemoteHost(LogLevel level, IEnumerable<string> messages)
        {
            Task task = GetSendTheMessageToRemoteHostTask(level, messages);

            task.Start();
        }

        protected abstract void SendToSignalR(Message message);
    }
}