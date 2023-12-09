using AdventCodeExtension;
using AdventCodeExtension.Models;
using System.Diagnostics;

var input = File.ReadAllText($"Resources\\Input.txt");
var stopwatch = Stopwatch.StartNew();

//Part 1
stopwatch.Start();
var output = Part1(input);
stopwatch.Stop();
Console.WriteLine($"Output Part1: {output}");
Console.WriteLine($"Done in Time: {stopwatch.ElapsedMilliseconds} ms");

//Part 2
stopwatch.Start();
output = Part2(input);
stopwatch.Stop();
Console.WriteLine($"Output Part2: {output}");
Console.WriteLine($"Done in Time: {stopwatch.ElapsedMilliseconds} ms");

object Part1(string input)
{
    var instructions = input.Split(Environment.NewLine)
                            .Select(x => (Command: (CommandEnum)x[0], Value: int.Parse(new string(x.Skip(1).ToArray()))))
                            .ToList();

    var shipPoint = new Point(0, 0);
    var compass = new Compass(DirectionEnum.East);
    foreach (var (command, value) in instructions)
        ExecuteInstructionPart1(command, value, ref shipPoint, ref compass);

    return Math.Abs(shipPoint.X) + Math.Abs(shipPoint.Y);
}

object Part2(string input)
{
    var instructions = input.Split(Environment.NewLine)
                            .Select(x => (Command: (CommandEnum)x[0], Value: int.Parse(new string(x.Skip(1).ToArray()))))
                            .ToList();

    var shipPoint = new Point(0, 0);
    var wayPoint = new Point(10, 1);
    foreach (var (command, value) in instructions)
        ExecuteInstructionPart2(command, value, ref wayPoint, ref shipPoint);

    return Math.Abs(shipPoint.X) + Math.Abs(shipPoint.Y);
}

void ExecuteInstructionPart1(CommandEnum command, int value, ref Point position, ref Compass compass)
{
    var directionCoordinates = new Point(0, 0);
    switch (command)
    {
        case CommandEnum.Forward:
            directionCoordinates = compass.GetDirectionCoordinates();
            break;

        case CommandEnum.North:
        case CommandEnum.South:
        case CommandEnum.East:
        case CommandEnum.West:
            directionCoordinates = Compass.GetDirectionCoordinates((DirectionEnum)command);
            break;

        case CommandEnum.Left:
            while (value > 0)
            {
                compass.RotateLeft();
                value -= 90;
            }
            return;

        case CommandEnum.Right:
            while (value > 0)
            {
                compass.RotateRight();
                value -= 90;
            }
            return;
    }

    position.Move(directionCoordinates.X * value, directionCoordinates.Y * value);
}

void ExecuteInstructionPart2(CommandEnum command, int value, ref Point wayPoint, ref Point shipPoint)
{
    switch (command)
    {
        case CommandEnum.Forward:
            shipPoint.Move(wayPoint * value);
            break;

        case CommandEnum.North:
        case CommandEnum.South:
        case CommandEnum.East:
        case CommandEnum.West:
            wayPoint.Move(Compass.GetDirectionCoordinates((DirectionEnum)command) * value);
            break;

        case CommandEnum.Left:
            while (value > 0)
            {
                wayPoint.Rotate(new Point(0, 0), PointRotate.Left);
                value -= 90;
            }
            return;

        case CommandEnum.Right:
            while (value > 0)
            {
                wayPoint.Rotate(new Point(0,0), PointRotate.Right);
                value -= 90;
            }
            return;
    }
}

public class Compass
{
    private static readonly Dictionary<DirectionEnum, Point> Directions = new()
    {
        { DirectionEnum.North, new Point(0, 1) },
        { DirectionEnum.East, new Point(1, 0) },
        { DirectionEnum.South, new Point(0, -1) },
        { DirectionEnum.West, new Point(-1, 0) },
    };

    public DirectionEnum Direction { get; private set; }

    public Compass(DirectionEnum direction)
    {
        Direction = direction;
    }

    public void RotateRight() => Direction = Directions.ElementAt((Directions.Count + Directions.IndexOfKey(Direction) + 1) % Directions.Count).Key;

    public void RotateLeft() => Direction = Directions.ElementAt((Directions.Count + Directions.IndexOfKey(Direction) - 1) % Directions.Count).Key;

    public Point GetDirectionCoordinates() => Directions[Direction].Copy();

    public static Point GetDirectionCoordinates(DirectionEnum direction) => Directions[direction].Copy();
}

public enum DirectionEnum
{
    North = 'N',
    South = 'S',
    East = 'E',
    West = 'W',
}

public enum CommandEnum
{
    North = 'N',
    South = 'S',
    East = 'E',
    West = 'W',
    Left = 'L',
    Right = 'R',
    Forward = 'F',
}