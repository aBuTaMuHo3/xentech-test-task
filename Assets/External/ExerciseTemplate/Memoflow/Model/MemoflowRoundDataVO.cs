using System.Collections.Generic;
using ExerciseEngine.Model.ValueObjects;
using ExerciseEngine.Model.ValueObjects.Interfaces;
using ExerciseEngine.Settings.Enums;

namespace Memoflow.Model
{
    public class MemoflowRoundDataVO : BaseRoundDataVO
    {
        public int NumCards { get; }
        public MemoflowMatchType[] MatchTypes { get; }
        public string TutorialTextId { get; }

        public MemoflowRoundDataVO(List<IRoundItem> items, List<IRoundItem> solutions, LevelState levelState, WarmUpState warmUpState, int timeout, int numCards, MemoflowMatchType[] matchTypes, string tutorialTextId) : base(items, solutions, levelState, warmUpState, timeout)
        {
            NumCards = numCards;
            MatchTypes = matchTypes;
            TutorialTextId = tutorialTextId;
        }
    }
}
