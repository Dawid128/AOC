
namespace _2020_05_BinaryBoarding.Helpers
{
    static public class DrawingHelper
    {
        public static void DrawPlane((int Row, int Column) sizePlane, HashSet<int> seats)
        {
            Console.WriteLine("----------Start Draw Plane----------");
            var currentPosition = Console.GetCursorPosition();

            for (int i = 0; i < sizePlane.Column; i++)
                for (int j = 0; j < sizePlane.Row; j++)
                    if (seats.Any(x=>x == (j * 8 + i)))
                    {
                        Console.SetCursorPosition(currentPosition.Left + i, currentPosition.Top + j);
                        Console.WriteLine("O");
                    }

            for (int i = 0; i < sizePlane.Row; i++)
            {
                Console.SetCursorPosition(10, currentPosition.Top + i);
                Console.WriteLine($"Row {i}");
            }

            Console.WriteLine("----------End Draw Plane----------");
        }
    }
}
