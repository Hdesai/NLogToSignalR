using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SignalR;

namespace NLog.Targets.SignalR.SelfHostServer
{
    public  class MyServerConnection:PersistentConnection
    {
        public static readonly string Authenticated = "authenticated";
        public static readonly string Unauthenticated = "unauthenticated";

        public static readonly string GroupName = "messaging";

        protected override Task OnConnectedAsync(IRequest request, string connectionId)
        {
            Console.WriteLine("{0} Connected",connectionId);
            return base.OnConnectedAsync(request, connectionId);
        }

        protected override Task OnDisconnectAsync(string connectionId)
        {
            Console.WriteLine("{0} disconnected", connectionId);
            return base.OnDisconnectAsync(connectionId);
        }

        public override Task ProcessRequestAsync(HostContext context)
        {
            return base.ProcessRequestAsync(context);
        }

        public override void Initialize(IDependencyResolver resolver)
        {
            base.Initialize(resolver);
        }

        protected override Connection CreateConnection(string connectionId, IEnumerable<string> groups, IRequest request)
        {
            return base.CreateConnection(connectionId, groups, request);
        }

        protected override Task OnErrorAsync(Exception error)
        {
            Console.WriteLine("Error Received {0}",error);
            return base.OnErrorAsync(error);
        }

        protected override Task OnReceivedAsync(IRequest request, string connectionId, string data)
        {
            //Broadcast
            //Connection.Broadcast(data);

            Console.WriteLine("Data Received from client {0}", data);
            Groups.Send(GroupName, data);
            return base.OnReceivedAsync(request, connectionId, data);
        }

        protected override Task OnReconnectedAsync(IRequest request, IEnumerable<string> groups, string connectionId)
        {
            return base.OnReconnectedAsync(request, groups, connectionId);
        }

        protected override System.Diagnostics.TraceSource Trace
        {
            get
            {
                return base.Trace;
            }
        }
    }
}