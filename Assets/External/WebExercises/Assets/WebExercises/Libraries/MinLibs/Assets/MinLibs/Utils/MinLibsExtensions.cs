using System;
using System.Collections.Generic;

namespace MinLibs.Utils
{
	public static class MinLibsExtensions
	{
		public static void Each<T> (this IEnumerable<T> enumerable, Action<T> handler)
		{
			foreach (var item in enumerable) {
				handler(item);
			}
		}

		public static void Times(this int number, Action<int> handler, int offset = 0, int step = 1)
		{
			for (var i = offset; i < number; i += step)
			{
				handler(i);
			}
		}

		public static TValue Retrieve<TKey, TValue> (this IDictionary<TKey, TValue> dictionary, TKey key, bool useCreator = true)
		{
			Func<TValue> creator = null;

			if (useCreator) {
				creator = Activator.CreateInstance<TValue>;
			}

			return Retrieve(dictionary, key, creator);
		}

		public static TValue Retrieve<TKey, TValue> (this IDictionary<TKey, TValue> dictionary, TKey key, TValue defValue, bool doStore = false)
		{
			TValue value;

			if (!dictionary.TryGetValue(key, out value)) {
				value = defValue;

				if (doStore) {
					dictionary[key] = value;
				}
			}

			return value;
		}

		public static TValue Retrieve<TKey, TValue> (this IDictionary<TKey, TValue> dictionary, TKey key, Func<TValue> creator)
		{
			TValue value;

			if (!dictionary.TryGetValue(key, out value)) {
				if (creator != null) {
					dictionary[key] = value = creator();
				}
			}

			return value;
		}

		public static bool AddNewEntry<TKey, TValue> (this IDictionary<TKey, TValue> dictionary, TKey key, TValue value)
		{
			var isKeyNew = !dictionary.ContainsKey(key);

			if (isKeyNew) {
				dictionary[key] = value;
			}

			return isKeyNew;
		}

		public static bool UpdateEntry<TKey, TValue> (this IDictionary<TKey, TValue> dictionary, TKey key, TValue value)
		{
			var isKeyExisting = dictionary.ContainsKey(key);

			if (isKeyExisting) {
				dictionary[key] = value;
			}

			return isKeyExisting;
		}

		public static TValue Withdraw<TKey, TValue> (this IDictionary<TKey, TValue> dictionary, TKey key)
		{
			TValue value;

			if (dictionary.TryGetValue(key, out value)) {
				dictionary.Remove(key);
			}

			return value;
		}

		public static T Pop<T> (this IList<T> list)
		{
			if (list.Count == 0)
			{
				return default(T);
			}
			
			var item = list[0];
			list.RemoveAt(0);

			return item;
		}
	}
}