using AdventCodeExtension;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

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
    return input.Split($"{Environment.NewLine}{Environment.NewLine}") //Split to groups
                .Select(x => x.Replace(Environment.NewLine,"") //Ignore division on persons
                              .ToCharArray() //Select responses in group
                              .Distinct()
                              .Count()) 
                .Sum();
}

object Part2(string input)
{
    return input.Split($"{Environment.NewLine}{Environment.NewLine}") //Split to groups
                .Select(x => x.Split(Environment.NewLine) //Split to persons in each group
                              .Select(y => y.ToCharArray())) //Select responses each person
                .Select(x => (PersonCount: x.Count(), ResponsesYes: x.SelectMany(y => y))) //Select PersonCount and ResponsesYes in group
                .Select(x => x.ResponsesYes.GroupBy(g => g) //Group by ResponsesYes in group
                                           .Where(w => w.Count() == x.PersonCount) //Get ResponsesYes choosen by each person in group
                                           .Count()) //Count the number of these ResponsesYes
                .Sum(); 
}
