
using AdventCodeExtension.Models;
using System.Security.Cryptography;
using System.Text;

namespace AdventCodeExtension
{
    public static class ArrayExtensions
    {
        public static string GenerateUniqueId(this char[,] array2D)
        {
            int rows = array2D.GetLength(0);
            int cols = array2D.GetLength(1);
            int[] flattenedArray = new int[rows * cols];

            int index = 0;
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    flattenedArray[index++] = array2D[i, j];

            string idString = string.Join("", flattenedArray);
            var hashBytes = MD5.HashData(Encoding.UTF8.GetBytes(idString));
            var sb = new StringBuilder();

            for (int i = 0; i < hashBytes.Length; i++)
                sb.Append(hashBytes[i].ToString("x2"));

            return sb.ToString();
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

        public static IEnumerable<Cell<T>> WhereCell<T>(this T[][] jaggedArray, Func<Cell<T>, bool> predicate)
        => jaggedArray.SelectMany((x, rowId) => x.Select((value, columnId) => new Cell<T>(rowId, columnId, value)))
                      .Where(predicate);

        public static IEnumerable<Cell<T>> SelectAdjacent4<T>(this T[][] jaggedArray, int rowId, int columnId, Func<Cell<T>, bool> predicate)
        {
            foreach (var (y ,x) in new[] { (-1, 0), (0, 1), (1, 0), (0, -1) })
            {
                var row = jaggedArray.ElementAtOrDefault(rowId + y);
                if (row is null)
                    continue;

                var value = row.ElementAtOrDefault(columnId + x);
                if (value is null)
                    continue;

                var cell = new Cell<T>(rowId + y, columnId + x, value);
                if (predicate(cell))
                    yield return cell;
            }
        }

        public static Point SinglePoint<T>(this T[][] jaggedArray, Func<T, bool> predicate)
        {
            var points = new List<Point>();

            for (int rowId = 0; rowId < jaggedArray.Length; rowId++)
                for (int columnId = 0; columnId < jaggedArray[rowId].Length; columnId++)
                {
                    if (!predicate.Invoke(jaggedArray[rowId][columnId])) 
                        continue;

                    points.Add(new Point(columnId, rowId));
                }

            return points.Single();
        }

        public static bool AnyAdjacent8<T>(this T[][] jaggedArray, int rowId, int columnId, Func<T, bool> predicate)
        {
            foreach (var y in new[] { -1, 0, 1 })
                foreach (var x in new[] { -1, 0, 1 })
                    if (y != 0 || x != 0)
                    {
                        var row = jaggedArray.ElementAtOrDefault(rowId + y);
                        if (row is null)
                            continue;

                        var cell = row.ElementAtOrDefault(columnId + x);
                        if (cell is null)
                            continue;

                        if (predicate(cell))
                            return true;
                    }

            return false;
        }

        public static int CountAdjacent8<T>(this T[][] jaggedArray, int rowId, int columnId, Func<T, bool> predicate)
        {
            var result = 0;

            foreach (var y in new[] { -1, 0, 1 })
                foreach (var x in new[] { -1, 0, 1 })
                    if (y != 0 || x != 0)
                    {
                        var row = jaggedArray.ElementAtOrDefault(rowId + y);
                        if (row is null)
                            continue;

                        var cell = row.ElementAtOrDefault(columnId + x);
                        if (cell is null)
                            continue;

                        if (predicate(cell))
                            result++;
                    }

            return result;
        }

        public static IEnumerable<Cell<T>> SelectAdjacent8<T>(this T[][] jaggedArray, int rowId, int columnId, Func<T, bool> predicate)
        {
            foreach (var y in new[] { -1, 0, 1 })
                foreach (var x in new[] { -1, 0, 1 })
                    if (y != 0 || x != 0)
                    {
                        var row = jaggedArray.ElementAtOrDefault(rowId + y);
                        if (row is null)
                            continue;

                        var cell = row.ElementAtOrDefault(columnId + x);
                        if (cell is null)
                            continue;

                        if (predicate(cell))
                            yield return new Cell<T>(rowId + y, columnId + x, cell);
                    }
        }

        public static bool ContainsMinCountAdjacent8<T>(this T[][] jaggedArray, int rowId, int columnId, int minCount, Func<T, bool> predicate)
        {
            var count = 0;
            foreach (var y in new[] { -1, 0, 1 })
                foreach (var x in new[] { -1, 0, 1 })
                    if (y != 0 || x != 0)
                    {
                        var row = jaggedArray.ElementAtOrDefault(rowId + y);
                        if (row is null)
                            continue;

                        var cell = row.ElementAtOrDefault(columnId + x);
                        if (cell is null)
                            continue;

                        if (predicate(cell))
                            count++;

                        if (count >= minCount)
                            return true;
                    }

            return false;
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

        public static IEnumerable<T[]> TakeRows<T>(this T[,] original)
        {
            for (int rowId = 0; rowId < original.GetLength(0); rowId++)
            {
                var row = new T[original.GetLength(1)];
                for (int columnId = 0; columnId < original.GetLength(1); columnId++)
                    row[columnId] = original[rowId, columnId];

                yield return row;
            }
        }

        public static T[] TakeColumn<T>(this T[,] original, int columnId)
        {
            var result = new T[original.GetLength(0)];
            for (int i = 0; i < result.Length; i++)
                result[i] = original[i, columnId];

            return result;
        }

        public static IEnumerable<T[]> TakeColumns<T>(this T[,] original)
        {
            for (int columnId = 0; columnId < original.GetLength(1); columnId++)
            {
                var column = new T[original.GetLength(0)];
                for (int rowId = 0; rowId < original.GetLength(0); rowId++)
                    column[rowId] = original[rowId, columnId];

                yield return column;
            }
        }

        public static IEnumerable<T[]> TakeDiagonals<T>(this T[,] array)
        {
            var rows = array.GetLength(0);
            var cols = array.GetLength(1);

            for (int startRow = rows - 1; startRow >= 0; startRow--)
            {
                var diagonal = new List<T>();
                var r = startRow;
                var c = 0;

                while (r < rows && c < cols)
                {
                    diagonal.Add(array[r, c]);
                    r++;
                    c++;
                }
                yield return diagonal.ToArray();
            }

            for (int startCol = 1; startCol < cols; startCol++)
            {
                var diagonal = new List<T>();
                var r = 0;
                var c = startCol;

                while (r < rows && c < cols)
                {
                    diagonal.Add(array[r, c]);
                    r++;
                    c++;
                }
                yield return diagonal.ToArray();
            }
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

        public static T[,] InsertRow<T>(this T[,] array, int rowIndex, T[] newRow)
        {
            int numRows = array.GetLength(0);
            int numCols = array.GetLength(1);

            if (rowIndex < 0 || rowIndex > numRows)
                throw new ArgumentOutOfRangeException(nameof(rowIndex), "Invalid row index");

            if (newRow.Length != numCols)
                throw new ArgumentException("The length of the new row must match the number of columns in the array", nameof(newRow));

            T[,] newArray = new T[numRows + 1, numCols];
            for (int i = 0; i < rowIndex; i++)
                for (int j = 0; j < numCols; j++)
                    newArray[i, j] = array[i, j];

            for (int j = 0; j < numCols; j++)
                newArray[rowIndex, j] = newRow[j];

            for (int i = rowIndex + 1; i < numRows + 1; i++)
                for (int j = 0; j < numCols; j++)
                    newArray[i, j] = array[i - 1, j];

            return newArray;
        }

        public static T[,] InsertColumn<T>(this T[,] array, int colIndex, T[] newColumn)
        {
            int numRows = array.GetLength(0);
            int numCols = array.GetLength(1);

            if (colIndex < 0 || colIndex > numCols)
                throw new ArgumentOutOfRangeException(nameof(colIndex), "Invalid column index");

            if (newColumn.Length != numRows)
                throw new ArgumentException("The length of the new column must match the number of rows in the array", nameof(newColumn));

            T[,] newArray = new T[numRows, numCols + 1];
            for (int i = 0; i < numRows; i++)
                for (int j = 0; j < colIndex; j++)
                    newArray[i, j] = array[i, j];

            for (int i = 0; i < numRows; i++)
                newArray[i, colIndex] = newColumn[i];

            for (int i = 0; i < numRows; i++)
                for (int j = colIndex + 1; j < numCols + 1; j++)
                    newArray[i, j] = array[i, j - 1];

            return newArray;
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

        public static T[,] Duplicate<T>(this T[,] original)
        {
            var rows = original.GetLength(0);
            var cols = original.GetLength(1);
            var result = new T[rows, cols];

            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    result[i, j] = original[i, j];

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
