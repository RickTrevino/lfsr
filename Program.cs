using System.Runtime.CompilerServices;
using System.Text;

namespace lfsr
{
    public static class Program
    {
        public static void Main()
        {
            Test testData = new();
            byte[] bytes = Encoding.ASCII.GetBytes(testData.a);

            Console.WriteLine(Encoding.ASCII.GetString(Crypt(bytes, testData.initialValue)));
        }

        static byte[] Crypt(byte[] data, uint initialValue)
        {
            return data;
        }
    }
}