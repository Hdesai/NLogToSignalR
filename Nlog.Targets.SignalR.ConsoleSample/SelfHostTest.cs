using System;
using System.Threading;
using Microsoft.AspNet.SignalR.Client;
using Owin;
using Microsoft.Owin.Hosting;

namespace Nlog.Targets.SignalR.ConsoleSample
{
    public class SelfHostTest
    {
        public static void Start()
        {
            const string url = "http://localhost:8081/";

            //var server=WebApplication.Start<MyConnection>(url + "/echo");
            
            

            Console.WriteLine("Server running on {0}", url);

            var connection = new Connection("http://localhost:8081/echo");
            connection.Received += Console.WriteLine;
            connection.StateChanged += change => Console.WriteLine(change.OldState + " => " + change.NewState);

            ThreadPool.QueueUserWorkItem(_ => connection.Start().ContinueWith(task =>
                {
                    if (task.IsFaulted)
                    {
                        Console.WriteLine("Faulted:{0}",
                                          task.Exception != null
                                              ? task.Exception.GetBaseException().ToString()
                                              : "Task Faulted");
                    }
                    else
                    {
                        Console.WriteLine("Successfully Connected");

                        try
                        {
                            while (true)
                            {
                                connection.Send(String.Format("Calling Server {0} @ {1}",connection.Url,DateTime.Now));
                                //Just a sample to test the concept
                                Thread.Sleep(2000);
                            }
                        }
                        catch (Exception exception)
                        {
                            Console.WriteLine(exception);
                        }
                    }
                }));

            Console.ReadLine();
            //server.Dispose();
        }
    }
}