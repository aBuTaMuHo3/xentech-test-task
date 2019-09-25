using Newtonsoft.Json;

namespace WebExercises.Memoflow
{
	public class MemoflowTestOptions
	{
		[JsonProperty(PropertyName = "totalRuns")]
		public int TotalRuns;
		
		[JsonProperty(PropertyName = "maxBadRuns")]
		public int MaxBadRuns;
		
		[JsonProperty(PropertyName = "withinLast")]
		public int WithinLast;

		[JsonProperty(PropertyName = "consecutiveBadRuns")]
		public int ConsecutiveBadRuns;
	}
}