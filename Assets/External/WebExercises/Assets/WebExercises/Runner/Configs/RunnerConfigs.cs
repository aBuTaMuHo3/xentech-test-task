using System.Collections.Generic;
using ExerciseEngine.Model.ValueObjects.Interfaces;
using Newtonsoft.Json;

namespace WebExercises.Runner
{
	public class AppInfo
	{
		[JsonProperty(PropertyName = "basePath")]
		public string BasePath;
		
		[JsonProperty(PropertyName = "localization")]
		public Dictionary<string,string> Localization;

		[JsonProperty(PropertyName = "defaultExercise")]
		public string DefaultExercise;

		[JsonProperty(PropertyName = "defaultMode")]
		public string DefaultMode;

		[JsonProperty(PropertyName = "antiAliasing")]
		public int AntiAliasing;

		[JsonProperty(PropertyName = "startedAt")]
		public long StartedAt;

		[JsonProperty(PropertyName = "bundles")]
		public BundleInfo[] Bundles;

        [JsonProperty(PropertyName = "enableLogging")]
        public int EnableLogging;
    }

	public class ContextInfo
	{
		[JsonProperty(PropertyName = "id")]
		public string Id;

		[JsonProperty(PropertyName = "contexts")]
		public ContextInfo[] Contexts;
	}

	public class ExerciseInfo : ContextInfo
	{
		[JsonProperty(PropertyName = "configs")]
		public Dictionary<string,string> Configs;
		
		[JsonProperty(PropertyName = "localization")]
		public Dictionary<string,string> Localization;

		[JsonProperty(PropertyName = "bundles")]
		public BundleInfo[] Bundles;
	}

	public class BundleInfo
	{
		[JsonProperty(PropertyName = "id")]
		public string Id;

		[JsonProperty(PropertyName = "path")]
		public string Path;

		[JsonProperty(PropertyName = "cache")]
		public bool Cache;

		[JsonProperty(PropertyName = "scenes")]
		public string[] Scenes;
	}
	
	public class AppConfig<T> where T: IExerciseConfiguration
	{
		[JsonProperty(PropertyName = "exercise")]
		public ExerciseConfig<T> Exercise;
	}

	public class ExerciseConfig<T> where T: IExerciseConfiguration
	{
		[JsonProperty(PropertyName = "id")]
		public string Id;

		[JsonProperty(PropertyName = "settingsClassName")]
		public string SettingsClassName;

		[JsonProperty(PropertyName = "exerciseConfig")]
		public T Config;
	}
}