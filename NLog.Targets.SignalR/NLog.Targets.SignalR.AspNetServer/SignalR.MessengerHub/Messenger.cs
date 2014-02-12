using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Web.UI;
using Microsoft.AspNet.SignalR;


namespace NLog.Targets.SignalR.AspNetServer.SignalR.MessengerHub
{
    public class Messenger
    {
        private readonly static Lazy<Messenger> _instance = new Lazy<Messenger>(() => new Messenger());
        private readonly ConcurrentDictionary<string, Message> _messages = new ConcurrentDictionary<string, Message>();
        
        private Messenger()
        {
        }

        /// <summary>
        /// Gets the instance.
        /// </summary>
        public static Messenger Instance
        {
            get
            {
                return _instance.Value;
            }
        }


        /// <summary>
        /// Gets all messages.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Message> GetAllMessages()
        {
            return _messages.Values;
        }

        /// <summary>
        /// Broads the cast message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void BroadCastMessage(dynamic message, string group)
        {
            //GetClients(group).add(message);
            GetClients(group).broadcastMessage(group, message);
        }

        /// <summary>
        /// Gets the clients.
        /// </summary>
        /// <returns></returns>
        private static dynamic GetClients(string group)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<MessengerHub>();
            //return context.Clients[group];
            return context.Clients.Group(group);
           
        }   


    }
}