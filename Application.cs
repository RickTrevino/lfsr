using System.Runtime.InteropServices;

namespace lfsr
{
    public static class Application
    {
        public static void Main()
        {
            Console.WriteLine("Type the file path for the file you wish to analyze and press Enter.");
            string filePath = Console.ReadLine() ?? string.Empty;
            byte[] fileBytes = ReadFile(filePath);

            LFSR analyzer = new();
            analyzer.Crypt(fileBytes, 0x1234567);
        }

        private static byte[] ReadFile(string filePath)
        {
            throw new NotImplementedException();
        }
    }
}