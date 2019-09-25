using System.IO;
using System.Threading.Tasks;
using MinLibs.Logging;
using MinLibs.MVC;
using MinLibs.Utils;
using UnityEngine.Networking;

namespace WebExercises.Runner
{
	public interface IGetConfig
	{
		Task<T> Execute<T>(string path) where T: class;
	}

	public class GetConfig : IGetConfig
	{
		[Inject] public IRunnerSettings settings;
		[Inject] public ILogging logger;
		[Inject] public IParser parser;
		
		public async Task<T> Execute<T>(string path) where T: class
		{
			var json = await Load(path);

			if (typeof(T) == typeof(string))
			{
				return json as T;
			}

			return parser.Deserialize<T>(json);
		}

		private async Task<string> Load(string url)
		{
			url = Path.Combine(settings.BasePath, url);
			
			//logger.Trace("Requesting " + url);
			
			var result = string.Empty;
			
			using (var request = UnityWebRequest.Get(url))
			{
				await request.SendWebRequest();

				if (request.isNetworkError || request.isHttpError)
				{
					logger.Error(request.error);
				}
				else
				{
					result = request.downloadHandler.text;
				}
			}

			return result;
		}
	}
}