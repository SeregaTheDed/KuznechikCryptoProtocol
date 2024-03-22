namespace KuznyechikCryptoProtocol.KuznyechikCryptoProtocol
{
    public class Operations
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
                output[i] = Constants.Pi[input[i]];
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
                output[i] = Constants.Pi_Reverse[input[i]];
            }
            return output;
        }

        /// <summary>
        /// Умножение в поле
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public byte MulInGF(byte a, byte b)
        {
            byte p = 0;
            byte hi_bit_set;
            for (byte counter = 0; counter < 8 && a != 0 && b != 0; counter++)
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
                a_15 ^= MulInGF(input[i], Constants.LVec[i]);
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
            byte[] result = input;
            for (int i = 0; i < 16; i++)
            {
                result = R(result);
            }
            return result;
        }

        public byte[] RReverse(byte[] input)
        {
            byte sum = input[0];
            byte[] result = new byte[16];
            for (int i = 0; i < 15; i++)
            {
                result[i] = input[i + 1];
            }
            for (int i = 15; i >= 0; i--)
            {
                sum ^= MulInGF(result[i], Constants.LVec[i]);
            }
            result[15] = sum;
            return result;
        }

        public virtual byte[] LReverse(byte[] input)
        {
            byte[] result = input;
            for (int i = 0; i < 16; i++)
            {
                result = RReverse(result);
            }
            return result;
        }

        public override string ToString()
        {
            return "";
        }
    }
}
