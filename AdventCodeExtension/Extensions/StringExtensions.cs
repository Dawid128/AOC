
namespace AdventCodeExtension
{
    public static class StringExtensions
    {
        public static string ReplaceFirst(this string original, string search, string replacement)
        {
            int index = original.IndexOf(search);
            if (index == -1)
                return original;

            return string.Concat(original.AsSpan(0, index), replacement, original.AsSpan(index + search.Length));
        }

        public static string ReplaceLast(this string original, string search, string replacement)
        {
            int index = original.LastIndexOf(search);
            if (index == -1)
                return original;

            return string.Concat(original.AsSpan(0, index), replacement, original.AsSpan(index + search.Length));
        }
    }
}
