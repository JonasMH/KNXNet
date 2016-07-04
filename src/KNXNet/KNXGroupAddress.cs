using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KNXNet.Placeholders
{
    public class KNXGroupAddress
    {
        private byte[] _value = new byte[2];

        public KNXGroupAddress() { }

        public KNXGroupAddress(byte mainGroup, byte middleGroup, byte subGroup)
        {
            MainGroup = mainGroup;
            MiddleGroup = middleGroup;
            SubGroup = subGroup;
        }

        public static KNXGroupAddress Parse(string input)
        {
            string[] vals = input.Split(new string[] { "/"}, StringSplitOptions.RemoveEmptyEntries);

            if(vals.Length != 3)
                throw new Exception("Could not parse, not enough /");

            return new KNXGroupAddress((byte) int.Parse(vals[0]), (byte) int.Parse(vals[1]), (byte) int.Parse(vals[2]));
        }

        public enum AddressFormat
        {
            Level2,
            Level3,
            Free
        };

        public AddressFormat Format { get; set; } = AddressFormat.Level3;

        public byte[] Value
        {
            get { return _value; }
            set
            {
                if (_value == null || _value.Length != 2)
                    throw new ArgumentException("Must be 2 long", nameof(value));

                _value = value;
            }
        }

        public byte MainGroup
        {
            get { return (byte)(Value[0] >> 3); }
            set
            {
                Value[0] &= 0xF8;
                Value[0] |= (byte)(value << 3);
            }
        }

        public byte MiddleGroup
        {
            get { return (byte)(Value[0] & 0xF8); }
            set
            {
                Value[0] &= 0xF8;
                Value[0] |= (byte)(value & 0x07);
            }
        }

        public byte SubGroup { get { return Value[1]; } set { Value[1] = value; } }

        public override string ToString()
        {
            return $"{MainGroup}/{MiddleGroup}/{SubGroup}";
        }
    }
}
