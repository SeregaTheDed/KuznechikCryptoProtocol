namespace KuznechikCryptoProtocol.alghoritm
{
    public class KCPOperations
    {

        /// <summary>
        /// Сложение по модулю 2
        /// </summary>
        /// <param name="input1"></param>
        /// <param name="input2"></param>
        /// <returns></returns>
        public byte[] X(byte[] input1, byte[] input2)
        {
            byte[] output = new byte[16];
            for (int i = 0; i < 16; i++)
            {
                output[i] = Convert.ToByte(input1[i] ^ input2[i]);
            }
            return output;
        }

        /// <summary>
        /// Генерация раундовых ключей
        /// </summary>
        /// <param name="input1"></param>
        /// <param name="input2"></param>
        /// <param name="output1"></param>
        /// <param name="output2"></param>
        /// <param name="round_C"></param>
        public void F(byte[] input1, byte[] input2, ref byte[] output1, ref byte[] output2, byte[] round_C)
        {
            byte[] state = X(input1, round_C);
            state = S(state);
            state = L(state);
            output1 = X(state, input2);
            output2 = input1;
        }

        /// <summary>
        /// Нелинейное преобразование (Операция S)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public byte[] S(byte[] input) // Прямое нелинейное преобразование S
        {
            byte[] output = new byte[16];
            for (int i = 0; i < 16; i++)
            {
                output[i] = KCPConstants.Pi[input[i]];
            }
            return output;
        }

        /// <summary>
        /// Обратное нелинейное преобразование S
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public byte[] SReverse(byte[] input)
        {
            byte[] output = new byte[16];
            for (int i = 0; i < 16; i++)
            {
                output[i] = KCPConstants.Pi_Reverse[input[i]];
            }
            return output;
        }

        /// <summary>
        /// Линейное преобразование (Операция L)
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public byte MulInGF(byte a, byte b)
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

        public byte[] R(byte[] input)
        {
            byte a_15 = 0;
            byte[] state = new byte[16];
            for (int i = 0; i <= 15; i++)
            {
                a_15 ^= MulInGF(input[i], KCPConstants.LVec[i]);
            }
            for (int i = 15; i > 0; i--)
            {
                state[i] = input[i - 1];
            }
            state[0] = a_15;
            return state;
        }

        public virtual byte[] L(byte[] input)
        {
            byte[] state = input;
            for (int i = 0; i < 16; i++)
            {
                state = R(state);
            }
            return state;
        }

        public virtual byte[] RReverse(byte[] input)
        {
            byte a_15 = input[0];
            byte[] state = new byte[16];
            for (int i = 0; i < 15; i++)
            {
                state[i] = input[i + 1];
            }
            for (int i = 15; i >= 0; i--)
            {
                a_15 ^= MulInGF(state[i], KCPConstants.LVec[i]);
            }
            state[15] = a_15;
            return state;
        }

        public byte[] LReverse(byte[] input)
        {
            byte[] state = input;
            for (int i = 0; i < 16; i++)
            {
                state = RReverse(state);
            }
            return state;
        }
    }
}
