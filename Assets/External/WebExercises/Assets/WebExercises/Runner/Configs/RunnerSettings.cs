namespace WebExercises.Runner
{
	public interface IRunnerSettings
	{
		string BasePath { get; set; }
		string ExerciseId { get; set; }
		ExerciseInfo ExerciseInfo { get; set; }
		string Mode { get; set; }
		int Timeout { get; set; }
	}
	
	public class RunnerSettings : IRunnerSettings
	{
		public string BasePath { get; set; } = string.Empty;
		public string ExerciseId { get; set; }
		public ExerciseInfo ExerciseInfo { get; set; }
		public string Mode { get; set; }
		public int Timeout { get; set; }
	}
}