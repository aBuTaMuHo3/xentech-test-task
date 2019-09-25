using Newtonsoft.Json;

namespace WebExercises.Exercise
{
	public class ExerciseDetails
	{
		[JsonProperty(PropertyName = "difficulty")]
		public int Difficulty;
		
		[JsonProperty(PropertyName = "mode")]
		public string Mode;
		
		[JsonProperty(PropertyName = "options")]
		public string Options;
	}
}