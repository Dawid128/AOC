using AdventCodeExtension;
using System.Diagnostics;

string[] colors = ["red", "green", "blue"];

var input = File.ReadAllText($"Resources\\Input.txt"); //60
var stopwatch = Stopwatch.StartNew();

//Part 1
stopwatch.Start();
var output = Part1(input, (12, 13, 14));
stopwatch.Stop();
Console.WriteLine($"Output Part1: {output}");
Console.WriteLine($"Done in Time: {stopwatch.ElapsedMilliseconds} ms");

//Part 2
stopwatch.Start();
output = Part2(input);
stopwatch.Stop();
Console.WriteLine($"Output Part2: {output}");
Console.WriteLine($"Done in Time: {stopwatch.ElapsedMilliseconds} ms");

object Part1(string input, (int R, int G, int B) maxCubes)
{
    return input.Split(Environment.NewLine)
                .Select(x => SelectCubesSets(x.Split(":")[1]).ToArray()) //Select Sets of cubes for each game
                .Select((x, index) => (Game: index + 1, CubesSets: x))
                .Where(game => !game.CubesSets.Any(x => x.R > maxCubes.R || x.G > maxCubes.G || x.B > maxCubes.B)) //Select only games meets the conditions
                .Select(x => x.Game)
                .Aggregate((x, y) => x + y);
}

object Part2(string input)
{
    return input.Split(Environment.NewLine)
                .Select(x => SelectCubesSets(x.Split(":")[1]).ToArray())
                .Select(x => new[] { x.Max(m => m.R), x.Max(m => m.G), x.Max(m => m.B) }) 
                .Select(x => x.Product())
                .Aggregate((x, y) => x + y);
}

IEnumerable<(int R, int G, int B)> SelectCubesSets(string input)
{
    var setsStr = input.Split(';');
    foreach (var setStr in setsStr)
    {
        var cubesStr = setStr.Split(", ");

        int[] rgb = [0, 0, 0];
        for (int i = 0; i < colors.Length; i++)
        {
            var color = colors[i];
            var cubeStr = cubesStr.SingleOrDefault(x => x.Contains(color));
            if (cubeStr is null)
                continue;

            rgb[i] = int.Parse(new string(cubeStr.Where(char.IsDigit).ToArray()));
        }

        yield return (rgb[0], rgb[1], rgb[2]);
    }
}