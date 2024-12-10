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
var output2 = Part2(input, 8);
stopwatch.Stop();
Console.WriteLine($"Output Part2: {output2}");
Console.WriteLine($"Done in Time: {stopwatch.ElapsedMilliseconds} ms");

object Part1(string input)
{
    var map = input.Split(Environment.NewLine)
                   .Select(x => x.ToCharArray())
                   .ToArray();

    var groups = map.WhereCell(x => x.Value != '.')
                    .GroupBy(x => x.Value)
                    .Select(s1 => s1.Select(s2 => new Point(s2.ColumnId, s2.RowId))
                                    .ToList())
                    .ToList();

    var maxY = map.Length - 1;
    var maxX = map.First().Length - 1;

    var points = new HashSet<string>();
    foreach (var antens in groups)
    {
        for (int i = 0; i < antens.Count; i++)
        {
            var antena = antens[i];
            foreach (var otherAntena in antens.Skip(i + 1))
            {
                var point1 = antena.Copy();
                point1.Flip(otherAntena, PointFlip.Vertical);
                point1.Flip(otherAntena, PointFlip.Horizontal);

                if (point1.X >= 0 && point1.X <= maxX && point1.Y >= 0 && point1.Y <= maxY)
                    points.Add($"{point1.X}_{point1.Y}");

                var point2 = otherAntena.Copy();
                point2.Flip(antena, PointFlip.Vertical);
                point2.Flip(antena, PointFlip.Horizontal);

                if (point2.X >= 0 && point2.X <= maxX && point2.Y >= 0 && point2.Y <= maxY)
                    points.Add($"{point2.X}_{point2.Y}");
            }
        }
    }

    return points.Count;
}

object Part2(string input, int maxLength)
{
    var map = input.Split(Environment.NewLine)
                   .Select(x => x.ToCharArray())
                   .ToArray();

    var groups = map.WhereCell(x => x.Value != '.')
                    .GroupBy(x => x.Value)
                    .Select(s1 => s1.Select(s2 => new Point(s2.ColumnId, s2.RowId))
                                    .ToList())
                    .ToList();

    var maxY = map.Length - 1;
    var maxX = map.First().Length - 1;

    var points = new HashSet<string>();
    foreach (var antens in groups)
    {
        for (int i = 0; i < antens.Count; i++)
        {
            var antena = antens[i];
            foreach (var otherAntena in antens.Skip(i + 1))
            {
                points.Add($"{antena.X}_{antena.Y}");
                points.Add($"{otherAntena.X}_{otherAntena.Y}");

                var point1 = antena.Copy();
                point1.Flip(otherAntena, PointFlip.Vertical);
                point1.Flip(otherAntena, PointFlip.Horizontal);
                var delta1 = new Point(point1.X - otherAntena.X, point1.Y - otherAntena.Y);
                while (true)
                {
                    if (point1.X >= 0 && point1.X <= maxX && point1.Y >= 0 && point1.Y <= maxY)
                    {
                        points.Add($"{point1.X}_{point1.Y}");
                        point1.Move(delta1);
                        continue;
                    }

                    break;
                }

                var point2 = otherAntena.Copy();
                point2.Flip(antena, PointFlip.Vertical);
                point2.Flip(antena, PointFlip.Horizontal);
                var delta2 = new Point(point2.X - antena.X, point2.Y - antena.Y);
                while (true)
                {
                    if (point2.X >= 0 && point2.X <= maxX && point2.Y >= 0 && point2.Y <= maxY)
                    {
                        points.Add($"{point2.X}_{point2.Y}");
                        point2.Move(delta2);
                        continue;
                    }

                    break;
                }
            }
        }
    }

    return points.Count;
}