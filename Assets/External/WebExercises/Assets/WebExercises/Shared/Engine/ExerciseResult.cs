using System.Collections.Generic;
using Newtonsoft.Json;

namespace WebExercises.Shared
{
	public interface IExerciseResult
	{
		string ExitCode { get; set; }
		IExerciseStats Stats { get; set; }
		List<List<PlayerAction>> Trials { get; }
	}

	public class ExerciseResult : IExerciseResult
	{
		[JsonProperty(PropertyName = "exitCode")]
		public string ExitCode { get; set; }

		[JsonProperty(PropertyName = "stats")]
		public IExerciseStats Stats { get; set; }
		
		[JsonProperty(PropertyName = "trials")]
		public List<List<PlayerAction>> Trials { get; } = new List<List<PlayerAction>>();
	}

	public interface IExerciseStats
	{
		float Score { get; set; }
		float Accuracy { get; set; }
		int MinDifficulty { get; set; }
		int MaxDifficulty { get; set; }
		int StartDifficulty { get; set; }
		int EndDifficulty { get; set; }
		float MeanDifficulty { get; set; }
	}

	public class ExerciseStats : IExerciseStats
	{
		[JsonProperty(PropertyName = "score")]
		public float Score { get; set; }

		[JsonProperty(PropertyName = "accuracy")]
		public float Accuracy { get; set; }
		
		[JsonProperty(PropertyName = "minDifficulty")]
		public int MinDifficulty { get; set; }
		
		[JsonProperty(PropertyName = "maxDifficulty")]
		public int MaxDifficulty { get; set; }
		
		[JsonProperty(PropertyName = "startDifficulty")]
		public int StartDifficulty { get; set; }
		
		[JsonProperty(PropertyName = "endDifficulty")]
		public int EndDifficulty { get; set; }
		
		[JsonProperty(PropertyName = "meanDifficulty")]
		public float MeanDifficulty { get; set; }
	}

	public class PlayerAction
	{
		[JsonProperty(PropertyName = "timestamp")]
		public long UnixTimeStamp { get; set; }
		
		[JsonProperty(PropertyName = "reactionTime")]
		public int DeltaTime { get; set; }
		
		[JsonProperty(PropertyName = "answer")]
		public int Answer { get; set; }
	}
}