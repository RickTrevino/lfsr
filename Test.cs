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
        public static void Main()
        {
            List<Test> testCases = new()
            {
                new("apple", 0x12345678),
                new(@"\xCD\x01\xEF\xD7\x30", 0x12345678),
                new(@"- ()\ A longer test invoking various chars 298%#1 `", 0x12345678),
                new(@"\x81\x51\xB7\x92\x09\x13\x63\xF9\xA6\xA8\xF1\x9B\xD5\x0E\xD1\x12\x84\xB0\x11\x6D\x37\x6C\x8E\xA6\x21\x2F\x6A\xF6\xBE\x65\xFA\xC4\x0F\x4D\x2A\x42\xEF\xA1\x52\xDB\xB0\x7F\x71\xEB\xD1\xAE\x2B\x74\xA0\x9F\xF3", 0x12345678)
            };

            foreach (var testCase in testCases)
            {
                LFSR analyzer = new();
                analyzer.Crypt(testCase.bytes, testCase.initialValue);
                Console.WriteLine();
            }
        }
    }
}