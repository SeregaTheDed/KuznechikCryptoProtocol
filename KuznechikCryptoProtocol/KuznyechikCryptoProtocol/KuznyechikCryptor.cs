namespace KuznyechikCryptoProtocol.KuznyechikCryptoProtocol
{
    internal class KuznyechikCryptor
    {
        private byte[][] iterationConstants = new byte[32][]; // массив итерационных констант
        private byte[][] iterationKeys = new byte[10][]; // массив итерационных ключей
        private readonly Operations operations;

        public KuznyechikCryptor(Operations operations)
        {
            this.operations = operations;
        }

        public byte[] Encript(byte[] originText, byte[] masterKey)
        {
            KeyGen(masterKey);
            byte[] encriptedBytes = new byte[originText.Length];
            AddBytesToGoodSize(originText, out int NumOfBlocks, ref originText, ref encriptedBytes);
            for (int i = 0; i < NumOfBlocks; i++)
            {
                byte[] block = new byte[16];
                for (int j = 0; j < 16; j++)
                {
                    block[j] = originText[i * 16 + j];
                }

                for (int j = 0; j < 9; j++)
                {
                    block = operations.X(block, iterationKeys[j]);
                    block = operations.S(block);
                    block = operations.L(block);
                }

                block = operations.X(block, iterationKeys[9]);
                for (int j = 0; j < 16; j++)
                {
                    encriptedBytes[i * 16 + j] = block[j];
                }
            }
            return encriptedBytes;
        }

        private static void AddBytesToGoodSize(byte[] file, out int NumOfBlocks, ref byte[] originBytes, ref byte[] encriptedBytes)
        {
            int NumberOfNull;
            if ((file.Length % 16) != 0)
            {
                NumOfBlocks = (file.Length / 16) + 1;
                NumberOfNull = NumOfBlocks * 16 - file.Length;
                Array.Resize(ref originBytes, originBytes.Length + NumberOfNull);
                Array.Resize(ref encriptedBytes, originBytes.Length + NumberOfNull);
                if (NumberOfNull == 1)
                {
                    originBytes[originBytes.Length - 1] = 0x80;
                }
                else
                {
                    for (int i = originBytes.Length - 1; i >= 0; i--)
                    {
                        if (i == originBytes.Length - 1)
                        {
                            originBytes[originBytes.Length - 1] = 0x81;
                        }
                        else if (originBytes[i] != 0)
                        {
                            originBytes[i + 1] = 0x01;
                            break;
                        }
                    }
                }
            }
            else
            {
                NumOfBlocks = file.Length / 16;
            }
        }

        public byte[] Decript(byte[] originBytes, byte[] masterKey)
        {
            KeyGen(masterKey);
            int blockCount = originBytes.Length / 16;
            byte[] decriptedBytes = new byte[originBytes.Length];
            for (int i = 0; i < blockCount; i++)
            {
                byte[] block = new byte[16];
                for (int j = 0; j < 16; j++)
                {
                    block[j] = originBytes[i * 16 + j];
                }

                block = operations.X(block, iterationKeys[9]);

                for (int j = 8; j >= 0; j--)
                {
                    block = operations.LReverse(block);
                    block = operations.SReverse(block);
                    block = operations.X(block, iterationKeys[j]);

                }

                for (int j = 0; j < 16; j++)
                {
                    decriptedBytes[i * 16 + j] = block[j];
                }
            }
            return decriptedBytes;
        }
        private void KeyGen(byte[] mas_key)
        {
            //Генерация раундовых констант
            byte[][] iterationNumber = new byte[32][];
            for (int i = 0; i < 32; i++)
            {
                iterationNumber[i] = [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, Convert.ToByte(i + 1)];
                iterationConstants[i] = operations.L(iterationNumber[i]);
            }

            //Генерация первых 2-х ключей
            byte[] A = new byte[16];
            for (int i = 0; i < 16; i++) 
            { 
                A[i] = mas_key[i]; 
            }
            byte[] B = new byte[16];
            int j = 0;
            for (int i = 16; i < 32; i++)
            {
                B[j] = mas_key[i];
                j++;
            }
            j = 0;
            iterationKeys[0] = A;
            iterationKeys[1] = B;

            byte[] C = new byte[16];
            byte[] D = new byte[16];

            for (int i = 0; i < 4; i++)
            {
                for (int k = 0; k < 4; k++)
                {
                    operations.F(A, B, ref C, ref D, iterationConstants[k*2 + 8 * i]);
                    operations.F(C, D, ref A, ref B, iterationConstants[k*2 + 1 + 8 * i]);
                }
                iterationKeys[2 * i + 2] = A;
                iterationKeys[2 * i + 3] = B;
            }
        }

        public override string ToString()
        {
            return "Кузнечик" + operations.ToString();
        }
    }
}
