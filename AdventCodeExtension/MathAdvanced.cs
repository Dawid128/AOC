
namespace AdventCodeExtension
{
    public static class MathAdvanced
    {
        public static long CalculateLCM(IList<long> numbers)
        {
            var lcm = numbers[0];
            for (var i = 1; i < numbers.Count; i++)
                lcm = CalculateLCM(lcm, numbers[i]);

            return lcm;
        }

        public static long CalculateLCM(long a, long b) => Math.Abs(a * b) / CalculateGCD(a, b);

        public static long CalculateGCD(long a, long b)
        {
            while (b is not 0)
            {
                var temp = b;
                b = a % b;
                a = temp;
            }

            return a;
        }
    }
}
