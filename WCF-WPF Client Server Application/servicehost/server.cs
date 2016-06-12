using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using tickerservice;
using client;
using contract;
using System.Threading;
using System.Diagnostics;

namespace servicehost
{
    class server
    {
        static void Main(string[] args)
        {
            TickerImpl tickerInstance = new TickerImpl();
            ServiceHost host = new ServiceHost(tickerInstance);

            host.Open();
            Console.WriteLine("Service is running");
            Console.WriteLine("Press F5 to clone, ESC to stop");
            while (true)
            {
                var curKey = Console.ReadKey().Key;
                if (curKey == ConsoleKey.Escape)
                {
                    break;
                }
                else if (curKey == ConsoleKey.F5)
                {
                    tickerInstance.Clone();
                }
                Thread.Sleep(50);
            }
            host.Close();
        }
    }
}
