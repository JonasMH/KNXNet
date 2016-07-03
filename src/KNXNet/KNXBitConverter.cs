using System;

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
    }
}
