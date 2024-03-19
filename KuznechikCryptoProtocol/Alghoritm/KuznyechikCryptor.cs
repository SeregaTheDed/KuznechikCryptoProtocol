namespace KuznechikCryptoProtocol.alghoritm
{
    internal class KuznyechikCryptor
    {
        public byte[][] iterC = new byte[32][]; // массив итерационных констант
        public byte[][] iterK = new byte[10][]; // массив итерационных ключей
        public byte[] Encript(byte[] file, byte[] masterKey)
        {
            KuzKeyGen(masterKey);
            int NumOfBlocks; // Определение кол-ва блоков по 16 байт
            int NumberOfNull; // Определение кол-ва недостающих байт последнего блока
            byte[] OriginText = file;
            byte[] encrText = new byte[file.Length]; // Массив для хранения зашифрованных байтов
            if ((file.Length % 16) == 0)
            {
                NumOfBlocks = file.Length / 16;
            }
            else
            {
                NumOfBlocks = (file.Length / 16) + 1;
                NumberOfNull = NumOfBlocks * 16 - file.Length;
                Array.Resize(ref OriginText, OriginText.Length + NumberOfNull);
                if (NumberOfNull == 1) 
                { 
                    OriginText[OriginText.Length - 1] = 0x80; 
                }
                else
                {
                    for (int i = OriginText.Length - 1; i >= 0; i--)
                    {
                        if (i == OriginText.Length - 1)
                        {
                            OriginText[OriginText.Length - 1] = 0x81;
                        }
                        else if (OriginText[i] != 0)
                        {
                            OriginText[i + 1] = 0x01;
                            break;
                        }
                    }
                }
            }
            for (int i = 0; i < NumOfBlocks; i++) // Операция зашифровки
            {
                byte[] block = new byte[16];
                for (int j = 0; j < 16; j++)
                {
                    block[j] = OriginText[i * 16 + j];
                }

                for (int j = 0; j < 9; j++)
                {
                    block = KuznechikCryptoProtocolUtils.KuzX(block, iterK[j]);
                    block = KuznechikCryptoProtocolUtils.KuzS(block);
                    block = KuznechikCryptoProtocolUtils.KuzL(block);

                }

                block = KuznechikCryptoProtocolUtils.KuzX(block, iterK[9]);
                for (int j = 0; j < 16; j++)
                {
                    encrText[i * 16 + j] = block[j];
                }
            }
            return encrText;
        }

        public byte[] Decript(byte[] file, byte[] masterKey)
        {
            KuzKeyGen(masterKey);
            int NumOfBlocks = file.Length / 16; // Определение кол-ва блоков по 16 байт
            byte[] OriginText = file;
            byte[] decrText = new byte[file.Length]; // Массив для хранения зашифрованных байтов
            for (int i = 0; i < NumOfBlocks; i++)
            {
                byte[] block = new byte[16];
                for (int j = 0; j < 16; j++)
                {
                    block[j] = OriginText[i * 16 + j];
                }

                block = KuznechikCryptoProtocolUtils.KuzX(block, iterK[9]);

                for (int j = 8; j >= 0; j--)
                {
                    block = KuznechikCryptoProtocolUtils.KuzLReverse(block);
                    block = KuznechikCryptoProtocolUtils.KuzSReverse(block);
                    block = KuznechikCryptoProtocolUtils.KuzX(block, iterK[j]);

                }

                for (int j = 0; j < 16; j++)
                {
                    decrText[i * 16 + j] = block[j];
                }
            }
            return decrText;
        }
        private void KuzKeyGen(byte[] mas_key)
        {
            //Генерация_раундовых_констант
            byte[][] iterNum = new byte[32][];
            for (int i = 0; i < 32; i++)
            {
                iterNum[i] = [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, Convert.ToByte(i + 1)];
                iterC[i] = KuznechikCryptoProtocolUtils.KuzL(iterNum[i]);
            }

            //Генерация_первых_2-х_ключей
            byte[] A = new byte[16];
            for (int i = 0; i < 16; i++) A[i] = mas_key[i];
            byte[] B = new byte[16];
            int j = 0;
            for (int i = 16; i < 32; i++)
            {
                B[j] = mas_key[i];
                j++;
            }
            j = 0;
            iterK[0] = A;
            iterK[1] = B;

            byte[] C = new byte[16];
            byte[] D = new byte[16];

            //Генерация_остальных_ключей
            for (int i = 0; i < 4; i++)
            {
                KuznechikCryptoProtocolUtils.KuzF(A, B, ref C, ref D, iterC[0 + 8 * i]);
                KuznechikCryptoProtocolUtils.KuzF(C, D, ref A, ref B, iterC[1 + 8 * i]);
                KuznechikCryptoProtocolUtils.KuzF(A, B, ref C, ref D, iterC[2 + 8 * i]);
                KuznechikCryptoProtocolUtils.KuzF(C, D, ref A, ref B, iterC[3 + 8 * i]);
                KuznechikCryptoProtocolUtils.KuzF(A, B, ref C, ref D, iterC[4 + 8 * i]);
                KuznechikCryptoProtocolUtils.KuzF(C, D, ref A, ref B, iterC[5 + 8 * i]);
                KuznechikCryptoProtocolUtils.KuzF(A, B, ref C, ref D, iterC[6 + 8 * i]);
                KuznechikCryptoProtocolUtils.KuzF(C, D, ref A, ref B, iterC[7 + 8 * i]);
                iterK[2 * i + 2] = A;
                iterK[2 * i + 3] = B;
            }
        }
    }
}
