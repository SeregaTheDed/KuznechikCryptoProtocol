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
                if (input[i] != 0)
                {
                    outputBlock = X(outputBlock, LookupTableOfTransformationL[i][input[i] - 1]);
                }
            }
            return outputBlock;
        }

        public override byte[] LReverse(byte[] input)
        {
            int blockSizeMinusOne = BlockSize - 1;
            byte[] outputBlock = new byte[BlockSize];
            for (int i = 0; i < BlockSize; ++i)
            {
                if (input[i] != 0)
                {
                    outputBlock = X(outputBlock, LookupTableOfTransformationL[blockSizeMinusOne - i][input[i] - 1].Reverse().ToArray());
                }
            }
            return outputBlock;
        }
    }
}
