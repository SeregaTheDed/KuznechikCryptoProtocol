using KuznechikCryptoProtocol.alghoritm;

namespace KuznechikCryptoProtocol
{
    internal class Program
    {
        private static KuznyechikCryptor kuznyechikDecryptor = new KuznyechikCryptor();
        static void Main(string[] args)
        {
            // Вариант 9
            string keyString = File.ReadAllText("./Data/key.txt");
            byte[] key = Convert.FromHexString(keyString);
            string dataString = File.ReadAllText("./Data/data.txt");
            byte[] data = Convert.FromBase64String(dataString);

            byte[] decriptedData = kuznyechikDecryptor.Decript(data, key).ToArray();
            //byte[] decriptedData = Kuz.KuzDecript(data, key).ToArray();

            using (FileStream fileStream = new FileStream("resultPDF.pdf", FileMode.OpenOrCreate))
            {
                fileStream.Write(decriptedData);
            }
            //var result = kuznyechikDecryptor.Decrypt(key, data);
            //Console.WriteLine(result);
        }
    }
}
