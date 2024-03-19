namespace KuznechikCryptoProtocol.alghoritm
{
    public static class KuznechikCryptoProtocolUtils
    {

        // Сложение_по_модулю_2
        public static byte[] KuzX(byte[] input1, byte[] input2) // Преобразование Х (сложение 2х веторов по модулю 2)
        {
            byte[] output = new byte[16];
            for (int i = 0; i < 16; i++)
            {
                output[i] = Convert.ToByte(input1[i] ^ input2[i]);
            }
            return output;
        }

        //Генерация_раундовых_ключей
        public static void KuzF(byte[] input1, byte[] input2, ref byte[] output1, ref byte[] output2, byte[] round_C)
        {
            byte[] state = KuzX(input1, round_C);
            state = KuzS(state);
            state = KuzL(state);
            output1 = KuzX(state, input2);
            output2 = input1;
        }

        //Нелинейное_преобразование_(Операция_S)
        public static byte[] KuzS(byte[] input) // Прямое нелинейное преобразование S
        {
            byte[] output = new byte[16];
            for (int i = 0; i < 16; i++)
            {
                output[i] = KuznechikCryptoProtocolConstants.Pi[input[i]];
            }
            return output;
        }

        public static byte[] KuzSReverse(byte[] input) // Обратное нелинейное преобразование S
        {
            byte[] output = new byte[16];
            for (int i = 0; i < 16; i++)
            {
                output[i] = KuznechikCryptoProtocolConstants.Pi_Reverse[input[i]];
            }
            return output;
        }

        //Линейное_преобразование_(Операция_L)
        public static byte KuzMulInGF(byte a, byte b)
        {
            byte p = 0;
            byte counter;
            byte hi_bit_set;
            for (counter = 0; counter < 8 && a != 0 && b != 0; counter++)
            {
                if ((b & 1) != 0)
                    p ^= a;
                hi_bit_set = (byte)(a & 0x80);
                a <<= 1;
                if (hi_bit_set != 0)
                    a ^= 0xc3; /* x^8 + x^7 + x^6 + x + 1 */
                b >>= 1;
            }
            return p;
        }

        public static byte[] LVec = [148, 32, 133, 16, 194, 192, 1, 251, 1, 192, 194, 16, 133, 32, 148, 1];

        public static byte[] KuzR(byte[] input)
        {
            byte a_15 = 0;
            byte[] state = new byte[16];
            for (int i = 0; i <= 15; i++)
            {
                a_15 ^= KuzMulInGF(input[i], LVec[i]);
            }
            for (int i = 15; i > 0; i--)
            {
                state[i] = input[i - 1];
            }
            state[0] = a_15;
            return state;
        }

        public static byte[] KuzL(byte[] input)
        {
            byte[] state = input;
            for (int i = 0; i < 16; i++)
            {
                state = KuzR(state);
            }
            return state;
        }

        public static byte[] KuzRReverse(byte[] input)
        {
            byte a_15 = input[0];
            byte[] state = new byte[16];
            for (int i = 0; i < 15; i++)
            {
                state[i] = input[i + 1];
            }
            for (int i = 15; i >= 0; i--)
            {
                a_15 ^= KuzMulInGF(state[i], LVec[i]);
            }
            state[15] = a_15;
            return state;
        }

        public static byte[] KuzLReverse(byte[] input)
        {
            byte[] state = input;
            for (int i = 0; i < 16; i++)
            {
                state = KuzRReverse(state);
            }
            return state;
        }
    }
}
