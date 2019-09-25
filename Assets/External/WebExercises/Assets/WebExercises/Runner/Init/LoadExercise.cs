using System.Collections.Generic;
using System.Threading.Tasks;
using MinLibs.Logging;
using MinLibs.MVC;
using MinLibs.Utils;
using WebExercises.Shared;

namespace WebExercises.Runner
{
	public interface ILoadExercise
	{
		void Execute(string text);
	}

	public class LoadExercise : ILoadExercise
	{
		[Inject] public IContext context;
		[Inject] public ILogging log;
		[Inject] public IParser parser;
		[Inject] public IRunnerSettings settings;
		[Inject] public ILocalization localization;
		[Inject] public ILoadBundle loadBundle;
		[Inject] public IFrontendSender sender;
		[Inject] public IScenes scenes;

		public async void Execute(string text)
		{
			log.Trace(">>>>>>>>> LOAD EXERCISE");
			var info = parser.Deserialize<ExerciseInfo>(text);
			settings.ExerciseId = info.Id;
			settings.ExerciseInfo = info;
			
			localization.Add(info.Localization);

			await loadBundle.Execute(info.Bundles);
			await InitContexts(info.Contexts, context);

			log.Trace(">>>>>>>>> ON EXERCISE READY");
			sender.OnExerciseReady();
		}

		private async Task InitContexts(IEnumerable<ContextInfo> contextInfos, IContext parentContext)
		{
			foreach (var contextInfo in contextInfos)
			{
				var scene = await scenes.Load(contextInfo.Id);
				var roots = scene.GetRootGameObjects();

				foreach (var root in roots)
				{
					var rootComp = root.GetComponent<IContextInitializer>();

					if (rootComp != null)
					{
						var childContext = rootComp.InitContext(contextInfo.Id, parentContext);
						
						if (contextInfo.Contexts != null)
						{
							await InitContexts(contextInfo.Contexts, childContext);
						}
					}
				}
			}
		}
	}
}