namespace MinLibs.Utils
{
	public interface IParser
	{
		T Deserialize<T>(string text);
		string Serialize(object obj, bool isPretty = false);
	}
}