using System;
using System.Collections.Generic;
using ExerciseEngine.Model.ValueObjects.Interfaces;

namespace ExerciseEngine.Model.ValueObjects
{
    public class FullRoundVO
    {
        public FullRoundVO(IExerciseRoundDataVO roundDataVO)
        {
            this.RoundDataVO = roundDataVO;
            this.RoundResult = new List<IRoundEvaluationResultVO>();
        }

        public IExerciseRoundDataVO RoundDataVO { get; }

        public List<IRoundEvaluationResultVO> RoundResult { get; }
    }
}
