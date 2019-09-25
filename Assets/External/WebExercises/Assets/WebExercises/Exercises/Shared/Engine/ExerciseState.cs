using System.Collections.Generic;
using System.Linq;

namespace WebExercises.Exercise
{
	public interface IExerciseState
	{
		bool IsFinished { get; }
		ExerciseExitCodes Result { get; }
		void Init(ExerciseOptions options);
		void CountRun(bool isGood);
		void HandleTimeOut(double timePassed);
		bool PreventInputPause { get; }
		float Timeout { get; }
		void Evaluate();
	}

	public class ExerciseState : IExerciseState
	{
		private readonly RunData totalRuns = new RunData();
		private readonly RunData goodRuns = new RunData();
		private readonly RunData consecutiveGoodRuns = new RunData();
		private readonly RunData badRuns = new RunData();
		private readonly RunData consecutiveBadRuns = new RunData();
		private readonly IList<bool> runHistory = new List<bool>();

		public void Init(ExerciseOptions options)
		{
			totalRuns.Max = options.MaxTotalRuns;
			goodRuns.Max = options.MaxTotalGoodRuns;
			consecutiveGoodRuns.Max = options.MaxConsecutiveGoodRuns;
			badRuns.Max = options.MaxTotalBadRuns;
			badRuns.Initial = options.MaxInitialBadRuns;
			badRuns.WithinLast = options.BadRunsWithinLast;
			badRuns.SectionedBy = options.BadRunsSectionedBy;
			consecutiveBadRuns.Max = options.MaxConsecutiveBadRuns;

			Timeout = options.Timeout * 1000;

			PreventInputPause = options.PreventInputPause;
		}
		
		public void CountRun(bool isGood)
		{
			runHistory.Insert(0, isGood);
			totalRuns.Increase();

			if (isGood)
			{
				goodRuns.Increase();
				consecutiveGoodRuns.Increase();
				consecutiveBadRuns.Reset();
			}
			else
			{
				badRuns.Increase();
				consecutiveBadRuns.Increase();
				consecutiveGoodRuns.Reset();
			}
		}

		public bool IsFinished => Result != ExerciseExitCodes.None;

		public ExerciseExitCodes Result { get; private set; }

		private bool CompletedTotalBadRuns
		{
			get
			{
				var withinLast = badRuns.WithinLast;

				if (withinLast == 0)
				{
					return badRuns.IsComplete;
				}

				var sectionedBy = badRuns.SectionedBy;

				return (sectionedBy == 0 || runHistory.Count % sectionedBy == 0)
				       && runHistory.Count >= withinLast
				       && badRuns.Max == runHistory.Take(withinLast).Count(r => !r);
			}
		}

		private bool CompletedInitialBadRuns
		{
			get
			{
				var initial = badRuns.Initial;

				return initial > 0 && initial == totalRuns.Current && initial == badRuns.Current;
			}
		}

		private bool isTimeUp;

		public void HandleTimeOut(double timePassed)
		{
			if (Timeout > 0)
			{
				isTimeUp = Timeout <= timePassed;
			}
		}

		public bool PreventInputPause { private set; get; }

		public float Timeout { get; private set; }

		public void Evaluate()
		{
			var result = ExerciseExitCodes.None;

			if (totalRuns.IsComplete)
			{
				result = ExerciseExitCodes.TotalRuns;
			}
			else if (goodRuns.IsComplete)
			{
				result = ExerciseExitCodes.GoodRuns;
			}
			else if (consecutiveGoodRuns.IsComplete)
			{
				result = ExerciseExitCodes.ConsecutiveGoodRuns;
			}
			else if (CompletedTotalBadRuns)
			{
				result = ExerciseExitCodes.BadRuns;
			}
			else if (CompletedInitialBadRuns)
			{
				result = ExerciseExitCodes.InitialBadRuns;
			}
			else if (consecutiveBadRuns.IsComplete)
			{
				result = ExerciseExitCodes.ConsecutiveBadRuns;
			}
			else if (isTimeUp)
			{
				result = ExerciseExitCodes.TimeIsUp;
			}

			Result = result;
		}
	}

	public enum ExerciseExitCodes
	{
		None = 0,
		TotalRuns,
		GoodRuns,
		ConsecutiveGoodRuns,
		BadRuns,
		InitialBadRuns,
		ConsecutiveBadRuns,
		TimeIsUp,
	}

	internal class RunData
	{
		public int Current { get; private set; }
		public int Best { get; private set; }
		public int Max { get; set; }
		public int Initial { get; set; }
		public int WithinLast { get; set; }
		public int SectionedBy { get; set; }
		public bool IsComplete => Max > 0 && Current == Max;

		public void Increase()
		{
			Current++;

			if (Current > Best)
			{
				Best = Current;
			}
		}

		public void Reset()
		{
			Current = 0;
			Initial = 0;
		}

		private float GetRatio(float value)
		{
			return Max > 0 ? value / Max : 0;
		}
	}
}