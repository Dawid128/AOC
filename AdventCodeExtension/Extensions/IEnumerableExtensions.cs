
namespace AdventCodeExtension
{
    public static class IEnumerableExtensions
    {
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            ArgumentNullException.ThrowIfNull(source);
            ArgumentNullException.ThrowIfNull(action);

            foreach (T item in source)
            {
                action(item);
                yield return item;
            }
        }

        public static IEnumerable<int> ElementsAt(this IEnumerable<int> input, IList<int> indexes)
        {
            foreach (var index in indexes)
                yield return input.ElementAt(index);
        }

        public static long Product(this IEnumerable<int> input)
        {
            var result = (long)input.First();
            foreach (var item in input.Skip(1))
                result *= item;

            return result;
        }
    }
}
