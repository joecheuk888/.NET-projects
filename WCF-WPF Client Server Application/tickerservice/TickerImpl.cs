using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using contract;
using client;
using System.Collections.Concurrent;
using System.Timers;

namespace tickerservice
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    [Export(typeof(ITicker))]
    public class TickerImpl : ITicker
    {
        IDictionary<int, IAlert> clients = new ConcurrentDictionary<int, IAlert>();

        public TickerImpl()
        {
            Timer timer = new Timer(1000);
            timer.Elapsed += new ElapsedEventHandler(NotifyClientsTime);
            timer.Start();
        }

        private void NotifyClientsTime(object source, ElapsedEventArgs e)
        {
            foreach (KeyValuePair<int, IAlert> pair in clients)
            {
                IAlert client = pair.Value;
                try
                {
                    client.OnAlert(DateTime.Now);
                }
                catch (Exception)
                {
                    clients.Remove(pair.Key);
                }
            }
        }

        public void Register()
        {
            var client = OperationContext.Current.GetCallbackChannel<IAlert>();
            var clientID = client.GetHashCode();
            Console.WriteLine("logging - Register function invoked " + clientID);
            if (!clients.ContainsKey(clientID))
            {
                clients.Add(clientID, client);
            }

        }

        public void Unregister()
        {
            var client = OperationContext.Current.GetCallbackChannel<IAlert>();
            var clientID = client.GetHashCode();
            Console.WriteLine("logging - Unregister function invoked " + clientID);
            try
            {
                while (clients.ContainsKey(clientID))
                {
                    clients.Remove(clientID);
                }
            }
            catch (Exception e) {
                Console.WriteLine("Exception : " + e.StackTrace.ToString());
            }
            Console.WriteLine(clients.Count);
        }

        public void Clone()
        {
            Console.WriteLine("Clone start " + clients.Count);
            int counter = 0;
            int size = clients.Count;
            foreach (KeyValuePair<int, IAlert> pair in clients)
            {
                if (counter >= size) {
                    break;
                }
                IAlert client = pair.Value;
                try
                {
                    client.OnAlert(DateTime.MinValue);
                }
                catch (Exception)
                {
                }
                counter++;
            }
            Console.WriteLine("End of cloning " + clients.Count);
        }
    }
}
