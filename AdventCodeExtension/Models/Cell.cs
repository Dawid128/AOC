using System.Diagnostics;

namespace AdventCodeExtension.Models
{
    [DebuggerDisplay("[{ColumnId};{RowId}]={Value}")]
    public struct Cell<T>
    {
        public int RowId { get; set; }
        public int ColumnId { get; set; }
        public T Value { get; set; }

        public Cell(int rowId, int columnId, T value)
        {
            RowId = rowId;
            ColumnId = columnId;
            Value = value;
        }
    }
}
