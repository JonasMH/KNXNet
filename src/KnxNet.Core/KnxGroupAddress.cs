using System;

namespace KnxNet.Core
{
	public class KnxGroupAddress
	{
		private byte[] _value = new byte[2];

		public KnxGroupAddress() { }

		public KnxGroupAddress(byte mainGroup, byte middleGroup, byte subGroup)
		{
			MainGroup = mainGroup;
			MiddleGroup = middleGroup;
			SubGroup = subGroup;
		}

		public static KnxGroupAddress Parse(string input)
		{
			string[] vals = input.Split(new[] { "/" }, StringSplitOptions.RemoveEmptyEntries);

			if (vals.Length != 3)
				throw new Exception("Could not parse, not enough /");

			return new KnxGroupAddress((byte)int.Parse(vals[0]), (byte)int.Parse(vals[1]), (byte)int.Parse(vals[2]));
		}

		public byte[] Value
		{
			get { return _value; }
			set
			{
				if (_value == null || _value.Length != 2)
					throw new ArgumentException("Lenght must be 2 long", nameof(value));

				_value = value;
			}
		}

		public byte MainGroup
		{
			get { return (byte)(Value[0] >> 3); }
			set
			{
				Value[0] &= 0x07;
				Value[0] |= (byte)(value << 3);
			}
		}

		public byte MiddleGroup
		{
			get { return (byte)(Value[0] & 0x07); }
			set
			{
				Value[0] &= 0xF8;
				Value[0] |= (byte)(value & 0x07);
			}
		}

		public byte SubGroup { get { return Value[1]; } set { Value[1] = value; } }

		public static bool operator==(KnxGroupAddress address1, KnxGroupAddress address2)
		{
			if ((object)address1 == null || (object)address2 == null)
				return false;

			return
				address1.MainGroup == address2.MainGroup &&
				address1.MiddleGroup == address2.MiddleGroup &&
				address1.SubGroup == address2.SubGroup;
		}

		public static bool operator !=(KnxGroupAddress address1, KnxGroupAddress address2)
		{
			return !(address1 == address2);
		}

		public override string ToString()
		{
			return $"{MainGroup}/{MiddleGroup}/{SubGroup}";
		}
	}
}
