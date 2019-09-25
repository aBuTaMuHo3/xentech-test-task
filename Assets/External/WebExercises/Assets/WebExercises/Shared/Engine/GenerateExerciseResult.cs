using System.Collections.Generic;
using ExerciseEngine.Model.ValueObjects;
using ExerciseEngine.Model.ValueObjects.Interfaces;
using MinLibs.MVC;
using SynaptikonFramework.Util;
using WebExercises.Exercise;

namespace WebExercises.Shared
{
	public interface IGenerateExerciseResult
	{
		IExerciseResult Execute(IExerciseResultVO vo);
	}
	
	public class GenerateExerciseResult : IGenerateExerciseResult
	{
		private const int CORRECT = 1;
		private const int WRONG = -1;
		private const int TIMEOUT = 0;
		
		[Inject] public IExerciseState state;
		
		public virtual IExerciseResult Execute(IExerciseResultVO vo)
		{
			var stats = new ExerciseStats
			{
				Score = vo.Score,
				Accuracy = vo.Accuracy,
				StartDifficulty = vo.StartDifficulty,
				EndDifficulty = vo.CurrentDifficulty,
				MaxDifficulty = vo.MaxDifficulty,
				MinDifficulty = vo.MinDifficulty,
				MeanDifficulty = vo.MeanDifficulty
			};

			var result = new ExerciseResult
			{
				Stats = stats,
				ExitCode = state.Result.ToString()
			};

			var history = vo.RawData as IList<FullRoundVO>;
			var index = 0;
			
			foreach (var fullRoundVo in history)
			{
				var actions = new List<PlayerAction>(); 

				foreach (var roundResult in fullRoundVo.RoundResult)
				{
					var action = new PlayerAction
					{
						UnixTimeStamp = (long)roundResult.TimeStampUTC.SinceDateZero().TotalMilliseconds,
						DeltaTime = (int)roundResult.ReactionTime.TotalMilliseconds,
						Answer = CalcAnswer(roundResult, index),
					};
					
					actions.Add(action);
				}
				
				result.Trials.Add(actions);

				index++;
			}

			return result;
		}

		private int CalcAnswer(IRoundEvaluationResultVO roundResult, int index)
		{
			if (roundResult.IsTimeOut)
			{
				return TIMEOUT;
			}

			return roundResult.AnswerCorrect ? CORRECT : GetErrorCode(index);
		}

		protected virtual int GetErrorCode(int index)
		{
			return WRONG;
		}
	}
}