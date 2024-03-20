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
        private static KuznyechikCryptor kuznyechikCryptor = new KuznyechikCryptor();

        private const string bigFileFilePath = "./Data/BigFile.mp4";
        private const string dataFilePath = "./Data/data.txt";
        private const string keyFilePath = "./Data/key.txt";

        private const string encryptedBigFileName = "EncryptedBigFile";
        private const string decryptedBigFileName = "DecryptedBigFile.mp4";
        private const string resultPdfFileName = "resultPDF.pdf";
        

        private static void Task1()
        {
            // Вариант 9
            string keyString = File.ReadAllText(keyFilePath);
            byte[] key = Convert.FromHexString(keyString);

            string dataString = File.ReadAllText(dataFilePath);
            byte[] data = Convert.FromBase64String(dataString);

            byte[] decriptedData = kuznyechikCryptor.Decript(data, key).ToArray();

            using (FileStream fileStream = new FileStream(resultPdfFileName, FileMode.OpenOrCreate))
            {
                fileStream.Write(decriptedData);
            }
            Console.WriteLine($"Расшифрован файл: {resultPdfFileName}. Нажмите Enter для продолжения...");
            Console.ReadLine();
        }

        private static void Task2()
        {
            string keyString = File.ReadAllText(keyFilePath);
            byte[] key = Convert.FromHexString(keyString);

            double fileSize = new FileInfo(bigFileFilePath).Length;
            double fileSizeMb = Math.Round(fileSize / 1024 / 1024, 2);
            Console.WriteLine($"Размер файла: { fileSizeMb } Мб");

            Console.WriteLine("Начало обычного шифрования без модификаций. " +
                "Длительный процесс, придется подождать...");
            DateTime start1 = DateTime.Now;
            var encriptedData = kuznyechikCryptor.Encript(File.ReadAllBytes(bigFileFilePath), key);
            DateTime end1 = DateTime.Now;
            Console.WriteLine($"Время начала шифрования: {start1}. Время начала шифрования: {end1}. " +
                $"Общее время: {(end1-start1).TotalSeconds} сек. Скорость: {fileSizeMb / (end1 - start1).TotalSeconds} Мб/сек");
            using (FileStream fileStream = new FileStream(encryptedBigFileName, FileMode.OpenOrCreate))
            {
                fileStream.Write(encriptedData);
            }
            Console.WriteLine("Зашифрованный файл: " + encryptedBigFileName);

            var decriptedData = kuznyechikCryptor.Decript(encriptedData, key);
            using (FileStream fileStream = new FileStream(decryptedBigFileName, FileMode.OpenOrCreate))
            {
                fileStream.Write(decriptedData);
            }
            Console.WriteLine("Расшифрованный файл: " + decryptedBigFileName);


            Console.WriteLine("Нажмите дважды Enter для продолжения...");
            Console.ReadLine();
            Console.ReadLine();
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
                        default: 
                            { 
                                Console.WriteLine("Выберите пункт меню!");
                                Console.WriteLine("Нажмите Enter для продолжения");
                                Console.ReadLine();
                                break; 
                            }
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
            Console.WriteLine("2. Зашифровать файл Data/BigFile.mp4 обычным методом и быстрым по ключу (файл Data/key.txt), вывести статистику");
            Console.WriteLine("3. Выход");
            Console.WriteLine("---------------------------------");
            Console.WriteLine("Все файлы можно подменять на горячую(во время выбора пункта меню).");
            Console.WriteLine("При выборе пункта файл автоматически подгрузится, главное чтобы совпадали имена");
        }

    }
}
