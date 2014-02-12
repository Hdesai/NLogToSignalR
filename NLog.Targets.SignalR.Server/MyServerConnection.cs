using System;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Owin;

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

    class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //// Turn cross domain on 
            var config = new ConnectionConfiguration {EnableJSONP = true};

            //// This will map out to http://localhost:8080/signalr by default
            ////app.MapConnection<MyServerConnection>("/messaging", config);

            //app.Map("/messaging", map =>
            //{
            //    //.UseCors(CorsOptions.AllowAll);
            //    var hubConfiguration = new HubConfiguration
            //    {
            //        // You can enable JSONP by uncommenting line below.
            //        // JSONP requests are insecure but some older browsers (and some
            //        // versions of IE) require JSONP to work cross domain
            //        // EnableJSONP = true
            //    };
            //    // Run the SignalR pipeline. We're not using MapSignalR
            //    // since this branch already runs under the "/signalr"
            //    // path.
            //    map.RunSignalR(hubConfiguration);
            //});
            app.MapSignalR<MyServerConnection>("/messaging", config);


        }

    }
}