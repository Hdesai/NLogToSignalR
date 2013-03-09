using System;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.AspNet.SignalR.Client.Hubs;


namespace NLog.Targets.SignalR
{
    public class PublishToHub : PublisherBase, IPublishToSignalR
    {
        private readonly string _groupName;
        private readonly string _methodToCallOnServer;
        private IHubProxy _hubProxy;
        private readonly string _uri;
        private readonly string _hubName;

        public PublishToHub(string groupName,string methodToCallOnServer,string uri,string hubName)
        {
            _groupName = groupName;
            _methodToCallOnServer = methodToCallOnServer;
            _uri = uri;
            _hubName = hubName;
        }

        public void Connect()
        {
            var connection = new HubConnection(_uri);
            _hubProxy = connection.CreateHubProxy(_hubName);
            connection.Start().Wait();
            if (connection.State != ConnectionState.Connected)
            {
                throw new NLogToSignalRTargetException(String.Format("Problem connection {0}", _uri));
            }

            //Let the base class know that you can start processing
            //and not have to queue up the calls
            IsConnected = true;
        }

        protected override void SendToSignalR(Message message)
        {
            Object[] myData = { message, _groupName };
            _hubProxy.Invoke(_methodToCallOnServer, myData);
        }
    }

   
}