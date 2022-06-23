using System.Collections.Generic;

namespace WXRadio.WeatherManager.Extensions
{
    public static class DictionaryExtensions
    {
        public static TValue GetOrDefault<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue = default(TValue))
        {
            if (dictionary.ContainsKey(key))
            {
                return dictionary[key];
            }

            return defaultValue;
        }
    }
}
