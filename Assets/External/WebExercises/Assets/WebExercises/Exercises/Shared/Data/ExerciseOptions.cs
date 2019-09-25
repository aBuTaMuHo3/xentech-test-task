using Newtonsoft.Json;

namespace WebExercises.Exercise
{	
	public class TimeoutTestOptions
	{
		[JsonProperty(PropertyName = "timeout")]
		public int Timeout;
	}

	public class ExerciseOptions
	{
		[JsonProperty(PropertyName = "maxTotalRuns")]
		public int MaxTotalRuns;

		[JsonProperty(PropertyName = "maxTotalGoodRuns")]
		public int MaxTotalGoodRuns;
		
		[JsonProperty(PropertyName = "maxConsecutiveGoodRuns")]
		public int MaxConsecutiveGoodRuns;

		[JsonProperty(PropertyName = "maxTotalBadRuns")]
		public int MaxTotalBadRuns;

		[JsonProperty(PropertyName = "maxConsecutiveBadRuns")]
		public int MaxConsecutiveBadRuns;
		
		[JsonProperty(PropertyName = "maxInitialBadRuns")]
		public int MaxInitialBadRuns;

		[JsonProperty(PropertyName = "badRunsWithinLast")]
		public int BadRunsWithinLast;

		[JsonProperty(PropertyName = "badRunsSectionedBy")]
		public int BadRunsSectionedBy;
		
		[JsonProperty(PropertyName = "timeout")]
		public int Timeout;

		[JsonProperty(PropertyName = "preventInputPause")]
		public bool PreventInputPause;
	}
}