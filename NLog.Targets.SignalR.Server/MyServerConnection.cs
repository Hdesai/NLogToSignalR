using System;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;

namespace NLog.Targets.SignalR.SelfHostServer
{
    public class MyServerConnection : PersistentConnection
    {
        public static readonly string Authenticated = "authenticated";
        public static readonly string Unauthenticated = "unauthenticated";

        public static readonly string GroupName = "messaging";

        protected override Task OnConnected(IRequest request, string connectionId)
        {
            Console.WriteLine("{0} Connected", connectionId);
            return base.OnConnected(request, connectionId);
        }

        protected override Task OnDisconnected(IRequest request, string connectionId)
        {
            Console.WriteLine("{0} disconnected", connectionId);
            return base.OnDisconnected(request, connectionId);
        }

        //protected override Task OnError(Exception error)
        //{
        //    Console.WriteLine("Error Received {0}",error);
        //    return base....OnError(error);
        //}

        protected override Task OnReceived(IRequest request, string connectionId, string data)
        {
            //Broadcast
            //Connection.Broadcast(data);

            Console.WriteLine("Data Received from client {0}", data);
            Groups.Send(GroupName, data);
            return base.OnReceived(request, connectionId, data);
        }
    }
}