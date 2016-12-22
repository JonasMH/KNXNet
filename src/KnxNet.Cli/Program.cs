using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KnxNet.Core;
using KnxNet.Tunneling;

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
			string mapping = File.ReadAllText("C:\\Users\\Jonas Hansen\\Desktop\\Ramskovvej.esf");
			KnxGroupAddressDescriptionMap map = new EsfImporter().LoadFromString(mapping);
			KnxTunnelingConnection connection = new KnxTunnelingConnection("192.168.1.21", 3671);

			foreach (KnxGroupAddressDescription knxGroupAddressDescription in map.Where(x => x.Name.Contains("08")))
			{
				Console.WriteLine(knxGroupAddressDescription);
			}

			connection.OnConnect += (sender, eventArgs) => Console.WriteLine("Connected");
			connection.OnDisconnect += (sender, eventArgs) => Console.WriteLine("Disconnected");
			connection.OnData += (sender, eventArgs) =>
			{
			};

			connection.Connect();

			connection.SendValue(KnxGroupAddress.Parse("6/0/24"), new byte[] { 0x01 }, 1);
			connection.SendValue(KnxGroupAddress.Parse("6/0/24"), new byte[] { 0x01 }, 1);

			Task.Delay(10000).Wait();

			connection.SendValue(KnxGroupAddress.Parse("6/0/24"), new byte[] { 0x00 }, 1);
			connection.SendValue(KnxGroupAddress.Parse("6/0/24"), new byte[] { 0x00 }, 1);

			Console.WriteLine("Sent");

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
