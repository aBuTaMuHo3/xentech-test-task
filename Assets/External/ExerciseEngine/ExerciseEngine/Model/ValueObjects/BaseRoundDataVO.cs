using System.Collections.Generic;
using System.Linq;
using ExerciseEngine.Model.ValueObjects.Interfaces;
using ExerciseEngine.Settings.Enums;

namespace ExerciseEngine.Model.ValueObjects
{
    public class BaseRoundDataVO : IExerciseRoundDataVO
    {
        public List<IRoundItem> Items { get; }
        public List<IRoundItem> Solutions { get; }
        public LevelState LevelState { get; }
        public int Timeout { get; set; }
        public WarmUpState WarmUpState { get; }
        public int Difficulty { get; set; }
        public int MemorizeTimeout { get; set; }

        public BaseRoundDataVO(List<IRoundItem> items, List<IRoundItem> solutions, LevelState levelState, WarmUpState warmUpState, int timeout)
        {
            WarmUpState = warmUpState;
            Timeout = timeout;
            LevelState = levelState;
            Solutions = solutions;
            Items = items;
        }

        public BaseRoundDataVO(List<IRoundItem> items, List<IRoundItem> solutions, LevelState levelState, WarmUpState warmUpState, int timeout, int memorizeTimeout):this(items, solutions, levelState, warmUpState, timeout) {
            MemorizeTimeout = memorizeTimeout;
        }
 
        protected static List<IRoundItem> ToRoundItems<T>(IEnumerable<T> items) where T: IRoundItem
        {
            return items.Select( arg => arg as IRoundItem).ToList();
        }
    }
}