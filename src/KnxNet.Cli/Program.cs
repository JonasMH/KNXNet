using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KnxNet.Core;
using KnxNet.Tunneling;
using KnxNet.Core.DataTypes;

namespace KnxNet.Cli
{
	public class Program
	{
		public static string ByteArrayToString(byte[] ba)
		{
			StringBuilder hex = new StringBuilder(ba.Length * 2);
			foreach (byte b in ba)
				hex.AppendFormat("{0:x2}", b);
			return hex.ToString();
		}

		public static void Main(string[] args)
		{
			KnxTunnelingConnection connection = new KnxTunnelingConnection("172.16.1.122", 3671);
			connection.Logger = new ConsoleLogger();

			connection.OnConnect += (sender, eventArgs) => Console.WriteLine("Connected");
			connection.OnDisconnect += (sender, eventArgs) => Console.WriteLine("Disconnected");
			connection.OnData += (sender, eventArgs) =>
			{
				Console.WriteLine(eventArgs.SourceAddress + " -> " + eventArgs.DestinationAddress + " : " + ByteArrayToString(eventArgs.Data));

				if(eventArgs.DestinationAddress.ToString() == "15/1/0" && eventArgs.RequestType == APCIType.AGroupValueWrite)
				{
					Console.WriteLine("Temp: " + new DataTypeParser().DTP9(eventArgs.Data).FloatValue);
				}
			};

			connection.Connect();

			while (true)
			{
				if (Console.ReadKey().Key == ConsoleKey.Q)
				{
					break;
				}
			}

			connection.Disconnect();

			Task.Delay(1000).Wait();
		}
	}

}
