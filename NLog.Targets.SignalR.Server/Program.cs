using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using SignalR.Hosting.Self;
using SignalR.Hubs;

namespace NLog.Targets.SignalR.SelfHostServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Debug.Listeners.Add(new ConsoleTraceListener());
            Debug.AutoFlush = true;

            const string url = "http://localhost:8005/";
            var server = new Server(url);
            server.Configuration.DisconnectTimeout = TimeSpan.Zero;

            server.MapConnection<MyServerConnection>("/messaging");

            server.Start();

            Console.WriteLine("Server running on {0}", url);

            // Keep going until somebody hits 'x'
            while (true)
            {
                ConsoleKeyInfo ki = Console.ReadKey(true);
                if (ki.Key == ConsoleKey.X)
                {
                    break;
                }
            }
        }
    }

   
}
