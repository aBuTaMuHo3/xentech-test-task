using System.Collections.Generic;
using MinLibs.Utils;

namespace WebExercises.Shared
{
	public interface ILocalization
	{
		void Add(IDictionary<string, string> localization);
		string Get(string key, params object[] args);
	}
	
	public class Localization : ILocalization
	{
		private readonly IDictionary<string, string> source = new Dictionary<string, string>();

		public void Add(IDictionary<string, string> localization)
		{
			if (localization == null)
			{
				return;
			}
			
			foreach (var pair in localization)
			{
				source[pair.Key] = pair.Value;
			}
		}
		
		public string Get(string key, params object[] args)
		{
			var text = source.Retrieve(key, key);

			if (args.Length > 0)
			{
				text = string.Format(text, args);
			}

			return text;
		}
	}
}