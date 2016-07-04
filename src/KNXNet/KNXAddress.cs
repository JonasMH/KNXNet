using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace KNXNet
{
    [DebuggerDisplay("Group Address = {ToString()}")]
    public class KNXAddress
    {
        private byte[] _value = new byte[2];

        public KNXAddress() { }

        public KNXAddress(byte area, byte line, byte busdevice)
        {
            Area = area;
            Line = line;
            BusDevice = busdevice;
        }

        public static KNXAddress Parse(string input)
        {
            string[] vals = input.Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries);

            if (vals.Length != 3)
                throw new Exception("Could not parse, not enough .");

            return new KNXAddress((byte)int.Parse(vals[0]), (byte)int.Parse(vals[1]), (byte)int.Parse(vals[2]));
        }

        public byte[] Value
        {
            get { return _value; }
            set
            {
                if(_value == null || _value.Length != 2)
                    throw new ArgumentException("Must be 2 long", nameof(value));

                _value = value;
            }
        }

        public byte Area
        {
            get { return (byte)(Value[0] >> 4); }
            set
            {
                Value[0] &= 0x0F;
                Value[0] |= (byte)(value << 4);
            }
        }

        public byte Line
        {
            get { return (byte)(Value[0] & 0x0F); }
            set
            {
                Value[0] &= 0xF0;
                Value[0] |= (byte)(value & 0xF0);
            }
        }

        public byte BusDevice { get { return Value[1]; } set { Value[1] = value; } }

        public override string ToString()
        {
            return $"{Area}.{Line}.{BusDevice}";
        }
    }
}
