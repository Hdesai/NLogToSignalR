using System;
using System.Diagnostics;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.AspNet.SignalR.Client.Http;

namespace NLog.Targets.SignalR
{
    public class PersistentConnectionPublisher : PublisherBase, IPublishToSignalR
    {
        private readonly IConnectionProxy _connectionProxy;

        public PersistentConnectionPublisher(IConnectionProxy connectionProxy)
        {
            _connectionProxy = connectionProxy;
        }


        public virtual void Connect(IHttpClient httpClient)
        {
            _connectionProxy.StartConnection(httpClient).ContinueWith(task =>
                {
                    if ((!task.IsFaulted))
                    {
                        Trace.WriteLine(String.Format("Success! Connected with client connectionProxy id {0}",
                                                      _connectionProxy.ConnectionId));

                        IsConnected = true;
                        //Sending test data
                        StartProcessing();
                    }
                    else
                    {
                        if (task.Exception != null)
                            Trace.WriteLine(String.Format("Failed to start: {0}", task.Exception.GetBaseException()));
                    }
                }).Wait();

            if (_connectionProxy.State != ConnectionState.Connected)
            {
                Trace.WriteLine(String.Format("Failed to connect"));
            }
        }


        protected override void SendToSignalR(Message message)
        {
            //persistent connectionProxy
            _connectionProxy.Send(message.Content).ContinueWith(task =>
                {
                    if (task.IsFaulted)
                    {
                        if (task.Exception != null)
                            Trace.WriteLine(String.Format("Send failed {0}", task.Exception.GetBaseException()));
                    }
                    else
                    {
                        SentToSignalR = true;
                        Trace.WriteLine("Success");
                    }
                }).Wait();
        }
    }
}