
namespace AdventCodeExtension
{
    public static class DictionaryExtensions
    {
        public static int IndexOfKey<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key)
        {
            ArgumentNullException.ThrowIfNull(dictionary);

            int index = 0;
            foreach (var kvp in dictionary)
            {
                if (EqualityComparer<TKey>.Default.Equals(kvp.Key, key))
                    return index;

                index++;
            }

            return -1;
        }

        public static void AddOrUpdate<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue value) where TKey : IEquatable<TKey>
        {
            if (dictionary.ContainsKey(key))
            {
                dictionary[key] = value;
                return;
            }

            dictionary.Add(key, value);
        }

        public static KeyValuePair<TKey, TValue> FirstAndRemove<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, Func<KeyValuePair<TKey, TValue>, bool> predicate)
        {
            var first = dictionary.First(predicate);
            dictionary.Remove(first.Key);
            return first;
        }
    }
}
