using KuznechikCryptoProtocol.alghoritm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuznechikCryptoProtocol
{
    internal static class ConsoleApplication
    {
        private static KuznyechikCryptor kuznyechikDecryptor = new KuznyechikCryptor();

        private static void Task1()
        {
            // Вариант 9
            string keyString = File.ReadAllText("./Data/key.txt");
            byte[] key = Convert.FromHexString(keyString);

            string dataString = File.ReadAllText("./Data/data.txt");
            byte[] data = Convert.FromBase64String(dataString);

            byte[] decriptedData = kuznyechikDecryptor.Decript(data, key).ToArray();

            using (FileStream fileStream = new FileStream("resultPDF.pdf", FileMode.OpenOrCreate))
            {
                fileStream.Write(decriptedData);
            }
            Console.WriteLine("Расшифрован файл: resultPDF.pdf. Нажмите Enter для продолжения");
            Console.ReadLine();
        }

        private static void Task2()
        {
            double fileSize = new FileInfo("./Data/BigFile.mp4").Length;
            Console.WriteLine(fileSize / 1024 / 1024);

        }

        public static void Run()
        {
            string selectedItem = "";
            while (selectedItem != "3")
            {
                PrintMenu();
                selectedItem = Console.ReadLine();
                try 
                {
                    switch (selectedItem)
                    {
                        case "1": Task1(); break;
                        case "2": Task2(); break;
                        case "3": return;
                        default: Console.WriteLine("Выберите пункт меню!"); break;
                    }
                } 
                catch (IOException exception)
                {
                    Console.WriteLine("Проверьте путь до файла или права на него: " + exception.Message);
                    Console.WriteLine("Нажмите Enter для продолжения");
                    Console.ReadLine();
                }
                
            }

        }

        private static void PrintMenu()
        {
            Console.Clear();
            Console.WriteLine("Выберите пункт меню:");
            Console.WriteLine("1. Расшифровать данные (файл Data/data.txt) по ключу (файл Data/key.txt)");
            Console.WriteLine("2. Зашифровать файл Data/BigFile.mp4 обычным методом и быстрым, вывести статистику");
            Console.WriteLine("3. Выход");
            Console.WriteLine("---------------------------------");
            Console.WriteLine("Все файлы можно подменять на горячую(во время выбора пункта меню).");
            Console.WriteLine("При выборе пункта файл автоматически подгрузится, главное чтобы совпадали имена");
        }

    }
}
