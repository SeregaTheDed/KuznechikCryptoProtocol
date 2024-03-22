namespace KuznyechikCryptoProtocol.KuznyechikCryptoProtocol
{
    internal class OperationsWithMatrix : Operations
    {
        private readonly byte[][][] LookupTableOfTransformationL;
        private readonly int BlockSize = Constants.BlockSize;

        public OperationsWithMatrix(byte[][][] lookupTableOfTransformationL)
        {
            LookupTableOfTransformationL = lookupTableOfTransformationL;
        }

        public override byte[] L(byte[] input)
        {
            byte[] outputBlock = new byte[BlockSize];
            for (int i = 0; i < BlockSize; ++i)
            {
                if (input[i] == 0)
                {
                    continue;
                }
                outputBlock = X(outputBlock, LookupTableOfTransformationL[i][input[i] - 1]);
            }
            return outputBlock;
        }

        public override byte[] LReverse(byte[] input)
        {
            byte[] outputBlock = new byte[BlockSize];
            for (int i = 0; i < BlockSize; ++i)
            {
                if (input[i] == 0)
                {
                    continue;
                }
                outputBlock = X(outputBlock, LookupTableOfTransformationL[BlockSize - i - 1][input[i] - 1].Reverse().ToArray());
            }
            return outputBlock;
        }

        public override string ToString()
        {
            return "_с_матрицей_на_L-операции";
        }
    }
}
