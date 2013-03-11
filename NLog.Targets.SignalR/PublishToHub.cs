using System;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.AspNet.SignalR.Client.Http;
using Microsoft.AspNet.SignalR.Client.Hubs;


namespace NLog.Targets.SignalR
{
    public class PublishToHub : PublisherBase, IPublishToSignalR
    {
        private readonly string _groupName;
        private readonly string _methodToCallOnServer;
        protected IHubProxy _hubProxy;
        private readonly string _uri;
        private readonly string _hubName;

        private readonly IHubConnectionProxy _hubConnectionProxy;

        public PublishToHub(IHubConnectionProxy hubConnectionProxy, string groupName, string methodToCallOnServer,
                             string hubName)
        {
            _hubConnectionProxy = hubConnectionProxy;
            _groupName = groupName;
            _methodToCallOnServer = methodToCallOnServer;
            _hubName = hubName;
        }


        public void Connect(IHttpClient httpClient)
        {
            
            _hubProxy = _hubConnectionProxy.CreateProxy(_hubName);

            _hubConnectionProxy.StartConnection(httpClient).Wait();

            if (_hubConnectionProxy.State != ConnectionState.Connected)
            {
                throw new NLogToSignalRTargetException("Problem with connection");
            }

            //Let the base class know that you can start processing
            //and not have to queue up the calls
            IsConnected = true;
        }

        protected override void SendToSignalR(Message message)
        {
            if (_hubConnectionProxy.State != ConnectionState.Connected)
            {
                throw new NLogToSignalRTargetException("Problem with connection");
            }

            Object[] myData = {message, _groupName};
            _hubProxy.Invoke(_methodToCallOnServer, myData);

            SentToSignalR = true;
        }
    }
}