﻿using System;
using System.Diagnostics;

namespace KnxNet.Core
{
	[DebuggerDisplay("Group Address = {" + nameof(ToString) + "()}")]
	public class KnxAddress
	{
		private byte[] _value = new byte[2];

		public KnxAddress()
		{
		}

		public KnxAddress(byte area, byte line, byte busdevice)
		{
			Area = area;
			Line = line;
			BusDevice = busdevice;
		}

		public static KnxAddress Parse(string input)
		{
			string[] vals = input.Split(new [] {"."}, StringSplitOptions.RemoveEmptyEntries);

			if (vals.Length != 3)
				throw new Exception("Could not parse, not enough .");

			return new KnxAddress((byte) int.Parse(vals[0]), (byte) int.Parse(vals[1]), (byte) int.Parse(vals[2]));
		}

		public byte[] Value
		{
			get => _value;
			set
			{
				if (_value == null || _value.Length != 2)
					throw new ArgumentException("Must be 2 long", nameof(value));

				_value = value;
			}
		}

		public byte Area
		{
			get => (byte) (Value[0] >> 4);
			set
			{
				Value[0] &= 0x0F;
				Value[0] |= (byte) (value << 4);
			}
		}

		public byte Line
		{
			get => (byte) (Value[0] & 0x0F);
			set
			{
				Value[0] &= 0xF0;
				Value[0] |= (byte) (value & 0xF0);
			}
		}

		public byte BusDevice
		{
			get => Value[1];
			set => Value[1] = value;
		}

		public override string ToString()
		{
			return $"{Area}.{Line}.{BusDevice}";
		}
	}
}