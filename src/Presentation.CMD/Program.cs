using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using KNXNet;
using KNXNet.Placeholders;

namespace Presentation.CMD
{
    public class Program
    {
        public static void Main(string[] args)
        {
            KnxConnection connection = new KnxConnection()
            {
                Host = "192.168.1.21",
                Port = 3671
            };

            connection.OnNewDataIn += (sender, eventArgs) =>
            {
                Console.WriteLine($"{eventArgs.SourceAddress} -> {eventArgs.DestinationAddress}: " + eventArgs.Data.Select(x => x.ToString("X")).Aggregate((s, s1) => s + s1));
            };

            connection.StartAsync();

            while (true)
            {
                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.A:
                        connection.SendMessage(new KNXGroupAddress(0, 0, 41), new byte[]{0x00}, 1 );
                        break;
                    case ConsoleKey.S:
                        connection.SendMessage(new KNXGroupAddress(0, 0, 41), new byte[] { 0x01 }, 1);
                        break;
                    case ConsoleKey.Q:
                        connection.Disconnect();
                        return;
                }
            }
        }
    }
}
