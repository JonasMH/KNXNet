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
			KnxGroupAddress temperatureAddress = KnxGroupAddress.Parse("15/1/0");
			KnxGroupAddress relayAddress = KnxGroupAddress.Parse("15/3/0");
			DataTypeParser parser = new DataTypeParser();

			connection.Logger = new ConsoleLogger();

			connection.OnConnect += (sender, eventArgs) => Console.WriteLine("Connected");
			connection.OnDisconnect += (sender, eventArgs) => Console.WriteLine("Disconnected");
			connection.OnData += (sender, eventArgs) =>
			{
				Console.WriteLine(eventArgs.SourceAddress + " -> " + eventArgs.DestinationAddress + " : " + ByteArrayToString(eventArgs.Data));

				if(eventArgs.DestinationAddress == temperatureAddress && (eventArgs.RequestType == APCIType.AGroupValueWrite || eventArgs.RequestType == APCIType.AGroupValueResponse))
				{
					Console.WriteLine("Temp: " + parser.DTP9(eventArgs.Data).FloatValue);
				}
			};

			connection.Connect();

			while (true)
			{
				ConsoleKey key = Console.ReadKey().Key;

				if (key == ConsoleKey.Q)
				{
					break;
				}
				else if (key == ConsoleKey.A)
				{
					connection.RequestValue(temperatureAddress);
				}
				else if (key == ConsoleKey.Z)
				{
					connection.SendValue(relayAddress, new byte[] {0}, 1);
				}
				else if (key == ConsoleKey.X)
				{
					connection.SendValue(relayAddress, new byte[] {1}, 1);
				}

			}

			connection.Disconnect();

			Task.Delay(1000).Wait();
		}
	}

}
