using System;
using System.Collections.Generic;
using ExerciseEngine.Model.ValueObjects.Interfaces;

namespace ExerciseEngine.Model.ValueObjects
{
    public class RoundEvaluationResultVO : ExerciseModelUpdateVO, IRoundEvaluationResultVO
    {
        public bool AnswerCorrect { get; }
        public IAnswerVO GivenAnswer { get; }
        public List<IRoundItem> Solutions { get; }
        public int CurrentStep { get; }
        public bool RoundCompleted { get; }
        public bool IsTimeOut { get; }
        public TimeSpan ReactionTime { get; }
        public DateTime TimeStampUTC { get; }

        public RoundEvaluationResultVO(
            bool answerCorrect,
            IAnswerVO givenAnswer,
            List<IRoundItem> solutions,
            List<IModelPropertyUpdateVO> updates,
            bool isTimeOut,
            DateTime startReactionDateTime,
            int currentStep = 1,
            bool roundCompleted = true):base(updates)
        {
            IsTimeOut = isTimeOut;
            RoundCompleted = roundCompleted;
            CurrentStep = currentStep;
            Solutions = solutions;
            GivenAnswer = givenAnswer;
            AnswerCorrect = answerCorrect;
            ReactionTime = DateTime.Now - startReactionDateTime;
            TimeStampUTC = DateTime.UtcNow;
        }

    }
}