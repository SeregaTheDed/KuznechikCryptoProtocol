using KuznechikCryptoProtocol.alghoritm;

namespace KuznechikCryptoProtocol
{
    internal class Program
    {
        private KuznyechikDecryptor kuznyechikDecryptor = new KuznyechikDecryptor();
        static void Main(string[] args)
        {
            // Вариант 9
            string key = File.ReadAllText("./Data/key.txt");
            string data = File.ReadAllText("./Data/data.txt");
            //var result = kuznyechikDecryptor.Decrypt(key, data);
            //Console.WriteLine(result);
        }
    }
}
