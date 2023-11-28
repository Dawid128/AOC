
namespace _2020_20_JurassicJigsaw.Helpers
{
    static public class DrawingHelper
    {
        public static void DrawPlane(bool[,] map)
        {
            Console.WriteLine("----------Start Draw----------");
            var currentPosition = Console.GetCursorPosition();

            for (int i = 0; i < map.GetLength(0); i++)
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    Console.SetCursorPosition(currentPosition.Left + i, currentPosition.Top + j);
                    if (map[j, i])
                        Console.WriteLine("#");
                    else
                        Console.WriteLine(".");
                }
            Console.WriteLine("");
            Console.WriteLine("----------End Draw----------");
        }
    }
}
