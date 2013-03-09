using System;
using Microsoft.AspNet.SignalR.Client;
using NLog.Common;
using NLog.Config;

namespace NLog.Targets.SignalR
{
    [Target("NLogSignalR")]
    public class SignalRTarget : TargetWithLayout
    {
        public IPublishToSignalR PublishToSignalR { get; set; }

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
            if (PublishToSignalR != null)
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
                if (string.IsNullOrEmpty(GroupName) || string.IsNullOrEmpty(MethodToCall) ||
                    string.IsNullOrEmpty(HubName))
                {
                    throw new ArgumentException(
                        "GroupName,MethodToCall & HubName are mandatory when CallViaPersistentConnection is set to false");
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
}