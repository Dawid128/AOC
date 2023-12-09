using System.Collections;

namespace AdventCodeExtension
{
    //Most significant bit is at the end of bit array
    //Least significant bit is at the start of bit array
    public static class BitArrayExtensions
    {
        public static int ToInt(this BitArray bitArray)
        {
            if (bitArray.Count > 32)
                throw new ArgumentException("BitArray is too large to convert to int");

            int[] array = [1];
            bitArray.CopyTo(array, 0);
            return array[0];
        }

        public static long ToLong(this BitArray bitArray)
        {
            if (bitArray.Length > 64)
                throw new ArgumentException("BitArray is too large to convert to long");

            long result = 0;
            for (var i = 0; i < bitArray.Length; i++)
                if (bitArray[i])
                    result |= (1L << i);

            return result;
        }

        public static BitArray Reverse(this BitArray originalBits)
        {
            var length = originalBits.Length;
            var reversedBits = new BitArray(length);

            for (int i = 0; i < length; i++)
                reversedBits[i] = originalBits[length - 1 - i];

            return reversedBits;
        }

        public static BitArray ToBitArray(this long value, int numberOfBits)
        {
            var bitArray = new BitArray(numberOfBits);
            for (int i = 0; i < numberOfBits; i++)
                bitArray[i] = (value & (1L << i)) != 0;

            return bitArray;
        }

        public static BitArray ToBitArray(this IList<bool> bools)
        {
            var result = new BitArray(bools.Count);
            for (int i = 0; i < bools.Count; i++)
                if (bools[i])
                    result.Set(i, true);

            return result;
        }
    }
}
