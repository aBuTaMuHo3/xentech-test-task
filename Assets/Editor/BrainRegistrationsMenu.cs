using MinLibs.MVC;
using UnityEditor;
using WebExercises.FlashGlance.Editor;
using WebExercises.HUD;
using WebExercises.Memoflow.Editor;
using WebExercises.MinHUD;
using WebExercises.Runner.Editor;

namespace WebExercises.Editor
{
	public static class BrainRegistrationsMenu
	{
		[MenuItem("Exercises/Registrations/Generate for Editor", false, 1)]
		public static void GenerateForEditor()
		{
			var envState = new EnvironmentState
			{
				editor = EditorFlags.IsEditor
			};

			Generate(envState);
		}

		[MenuItem("Exercises/Registrations/Generate for Web", false, 1)]
		public static void GenerateForWeb()
		{
			var envState = new EnvironmentState
			{
				editor = EditorFlags.IsNotEditor
			};

			Generate(envState);
		}

		[MenuItem("Exercises/Publish for Editor &%e")]
		public static void BuildAndRunInEditor()
		{
			GenerateForEditor();
			WebExercisesEditorScripts.BuildAndRunInEditor();
		}
		
		[MenuItem("Exercises/Debug Web &%w")]
		public static void DebugWeb()
		{
			GenerateForWeb();
			WebExercisesEditorScripts.DebugWeb();
		}

		[MenuItem("Exercises/Profile Web &%p")]
		public static void ProfileWeb()
		{
			GenerateForWeb();
			WebExercisesEditorScripts.ProfileWeb();
		}

		[MenuItem("Exercises/Publish Web &%r")]
		public static void PublishWeb()
		{
			GenerateForWeb();
			WebExercisesEditorScripts.PublishWeb();
		}

		private static void Generate(EnvironmentState envState)
		{
			Generate<RunnerRegistrations>(envState);
			Generate<MinHUDRegistrations>(envState);
			Generate<HUDRegistrations>(envState);
			Generate<DialogueRegistrations>(envState);
			Generate<MemoflowRegistrations>(envState);
			Generate<FlashGlanceRegistrations>(envState);
		}

		private static void Generate<T>(EnvironmentState envState) where T: class, IRegistrations, new()
		{
			ModularRegistrationsGenerator.GenerateRegistrations<T>(envState);
		}
	}
}