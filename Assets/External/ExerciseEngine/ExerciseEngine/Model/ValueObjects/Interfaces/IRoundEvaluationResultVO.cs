using System;
using System.Collections.Generic;

namespace ExerciseEngine.Model.ValueObjects.Interfaces
{
    public interface IRoundEvaluationResultVO: IExerciseModelUpdateVO
    {
        bool AnswerCorrect { get; }
        IAnswerVO GivenAnswer { get; }
        List<IRoundItem> Solutions { get; }
        bool IsTimeOut { get; }
        bool RoundCompleted { get; }
        int CurrentStep { get; }
        TimeSpan ReactionTime { get; }
        DateTime TimeStampUTC { get; }
    }
}