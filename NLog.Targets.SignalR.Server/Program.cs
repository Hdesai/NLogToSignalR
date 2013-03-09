using System;
using System.Diagnostics;
using Microsoft.Owin.Hosting;

namespace NLog.Targets.SignalR.SelfHostServer
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Debug.Listeners.Add(new ConsoleTraceListener());
            Debug.AutoFlush = true;

            const string url = "http://localhost:8005/";
            using (WebApplication.Start<MyServerConnection>(url + "/messaging"))
            {
                Console.WriteLine("Server running on {0}", url);
                Console.ReadLine();
            }

            Console.WriteLine("Server running on {0}", url);

           
        }
    }
}