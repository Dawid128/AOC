//[Zip]
using AdventCodeExtension;
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
    //Calculate, take score
    return input.Split(Environment.NewLine)                                                     //Split input to lines
                .Select(x => x.Split(" ")                                                       //Split line to values
                              .Select(int.Parse)                                                //Convert values to integers
                              .ToArray())                                                       //Take as array
                .Where(x => x.Zip(x.Skip(1), (a, b) => Math.Abs(Math.Abs(a - b) - 2) <= 1)      //Take where, adjacent elements are different between 1 and 3
                             .All(x => x))                                                      //Check if all adjacent elements are different between 1 and 3
                .Where(x => x.Zip(x.Skip(1), (a, b) => a > b)                                   //Take where, adjacent elements are ascending
                             .All(x => x) ||                                                    //Check if all adjacent elements are ascending
                            x.Zip(x.Skip(1), (a, b) => a < b)                                   //Take where, adjacent elements are descending
                             .All(x => x))                                                      //Check if all adjacent elements are descending
                .Count();                                                                       //Count all valid lines
}

object Part2(string input, int maxLength)
{
    var data = input.Split(Environment.NewLine)         //Split input to lines
                    .Select(x => x.Split(" ")           //Split line to values
                                  .Select(int.Parse)    //Convert values to integers
                                  .ToList())            //Take as list
                    .ToList();                          //Take as list

    //Calculate, take score
    var temp = new HashSet<int>();          //HashSet -> list for unique valid indexes (lines)
    for (var i = 0; i < maxLength; i++)     //Delete one element, for each combinations
    {
        var dataTemp = data.Select((x, index) => (Index: index, Value: x.ToList()))                                     //Take as tuple, Index and values
                           .Where(x => x.Value.Count > i)                                                               //Only for list with enough length
                           .ForEach(x => x.Value.RemoveAt(i))                                                           //Delete one element in list - i
                           .Where(x => x.Value.Zip(x.Value.Skip(1), (a, b) => Math.Abs(Math.Abs(a - b) - 2) <= 1)       //Take where, adjacent elements are different between 1 and 3
                                              .All(x => x))                                                             //Check if all adjacent elements are different between 1 and 3
                           .Where(x => x.Value.Zip(x.Value.Skip(1), (a, b) => a > b)                                    //Take where, adjacent elements are ascending
                                              .All(x => x) ||                                                           //Check if all adjacent elements are ascending
                                       x.Value.Zip(x.Value.Skip(1), (a, b) => a < b)                                    //Take where, adjacent elements are descending
                                              .All(x => x))                                                             //Check if all adjacent elements are descending
                           .Select(x => x.Index)                                                                        //Take only indexes
                           .ToList();                                                                                   //Take as list

        temp.AddRange(dataTemp);
    }
    return temp.Count;
} 