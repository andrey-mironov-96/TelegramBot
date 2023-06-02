namespace app.business.Extensions
{
    public static class DictionaryExtensions
    {
        public static void RenameKey<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey fromKey, TKey toKey)
        {
            TValue value = dictionary[fromKey];
            dictionary.Remove(fromKey);
            dictionary[toKey] = value;
        }
    }
}