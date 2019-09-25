using System;
using System.Collections.Generic;
using MinLibs.Logging;
using MinLibs.MVC;
using MinLibs.Utils;
using WebExercises.Shared;

namespace WebExercises.Runner
{
	public interface IInitApp
	{
		void Execute(string text);
	}

	public class InitApp : IInitApp
	{
		[Inject] public IPlatform platform;
		[Inject] public ILogging log;
		[Inject] public IParser parser;
		[Inject] public IRunnerSettings settings;
		[Inject] public ILocalization localization;
		[Inject] public IFrontendSender sender;
		[Inject] public ILoadBundle loadBundle;

		public async void Execute(string text)
		{
            var info = parser.Deserialize<AppInfo>(text);
            if (info.EnableLogging == 0) {
//                log.Level = LoggingLevel.OFF;
            }
            log.Trace(">>>>>>>>> INIT APP");
            localization.Add(info.Localization);

            var parameters = UriExtentsions.GetParams(platform.URL);
            
            if (platform.IsEditor)
            {
                settings.ExerciseId = info.DefaultExercise;
                settings.Mode = settings.Mode ?? info.DefaultMode;
            }
            else if (!parameters.ContainsKey(RunnerConsts.IS_LOCAL))
            {
                settings.BasePath = info.BasePath;
            }
            
            platform.AntiAliasing = GetAA(parameters, info.AntiAliasing);
            platform.EnableLogging = info.EnableLogging;

            var startedAt = info.StartedAt;
            var startTime = DateTimeOffset.FromUnixTimeSeconds(startedAt);

            var span = DateTime.UtcNow - startTime;
            log.Info("+++++++++++++++++++++++++++++++++++++++++ Time to start the unity app: " + span);

            var bundles = info.Bundles;
            await loadBundle.Execute(bundles);

            log.Trace(">>>>>>>>> ON ENGINE READY");
            sender.OnEngineReady();
		}

		private static int GetAA(IDictionary<string, string> parameters, int aa)
		{
			var aaValue = parameters.Retrieve(RunnerConsts.AA, false);
			int value;
			
			if (!int.TryParse(aaValue, out value))
			{
				value = aa;
			}

			return value;
		}
	}
}