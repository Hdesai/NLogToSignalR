using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using NLog.Common;
using NLog.Config;
using SignalR.Client;
using SignalR.Client.Hubs;

namespace NLog.Targets.SignalR
{
    [Target("NLogSignalR")]
    public class SignalRTarget : TargetWithLayout
    {
        public IPublishToSignalR PublishToSignalR { get; set; }

        public SignalRTarget()
        {
        }

        [RequiredParameter]
        public string Uri { get; set; }

        public bool CallViaPersistentConnection { get; set; }

        public string HubName { get; set; }
        public string GroupName { get; set; }
        public string MethodToCall { get; set; }

        protected override void InitializeTarget()
        {
            base.InitializeTarget();
            SetPublisher();

            PublishToSignalR.Connect();
        }

        private void SetPublisher()
        {
            if (PublishToSignalR!=null)
            {
                return;
            }

            if (string.IsNullOrEmpty(Uri))
            {
                throw new ArgumentNullException(Uri);
            }


            //Try to set appropriate publisher if not set
            if (CallViaPersistentConnection)
            {
                PublishToSignalR = new PersistentConnectionPublisher(new Connection(Uri));
            }
            else
            {
                if (string.IsNullOrEmpty(GroupName) || string.IsNullOrEmpty(MethodToCall) || string.IsNullOrEmpty(HubName))
                {
                   throw new ArgumentException("GroupName,MethodToCall & HubName are mandatory when CallViaPersistentConnection is set to false");
                }

                PublishToSignalR = new PublishToHub(GroupName, MethodToCall, Uri, HubName);
            }
        }

        protected override void Write(AsyncLogEventInfo logEvent)
        {
            base.Write(logEvent);
            
            string asyncBody = Layout.Render(logEvent.LogEvent);
            PublishToSignalR.WriteToQueue(logEvent.LogEvent.Level, asyncBody);
        }

    }
    //[Target("NLogSignalR")]
    //public class SignalRTarget : TargetWithLayout
    //{
    //    private readonly ConcurrentQueue<Action> _functionsToExecute = new ConcurrentQueue<Action>();
    //    private IHubProxy _myHub;
    //    private Connection _persistentConnection;
    //    private bool _shouldStartProcessing;

    //    [RequiredParameter]
    //    public string Uri { get; set; }

    //    public bool CallViaPersistentConnection { get; set; }

    //    [RequiredParameter]
    //    public string HubName { get; set; }

    //    [RequiredParameter]
    //    public string GroupName { get; set; }

    //    public string MethodToCall { get; set; }

    //    protected override void InitializeTarget()
    //    {
    //        base.InitializeTarget();
    //        if (CallViaPersistentConnection)
    //        {
    //            _persistentConnection = new Connection(Uri);
    //            _persistentConnection.Start().ContinueWith(task =>
    //                {
    //                    if (task.IsFaulted)
    //                    {
    //                        if (task.Exception != null)
    //                            Trace.WriteLine(String.Format("Failed to start: {0}", task.Exception.GetBaseException()));
    //                    }
    //                    else
    //                    {
    //                        Trace.WriteLine(String.Format("Success! Connected with client connection id {0}",_persistentConnection.ConnectionId));

    //                        //Sending test data
    //                        _shouldStartProcessing = true;
    //                        StartProcessing();
    //                    }
    //                });
    //        }
    //        else
    //        {
    //            SendToHub();
    //            StartProcessing();
    //        }
    //    }

    //    private void SendToHub()
    //    {
    //        var connection = new HubConnection(Uri);
    //        _myHub = connection.CreateProxy(HubName);
    //        connection.Start().Wait();
    //        if (connection.State != ConnectionState.Connected)
    //        {
    //            throw new NLogToSignalRTargetException(String.Format("Problem connection {0}",Uri));
    //        }
    //    }

    //    protected override void Write(AsyncLogEventInfo logEvent)
    //    {
    //        base.Write(logEvent);
    //        string asyncBody = Layout.Render(logEvent.LogEvent);

    //        if (!_shouldStartProcessing)
    //        {
    //            _functionsToExecute.Enqueue(() => SendTheMessageToRemoteHost(logEvent.LogEvent.Level, new[] {asyncBody}));
    //        }
    //    }

    //    private void StartProcessing()
    //    {
    //        while (_functionsToExecute.Count > 0)
    //        {
    //            Action functionToExecute;
    //            if (_functionsToExecute.TryDequeue(out functionToExecute))
    //            {
    //                functionToExecute.Invoke();
    //            }
    //        }
    //    }

    //    private void SendTheMessageToRemoteHost(LogLevel level, IEnumerable<string> messages)
    //    {
    //        var task = new Task(() =>
    //            {
    //                var builder = new StringBuilder();
    //                foreach (string message in messages)
    //                {
    //                    builder.AppendLine(message);
    //                }

    //                string allTexts = builder.ToString();

    //                var signalRMessage = new Message {Title = level.Name, Content = allTexts};

    //                SendToSignalR(signalRMessage);
    //            });

    //        task.Start();
    //    }

    //    private void SendToSignalR(Message message)
    //    {
    //        if (CallViaPersistentConnection)
    //        {
    //            //persistent connection
    //            _persistentConnection.Send(message.Content).ContinueWith(task =>
    //            {
    //                if (task.IsFaulted)
    //                {
    //                    if (task.Exception != null)
    //                        Trace.WriteLine(String.Format("Send failed {0}", task.Exception.GetBaseException()));
    //                }
    //                else
    //                {
    //                    Trace.WriteLine("Success");
    //                }
    //            });
    //        }
    //        else
    //        {
    //            Object[] myData = { message, GroupName };
    //            _myHub.Invoke(MethodToCall, myData);
    //        }
    //    }

        
    //}
}