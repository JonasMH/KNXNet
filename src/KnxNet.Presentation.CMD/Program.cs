using System;
using KnxNet.Core;
using KnxNet.Tunneling;
using System.Linq;

namespace Presentation.CMD
{
    public class Program
    {
        public static void Main(string[] args)
        {
            KnxTunnelingConnection connection = new KnxTunnelingConnection("192.168.1.21", 3671)
            { 
                Logger = new ConsoleLogger()
            };

            connection.OnData += (sender, eventArgs) =>
            {
                Console.WriteLine($"{eventArgs.SourceAddress} -> {eventArgs.DestinationAddress}: " + eventArgs.Data.Select(x => x.ToString("X")).Aggregate((s, s1) => s + s1));
            };

            connection.OnConnect += (sender, eventArgs) =>
            {
                Console.WriteLine("Connected");
            };

            connection.OnDisconnect += (sender, eventArgs) =>
            {
                Console.WriteLine("Disconnected");
            };

            connection.Connect();

            while (true)
            {
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.A:
                        connection.SendValue(KnxGroupAddress.Parse("0/0/41"), new byte[]{ 0x00 }, 1 );
                        break;
                    case ConsoleKey.S:
                        connection.SendValue(KnxGroupAddress.Parse("0/0/41"), new byte[] { 0x01 }, 1);
                        break;
                    case ConsoleKey.Q:
                        connection.Disconnect();
                        return;
                }
            }
        }
    }
}
