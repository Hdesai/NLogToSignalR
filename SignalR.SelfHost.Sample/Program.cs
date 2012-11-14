using System;
using System.Threading;
using System.Threading.Tasks;
using SignalR.Hosting.Self;

namespace SignalR.SelfHost.Sample
{
    internal class Program
    {
        private static void Main()
        {
            const string url = "http://localhost:8081/";

            var server = new Server(url);
            server.MapConnection<MyConnection>("/echo");
            server.Start();
            Console.WriteLine("Server running on {0}", url);

            var connection = new Client.Connection("http://localhost:8081/echo");
            connection.Received += Console.WriteLine;
            connection.StateChanged += change => Console.WriteLine(change.OldState + " => " + change.NewState);
            connection.Start().ContinueWith(task =>
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
                                //Just a sample to test the concept
                                Console.WriteLine("\nCalling Server {0} @ {1}", connection.Url, DateTime.Now);
                                connection.Send("Hello Server");
                                Thread.Sleep(2000);
                            }
                        }
                        catch (Exception exception)
                        {
                            Console.WriteLine(exception);
                        }
                    }
                });


            Console.ReadLine();
        }
    }

    internal class MyConnection : PersistentConnection
    {
        protected override Task OnReceivedAsync(IRequest request, string connectionId, string data)
        {
            Console.WriteLine("Call Received by Server with data: {0}",data);
            Console.WriteLine("Server calling all clients back with:");
            return Connection.Broadcast("Hello Client");
        }
    }
}