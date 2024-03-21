using KuznechikCryptoProtocol.alghoritm;

namespace KuznechikCryptoProtocol
{
    internal static class ConsoleApplication
    {
        private static KuznyechikCryptor kuznyechikCryptor = new KuznyechikCryptor(new Operations());

        private const string bigFileFilePath = "./Data/BigFile.mp4";
        private const string dataFilePath = "./Data/data.txt";
        private const string keyFilePath = "./Data/key.txt";

        private const string encryptedBigFileName = "EncryptedBigFile";
        private const string decryptedBigFileName = "DecryptedBigFile.mp4";
        private const string resultPdfFileName = "resultPDF.pdf";
        private const string GOST_KEY = "8899aabbccddeeff0011223344556677fedcba98765432100123456789abcdef";


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
            byte[] key = Convert.FromHexString(GOST_KEY);

            double fileSize = new FileInfo(bigFileFilePath).Length;
            double fileSizeMb = Math.Round(fileSize / 1024 / 1024, 2);
            Console.WriteLine($"Размер файла: {fileSizeMb} Мб");
            Task2SimpleMode(key, fileSizeMb);

            Console.WriteLine("Нажмите дважды Enter для продолжения...");
            Console.ReadLine();
            Console.ReadLine();
        }

        private static void Task2SimpleMode(byte[] key, double fileSizeMb)
        {
            Console.WriteLine("Начало обычного шифрования без модификаций. " +
                            "Длительный процесс, придется подождать...");
            DateTime start = DateTime.Now;
            var encriptedData = kuznyechikCryptor.Encript(File.ReadAllBytes(bigFileFilePath), key);
            DateTime end = DateTime.Now;
            Console.WriteLine($"Время начала шифрования: {start}. \n" +
                $"Время конца шифрования: {end}. \n" +
                $"Общее время: {(end - start).TotalSeconds} сек. \n" +
                $"Скорость: {fileSizeMb / (end - start).TotalSeconds} Мб/сек\n");
            var simpleEncryptedBigFileName = "simple_" + encryptedBigFileName;
            using (FileStream fileStream = new FileStream(simpleEncryptedBigFileName, FileMode.OpenOrCreate))
            {
                fileStream.Write(encriptedData);
            }
            Console.WriteLine("Зашифрованный файл: " + simpleEncryptedBigFileName);

            var simpleDecriptedBigFileName = "simple_" + decryptedBigFileName;
            var decriptedData = kuznyechikCryptor.Decript(encriptedData, key);
            using (FileStream fileStream = new FileStream(simpleDecriptedBigFileName, FileMode.OpenOrCreate))
            {
                fileStream.Write(decriptedData);
            }
            Console.WriteLine("Расшифрованный файл: " + simpleDecriptedBigFileName);
            Console.WriteLine("---------------------");
        }

        public static void Run()
        {
            string selectedItem = "";
            while (selectedItem != "3")
            {
                PrintMenu();
                selectedItem = Console.ReadLine();
                //selectedItem = "2";
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
            Console.WriteLine("2. Зашифровать файл Data/BigFile.mp4 обычным методом и быстрым по ключу из ГОСТ, вывести статистику");
            Console.WriteLine("3. Выход");
            Console.WriteLine("---------------------------------");
            Console.WriteLine("Все файлы можно подменять на горячую(во время выбора пункта меню).");
            Console.WriteLine("При выборе пункта файл автоматически подгрузится, главное чтобы совпадали имена");
        }

    }
}
