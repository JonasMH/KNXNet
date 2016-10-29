using System.Collections.Generic;
using System.Linq;

namespace KnxNet.Core
{
	public class KnxGroupAddressDescriptionMap : List<KnxGroupAddressDescription>
	{
		public new void Add(KnxGroupAddressDescription description)
		{
			base.Add(description);
		}

		public KnxGroupAddressDescription GetByAddress(KnxGroupAddress address)
		{
			return this.FirstOrDefault(x => x.Address == address);
		}
	}
}
