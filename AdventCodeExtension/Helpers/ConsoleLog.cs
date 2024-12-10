using AdventCodeExtension.Models;

namespace AdventCodeExtension.Helpers
{
    public static class ConsoleLog
    {
        private static (int Left, int Top) _currentPosition;

        public static void WriteMap2D(IList<Cell<char>> map)
        {
            Console.WriteLine("----------START MAP 2D----------");
            Console.WriteLine();

            _currentPosition = Console.GetCursorPosition();
            foreach (var cell in map)
            {
                Console.SetCursorPosition(_currentPosition.Left + cell.ColumnId, _currentPosition.Top + cell.RowId);
                Console.WriteLine(cell.Value);
            }

            Console.SetCursorPosition(0, _currentPosition.Top + map.Max(x => x.RowId) + 2);
            Console.WriteLine("-----------END MAP 2D-----------");
        }

        public static void WriteCellOnMap2D(Cell<char> cell, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.SetCursorPosition(_currentPosition.Left + cell.ColumnId, _currentPosition.Top + cell.RowId);
            Console.WriteLine(cell.Value);
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
