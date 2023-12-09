﻿
namespace AdventCodeExtension
{
    public static class ArrayExtensions
    {
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
    }
}