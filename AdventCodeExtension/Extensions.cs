using AdventCodeExtension.Models;
using System.Collections;

namespace AdventCodeExtension
{
    public static class Extensions
    {
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            foreach (T item in source)
            {
                action(item);
                yield return item;
            }
        }

        public static IEnumerable<int> ElementsAt(this IEnumerable<int> input, IList<int> indexes)
        {
            foreach (var index in indexes)
                yield return input.ElementAt(index);
        }

        public static long Product(this IEnumerable<int> input)
        {
            var result = (long)input.First();
            foreach (var item in input.Skip(1))
                result *= item;

            return result;
        }

        public static int IndexOfKey<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key)
        {
            if (dictionary == null)
                throw new ArgumentNullException(nameof(dictionary));

            int index = 0;
            foreach (var kvp in dictionary)
            {
                if (EqualityComparer<TKey>.Default.Equals(kvp.Key, key))
                    return index;

                index++;
            }

            return -1;
        }

        public static void AddOrUpdate<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue value) where TKey : IEquatable<TKey>
        {
            if (dictionary.ContainsKey(key))
            {
                dictionary[key] = value;
                return;
            }

            dictionary.Add(key, value);
        }

        public static KeyValuePair<TKey, TValue> FirstAndRemove<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, Func<KeyValuePair<TKey, TValue>, bool> predicate)
        {
            var first = dictionary.First(predicate);
            dictionary.Remove(first.Key);
            return first;
        }

        public static T[,] To2DArray<T>(this T[][] jaggedArray)
        {
            int rows = jaggedArray.Length;
            int columns = jaggedArray[0].Length;

            T[,] multidimensionalArray = new T[rows, columns];

            for (int i = 0; i < rows; i++)
                for (int j = 0; j < columns; j++)
                    multidimensionalArray[i, j] = jaggedArray[i][j];

            return multidimensionalArray;
        }

        public static T[][] TakeRange<T>(this T[][] jaggedArray, (int RowId, int ColumnId) startRange, (int RowId, int ColumnId) endRange)
        {
            int rows = endRange.RowId - startRange.RowId + 1;
            int columns = endRange.ColumnId - startRange.ColumnId + 1;

            var result = new T[rows][].Select(_ => new T[columns]).ToArray();
            for (int rowId = startRange.RowId; rowId <= endRange.RowId; rowId++)
                for (int columnId = startRange.ColumnId; columnId <= endRange.ColumnId; columnId++)
                    result[rowId - startRange.RowId][columnId - startRange.ColumnId] = jaggedArray[rowId][columnId];

            return result;
        }

        public static T[][] ToJaggedArray<T>(this T[,] array2D)
        {
            int rows = array2D.GetLength(0);
            int columns = array2D.GetLength(1);

            var result = new T[rows][];
            for (int rowId = 0; rowId < rows; rowId++)
            {
                result[rowId] = new T[columns];
                for (int columnId = 0; columnId < columns; columnId++)
                    result[rowId][columnId] = array2D[rowId, columnId];
            }

            return result;
        }

        public static T[] TakeRow<T>(this T[,] original, int rowId)
        {
            var result = new T[original.GetLength(1)];
            for (int i = 0; i < result.Length; i++)
                result[i] = original[rowId, i];

            return result;
        }

        public static T[] TakeColumn<T>(this T[,] original, int columnId)
        {
            var result = new T[original.GetLength(0)];
            for (int i = 0; i < result.Length; i++)
                result[i] = original[i, columnId];

            return result;
        }

        public static T[,] TakeRange<T>(this T[,] original, (int RowId, int ColumnId) startRange, (int RowId, int ColumnId) endRange)
        {
            int rows = endRange.RowId - startRange.RowId + 1;
            int columns = endRange.ColumnId - startRange.ColumnId + 1;
            T[,] result = new T[rows, columns];

            for (int rowId = startRange.RowId; rowId <= endRange.RowId; rowId++)
                for (int columnId = startRange.ColumnId; columnId <= endRange.ColumnId; columnId++)
                    result[rowId - startRange.RowId, columnId - startRange.ColumnId] = original[rowId, columnId];

            return result;
        }

        public static void InsertRange<T>(this T[,] original, (int RowId, int ColumnId) startRange, T[,] insert)
        {
            (int RowId, int ColumnId) endRange = (insert.GetLength(0) + startRange.RowId - 1, insert.GetLength(1) + startRange.ColumnId - 1);

            for (int rowId = startRange.RowId; rowId <= endRange.RowId; rowId++)
                for (int columnId = startRange.ColumnId; columnId <= endRange.ColumnId; columnId++)
                    original[rowId, columnId] = insert[rowId - startRange.RowId, columnId - startRange.ColumnId];
        }

        public static T[,] TransposeArray<T>(this T[,] original)
        {
            int rows = original.GetLength(0);
            int columns = original.GetLength(1);
            T[,] transposed = new T[columns, rows];

            for (int i = 0; i < rows; i++)
                for (int j = 0; j < columns; j++)
                    transposed[j, i] = original[i, j];

            return transposed;
        }

        public static T[,] FlipVertical<T>(this T[,] original)
        {
            var rows = original.GetLength(0);
            var cols = original.GetLength(1);
            var result = new T[rows, cols];

            for (var i = 0; i < rows; i++)
                for (var j = 0; j < cols; j++)
                    result[i, j] = original[rows - 1 - i, j];

            return result;
        }

        public static T[,] FlipHorizontal<T>(this T[,] original)
        {
            var rows = original.GetLength(0);
            var cols = original.GetLength(1);
            var result = new T[rows, cols];

            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    result[i, j] = original[i, cols - 1 - j];

            return result;
        }

        public static T[,] RotateRight<T>(this T[,] original)
        {
            var rows = original.GetLength(0);
            var cols = original.GetLength(1);
            var result = new T[rows, cols];

            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    result[j, rows - 1 - i] = original[i, j];

            return result;
        }

        public static T[,] RotateLeft<T>(this T[,] original)
        {
            var rows = original.GetLength(0);
            var cols = original.GetLength(1);
            var result = new T[rows, cols];

            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    result[cols - 1 - j, i] = original[i, j];

            return result;
        }

        public static BitArray ToBitArray(this IList<bool> bools)
        {
            var result = new BitArray(bools.Count);
            for (int i = 0; i < bools.Count; i++)
                if (bools[i])
                    result.Set(i, true);

            return result;
        }

        public static int ToInt(this BitArray bits)
        {
            if (bits.Count > 32)
                throw new ArgumentException("BitArray contains too many bits to fit into an integer.");

            int[] array = new int[1];
            bits.CopyTo(array, 0);
            return array[0];
        }

        public static BitArray Reverse(this BitArray originalBits)
        {
            int length = originalBits.Length;
            BitArray reversedBits = new BitArray(length);

            for (int i = 0; i < length; i++)
                reversedBits[i] = originalBits[length - 1 - i];

            return reversedBits;
        }

        public static string ReplaceFirst(this string original, string search, string replacement)
        {
            int index = original.IndexOf(search);
            if (index == -1)
                return original;

            return original.Substring(0, index) + replacement + original.Substring(index + search.Length);
        }

        public static string ReplaceLast(this string original, string search, string replacement)
        {
            int index = original.LastIndexOf(search);
            if (index == -1)
                return original;

            return original.Substring(0, index) + replacement + original.Substring(index + search.Length);
        }
    }
}