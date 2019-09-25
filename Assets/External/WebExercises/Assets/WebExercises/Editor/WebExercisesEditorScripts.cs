using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using WebExercises.Runner;

namespace WebExercises.Editor
{
	public static class WebExercisesEditorScripts
	{
		private static string startScene;
		private static string prevScene;

		private const string bundlesName = "Bundles";
		private const string configsDir = "Configs";
		private const string appDir = "Builds/WebGL/Current";
		private static readonly string debugDir = Path.Combine(appDir, "Debug");
		private static readonly string productionDir = Path.Combine(appDir, "Production");

		[MenuItem("Exercises/Switch Scenes &%g")]
		public static void SwitchScenes()
		{
			var currentScene = EditorSceneManager.GetActiveScene().path;
			EditorSceneManager.OpenScene(prevScene ?? StartScene, OpenSceneMode.Single);
			prevScene = currentScene;
		}

		private static string StartScene
		{
			get
			{
				startScene = startScene ?? EditorBuildSettings.scenes[0].path;

				return startScene;
			}
		}

		#region Editor

		//[MenuItem("Exercises/Publish for Editor &%e")]
		public static void BuildAndRunInEditor()
		{
			EditorSceneManager.SaveOpenScenes();

			UpdateAssetBundles(productionDir, BuildTarget.StandaloneOSX);
			UpdateConfigs(productionDir);
			//ExampleRegistrationsMenu.GenerateForEditor();
			EditorApplication.isPlaying = false;
			EditorSceneManager.OpenScene(StartScene, OpenSceneMode.Single);
			EditorApplication.isPlaying = true;
		}

		#endregion

		#region AssetBundles

		[MenuItem("Exercises/Update Debug Bundles")]
		public static void UpdateDebugBundles()
		{
			UpdateAssetBundles(debugDir, EditorUserBuildSettings.activeBuildTarget);
		}

		[MenuItem("Exercises/Update Production Bundles")]
		public static void UpdateProductionBundles()
		{
			UpdateAssetBundles(productionDir, EditorUserBuildSettings.activeBuildTarget);
		}

		private static void UpdateAssetBundles(string dir, BuildTarget target)
		{
			var bundlesDir = Path.Combine(dir, bundlesName);

			if (Directory.Exists(bundlesDir))
			{
				Directory.Delete(bundlesDir, true);
			}

			Directory.CreateDirectory(bundlesDir);

			BuildPipeline.BuildAssetBundles(bundlesDir,
				BuildAssetBundleOptions.ChunkBasedCompression,
				target);
		}

		#endregion

		//[MenuItem("Exercises/Debug Web &%w")]
		public static void DebugWeb()
		{
			Publish("Debug", debugDir);
		}

		//[MenuItem("Exercises/Profile Web &%p")]
		public static void ProfileWeb()
		{
			Publish("Profile", debugDir);
		}

		//[MenuItem("Exercises/Publish Web &%r")]
		public static void PublishWeb()
		{
			Publish("Production", productionDir);
		}

		private static void Publish(string name, string path)
		{
			var settings = SetPlayerSettings(name);
			UpdateAssetBundles(path, BuildTarget.WebGL);
			Export(path, settings);
			//UpdateAssetBundles(path, BuildTarget.WebGL);
			UpdateConfigs(path);
			OpenInBrowser(path);
		}

		private static PublishSettings SetPlayerSettings(string name)
		{
			var path = Path.Combine("BuildSettings", name);
			var settings = Resources.Load<PublishSettings>(path);
			PlayerSettings.WebGL.compressionFormat = settings.compressionFormat;
			PlayerSettings.WebGL.memorySize = settings.memorySize;
			PlayerSettings.WebGL.exceptionSupport = settings.exceptionSupport;
			PlayerSettings.WebGL.dataCaching = settings.dataCaching;
			PlayerSettings.WebGL.linkerTarget = settings.linkerTarget;
			PlayerSettings.WebGL.debugSymbols = settings.debugSymbols;
			PlayerSettings.SetManagedStrippingLevel(BuildTargetGroup.WebGL, settings.strippingLevel);

			return settings;
		}

		private static void UpdateConfigs(string targetDir)
		{
			targetDir = Path.Combine(targetDir, configsDir);
			CopyDirectory(configsDir, targetDir);
		}

		private static void Export(string dir, PublishSettings settings)
		{
			var buildOptions = BuildOptions.None;

			if (settings.development) buildOptions |= BuildOptions.Development;
			if (settings.allowDebugging) buildOptions |= BuildOptions.AllowDebugging;
			if (settings.connectWithProfiler) buildOptions |= BuildOptions.ConnectWithProfiler;
			if (settings.compressWithLz4) buildOptions |= BuildOptions.CompressWithLz4;

			var options = CreateBuildPlayerOptions(dir);
			options.options = buildOptions;

			BuildPipeline.BuildPlayer(options);
		}


		private static BuildPlayerOptions CreateBuildPlayerOptions(string baseDir)
		{
			var bundlesDir = Path.Combine(baseDir, bundlesName);
			var bundleManifest = Path.Combine(bundlesDir, bundlesName + ".manifest");

			var buildPlayerOptions = new BuildPlayerOptions
			{
				scenes = new[] {StartScene},
				locationPathName = baseDir,
				target = BuildTarget.WebGL,
				assetBundleManifestPath = bundleManifest
			};

			return buildPlayerOptions;
		}

		private static void OpenInBrowser(string dir)
		{
			var settings = Resources.Load<AppSettings>("AppSettings");
			var url = Path.Combine(settings.url, dir) + "?" + RunnerConsts.IS_LOCAL + "=1";
			Application.OpenURL(url);
		}

		private static void CopyDirectory(string sourceDirName, string destDirName, bool copySubDirs = true)
		{
			// Get the subdirectories for the specified directory.
			var dir = new DirectoryInfo(sourceDirName);

			if (!dir.Exists)
			{
				throw new DirectoryNotFoundException(
					"Source directory does not exist or could not be found: "
					+ sourceDirName);
			}

			var dirs = dir.GetDirectories();
			// If the destination directory doesn't exist, create it.

			if (Directory.Exists(destDirName))
			{
				Directory.Delete(destDirName, true);
			}

			Directory.CreateDirectory(destDirName);

			// Get the files in the directory and copy them to the new location.
			var files = dir.GetFiles();
			foreach (var file in files)
			{
				var tempPath = Path.Combine(destDirName, file.Name);
				file.CopyTo(tempPath, true);
			}

			// If copying subdirectories, copy them and their contents to new location.
			if (copySubDirs)
			{
				foreach (var subdir in dirs)
				{
					var tempPath = Path.Combine(destDirName, subdir.Name);
					CopyDirectory(subdir.FullName, tempPath, copySubDirs);
				}
			}
		}
	}
}