using System.IO;
using System.Text;

namespace KnxNet.Core
{
	public class EsfImporter
	{
		public Encoding Encoding { get; set; } = Encoding.UTF8;

		public KnxGroupAddressDescriptionMap LoadFromString(string input)
		{
			StringReader reader = new StringReader(input);
			KnxGroupAddressDescriptionMap map = new KnxGroupAddressDescriptionMap();

			int lineCount = 0;
			while (true)
			{
				string line = reader.ReadLine();
				if (line == null)
					break;

				if (lineCount == 0)
				{
					lineCount++;
					continue;
				}

				KnxGroupAddressDescription description = new KnxGroupAddressDescription();
				string[] tabSeperated = line.Split('\t');

				if (tabSeperated.Length >= 1)
				{
					string[] groupDesc = tabSeperated[0].Split('.');
					description.MainGroup = groupDesc[0];
					description.MiddleGroup = groupDesc[1];
					description.Address = KnxGroupAddress.Parse(groupDesc[2]);
				}

				if (tabSeperated.Length >= 2)
				{
					description.Name = tabSeperated[1];
				}

				if (tabSeperated.Length >= 3)
				{
					description.DataType = tabSeperated[2];
				}

				map.Add(description);
				lineCount++;
			}

			return map;
		}
	}
}