
namespace Benchmarks.Tests
{
    public class LinqOrder
    {
        public List<(int[] numbers, int number)> OrderByOnly(List<(int[] numbers, int number)> list)
        => list.OrderBy(x => x.numbers[4])
               .OrderBy(x => x.numbers[3])
               .OrderBy(x => x.numbers[2])
               .OrderBy(x => x.numbers[1])
               .OrderBy(x => x.numbers[0])
               .OrderBy(x => x.number)
               .ToList();

        public List<(int[] numbers, int number)> OrderByAndThenBy(List<(int[] numbers, int number)> list)
        => list.OrderBy(x => x.number)
               .ThenBy(x => x.numbers[0])
               .ThenBy(x => x.numbers[1])
               .ThenBy(x => x.numbers[2])
               .ThenBy(x => x.numbers[3])
               .ThenBy(x => x.numbers[4])
               .ToList();

        public List<(int[] numbers, int number)> OrderByOnlyWithTuple(List<(int[] numbers, int number)> list)
        => list.OrderBy(x => (x.number, x.numbers[0], x.numbers[1], x.numbers[2], x.numbers[3], x.numbers[4]))
               .ToList();

        public List<(int[] numbers, int number)> OrderByAndThenByWithTuple(List<(int[] numbers, int number)> list)
        => list.OrderBy(x => x.number)
               .ThenBy(x => (x.numbers[0], x.numbers[1], x.numbers[2], x.numbers[3], x.numbers[4]))
               .ToList();
    }
}
