using System;
using System.Diagnostics;
using SignalR.Client;

namespace NLog.Targets.SignalR
{
    public class PersistentConnectionPublisher: PublisherBase, IPublishToSignalR
    {
        public PersistentConnectionPublisher(Connection persistentConnection)
        {
            PersistentConnection = persistentConnection;
        }


        public virtual void Connect()
        {
            PersistentConnection.Start().ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    if (task.Exception != null)
                        Trace.WriteLine(String.Format("Failed to start: {0}", task.Exception.GetBaseException()));
                }
                else
                {
                    Trace.WriteLine(String.Format("Success! Connected with client connection id {0}", PersistentConnection.ConnectionId));
                    
                    IsConnected = true;
                    //Sending test data
                    StartProcessing();
                    
                }
            });
        }


        protected override void SendToSignalR(Message message)
        {
            //persistent connection
            PersistentConnection.Send(message.Content).ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    if (task.Exception != null)
                        Trace.WriteLine(String.Format("Send failed {0}", task.Exception.GetBaseException()));
                }
                else
                {
                    Trace.WriteLine("Success");
                }
            });
        }
    }
}