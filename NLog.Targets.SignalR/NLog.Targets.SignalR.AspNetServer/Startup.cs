using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(NLog.Targets.SignalR.AspNetServer.Startup))]

namespace NLog.Targets.SignalR.AspNetServer
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //Any connection or hub wire up and configuration should go here
            //app.MapSignalR();
            app.MapSignalR(new HubConfiguration
            {
                EnableDetailedErrors = true
            });
        }

    }
}