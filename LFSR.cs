using System.Text.RegularExpressions;
using System.Collections;
using System.Text;

namespace lfsr
{
    public class LFSR
    {
        public byte[] Crypt(byte[] data, uint initialValue)
        {
            const uint feedbackValue = 0x87654321; //Given const from instructions.
            bool inputIsHex = IsHex(data);

            data = FormatAndPrintData(data, inputIsHex); //Interpret hex input strings and convert to a byte array. Print input.
            byte[] key = CreateKey(feedbackValue, initialValue, data.Length); //Create the key using lfsr per project specs.
            byte[] resultBytes = Xor(data, key);
            PrintResult(resultBytes, inputIsHex);
            return resultBytes; //Returns the actual byte array, which is not the same as the visual output (PrintResult).
        }

        private static bool IsHex(byte[] data)
        {
            var dataString = Encoding.ASCII.GetString(data);
            return Regex.IsMatch(dataString, "^[x\\\\0-9A-Fa-f]+$"); //Regex pattern built w/ regex101.com. This checks if there are any characters besides those used in \x hex format.
        }

        private static byte[] CreateKey(uint feedbackValue, uint workingValue, int keyLength)
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
        private static byte[] Xor(byte[] data, byte[] key)
        {
            var dataBits = new BitArray(data);
            var keyBits = new BitArray(key);
            var resultBytes = new byte[data.Length];

            if (dataBits.Length != keyBits.Length) { throw new Exception("The input data and key are not the same size."); }

            var resultBits = dataBits.Xor(keyBits);
            resultBits.CopyTo(resultBytes, 0);
            return resultBytes;
        }

        private static byte[] FormatAndPrintData(byte[] data, bool inputIsHex)
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
        private static void PrintResult(byte[] data, bool inputIsHex)
        {
            Console.Write("\nOutput: ");
            if (inputIsHex) { Console.WriteLine(Encoding.ASCII.GetString(data)); } //If the input was hex, then the output bytes to be printed are ASCII chars.
            else { Console.WriteLine("\\x" + BitConverter.ToString(data).Replace("-", "\\x")); } //If the input was not hex, then we will reformat the string to \x hex format before printing.
        }
    }
}