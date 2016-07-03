using System;
using System.Linq;

namespace KNXNet
{
    public static class KNXBitConverter
    {
        public static byte[] GetBytes(short value)
        {
            byte[] buffer = BitConverter.GetBytes(value);

            if(BitConverter.IsLittleEndian)
                Array.Reverse(buffer);

            return buffer;
        }

        public static short ToShort(byte[] buffer, int index)
        {
            return BitConverter.ToInt16(new byte[] {buffer[index + 1], buffer[index]}, 0);
        }
    }
}
