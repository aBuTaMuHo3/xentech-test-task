using MinLibs.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WebExercises.Shared
{
	public class JsonParser : IParser
	{
		public T Deserialize<T>(string text)
		{
			return JsonConvert.DeserializeObject<T>(text);
		}

		public string Serialize(object obj, bool isPretty = false)
		{
			var json = JsonConvert.SerializeObject(obj);

			if (isPretty)
			{
				json = JValue.Parse(json).ToString(Formatting.Indented);
			}

			return json;
		}
	}
}