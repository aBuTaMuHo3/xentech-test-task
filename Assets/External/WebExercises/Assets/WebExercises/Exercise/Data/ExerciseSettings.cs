namespace WebExercises.Exercise
{
	public interface IExerciseSettings
	{
		string Mode { get; set; }
		
		string Options { get; set; }
		
		int Difficulty { get; set; }
	}
	
	public class ExerciseSettings : IExerciseSettings
	{
		public string Mode { get; set; }
		public string Options { get; set; }
		public int Difficulty { get; set; }
	}
}