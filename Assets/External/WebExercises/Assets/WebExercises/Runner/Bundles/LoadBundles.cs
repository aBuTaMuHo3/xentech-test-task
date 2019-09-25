using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using MinLibs.Logging;
using MinLibs.MVC;
using MinLibs.Utils;
using UnityEngine.Networking;

namespace WebExercises.Runner
{
	public interface ILoadBundle
	{
		Task Execute(IEnumerable<BundleInfo> infos);
		Task Execute(BundleInfo info);
	}

	public class LoadBundle : ILoadBundle
	{
		[Inject] public ILogging log;
		[Inject] public IRunnerSettings settings;
		[Inject] public IBundles bundles;
		[Inject] public IScenes scenes;

		public async Task Execute(IEnumerable<BundleInfo> infos)
		{
			foreach (var info in infos)
			{
				await Execute(info);
			}
		}
		
		public async Task Execute(BundleInfo info)
		{
			var path = Path.Combine(settings.BasePath, info.Path);

			if (bundles.Has(info.Id))
			{
				return;
			}
			
			var request = UnityWebRequestAssetBundle.GetAssetBundle(path);
			
			//log.Trace("REQUEST " + path);
			
			await request.SendWebRequest();

			var hasError = request.isHttpError || request.isNetworkError;

			if (hasError)
			{
				log.Warn("no bundle at " + path);
				return;
			}

			var bundle = DownloadHandlerAssetBundle.GetContent(request);
			
			bundles.Store(info.Id, bundle);

			info.Scenes?.Each(name => scenes.Load(name));
		}
	}
}