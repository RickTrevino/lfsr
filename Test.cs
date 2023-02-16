using System.Text;

namespace lfsr
{
    public class Test
    {
        public uint initialValue;
        public byte[] bytes;

        public Test(string stringValue, uint initialValue)
        {
            bytes = Encoding.ASCII.GetBytes(stringValue);
            this.initialValue = initialValue;
        }
    }
}