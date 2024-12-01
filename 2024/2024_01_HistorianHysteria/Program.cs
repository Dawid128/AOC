//[Transpose][Group][Dictionary]
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
var output2 = Part2(input);
stopwatch.Stop();
Console.WriteLine($"Output Part2: {output2}");
Console.WriteLine($"Done in Time: {stopwatch.ElapsedMilliseconds} ms");

object Part1(string input)
{
    //Take raw data from input file
    var data = input.Split(Environment.NewLine)         //Split input to lines
                    .Select(x => x.Split("   ")         //Split line to 2 values
                                  .Select(int.Parse)    //Convert value to integer
                                  .ToArray())           //Take data (2 values) as array
                    .ToList();                          //Take data (all lines) as array

    //Transpose, Take ready data to calculate
    var compareList = Enumerable.Range(0, 2)                            //Transpose 
                                .Select(i => data.Select(x => x[i])     //Transpose
                                                 .OrderBy(x => x)       //Sort
                                                 .ToList())             //Take as list
                                .ToArray();                             //Take as array

    //Calculate, take score
    return compareList[0].Select((x, index) => Math.Abs(compareList[1][index] - x))     //Calculate ABS for sorted integers (left and right)
                         .Sum();                                                        //Sum ABS scores
}

object Part2(string input)
{
    //Take raw data from input file
    var data = input.Split(Environment.NewLine)         //Split input to lines
                    .Select(x => x.Split("   ")         //Split line to 2 values
                                  .Select(int.Parse)    //Convert value to integer
                                  .ToArray())           //Take data (2 values) as array
                    .ToList();                          //Take data (all lines) as array

    //Transpose, Take ready data to calculate
    var compareList = Enumerable.Range(0, 2)                                                    //Transpose 
                                .Select(i => data.Select(x => x[i])                             //Transpose
                                                 .GroupBy(x => x)                               //Group by integer values
                                                 .ToDictionary(x => x.Key, x => x.Count()))     //Take as dictionary (integer value and count)
                                .ToArray();                                                     //Take as array

    //Calculate, take score
    return compareList[0].Select(x => compareList[1].TryGetValue(x.Key, out var value) ? value * x.Key : 0)     //Calculate score per element
                         .Sum();                                                                                //Sum scores
}