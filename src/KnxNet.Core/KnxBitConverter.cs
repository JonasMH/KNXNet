using System;

namespace KnxNet.Core
{
	public static class KnxBitConverter
	{
		public static byte[] GetBytes(short value)
		{
			byte[] buffer = BitConverter.GetBytes(value);

			if (BitConverter.IsLittleEndian)
				Array.Reverse(buffer);

			return buffer;
		}

		public static short ToShort(byte[] buffer, int index)
		{
			return BitConverter.ToInt16(new[] {buffer[index + 1], buffer[index]}, 0);
		}
	}
}