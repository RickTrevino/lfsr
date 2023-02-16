using System.Text.RegularExpressions;
using System.Collections;
using System.Text;

namespace lfsr
{
    public static class Program
    {
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
                Crypt(testCase.bytes, testCase.initialValue);
                Console.WriteLine();
            }
        }

        static byte[] Crypt(byte[] data, uint initialValue)
        {
            const uint feedbackValue = 0x87654321; //Given const from instructions.
            bool inputIsHex = IsHex(data);

            data = FormatAndPrintData(data, inputIsHex); //Interpret hex input strings and convert to a byte array. Print input.
            byte[] key = CreateKey(feedbackValue, initialValue, data.Length); //Create the key using lfsr per project specs.
            byte[] resultBytes = Xor(data, key);
            PrintResult(resultBytes, inputIsHex);
            return resultBytes; //Returns the actual byte array, which is not the same as the visual output (PrintResult).
        }

        static bool IsHex(byte[] data)
        {
            var dataString = Encoding.ASCII.GetString(data);
            return Regex.IsMatch(dataString, "^[x\\\\0-9A-Fa-f]+$"); //Regex pattern built w/ regex101.com. This checks if there are any characters besides those used in \x hex format.
        }

        static byte[] CreateKey(uint feedbackValue, uint workingValue, int keyLength)
        {
            var key = new byte[keyLength]; //Create a key the same length as the input data.

            for (int i = 0; i < key.Length; i++)
            {
                int stepCount = 8;

                while (stepCount != 0)
                {
                    stepCount--;
                    var shouldXor = (workingValue & 0x01) > 0; //Check if the last bit is 0 or 1 per the instructions.

                    workingValue >>= 1; //Shift the bits right.
                    if (shouldXor) { workingValue ^= feedbackValue; } //If the last bit is 1, XOR the bits with the feedbackValue per the instructions.
                }

                key[i] = (byte)workingValue; //Make the lowest byte the next key byte every 8 steps per the instructions.
            }

            return key;
        }

        // I used the following documentation as a guide for converting the data/key to BitArrays, then Xoring, because this seemed simplier than looping through the byte arrays.
        // https://learn.microsoft.com/en-us/dotnet/api/system.collections.bitarray.xor?view=net-7.0
        static byte[] Xor(byte[] data, byte[] key)
        {
            var dataBits = new BitArray(data);
            var keyBits = new BitArray(key);
            var resultBytes = new byte[data.Length];

            if (dataBits.Length != keyBits.Length) { throw new Exception("The input data and key are not the same size."); }

            var resultBits = dataBits.Xor(keyBits);
            resultBits.CopyTo(resultBytes, 0);
            return resultBytes;
        }

        static byte[] FormatAndPrintData(byte[] data, bool inputIsHex)
        {
            Console.Write("Input: " + Encoding.ASCII.GetString(data));
            if (inputIsHex) //The byte array currently includes many bytes witht the values 92 and 120 repeated due to \x format.
            {
                var dataString = Encoding.ASCII.GetString(data).Replace("\\x", ""); //Remove all of the \x from the string.
                return Convert.FromHexString(dataString); //Reformat the byte array so that there is not a byte for every \x.
            }
            return data;
        }

        // I used the following website for help reformating to \x hex format prior to printing as I had never done this before and wasn't aware of these libraries:
        // https://codelikeadev.com/blog/convert-string-to-hex-c-sharp
        static void PrintResult(byte[] data, bool inputIsHex)
        {
            Console.Write("\nOutput: ");
            if (inputIsHex) { Console.WriteLine(Encoding.ASCII.GetString(data)); } //If the input was hex, then the output bytes to be printed are ASCII chars.
            else { Console.WriteLine("\\x" + BitConverter.ToString(data).Replace("-", "\\x")); } //If the input was not hex, then we will reformat the string to \x hex format before printing.
        }
    }
}