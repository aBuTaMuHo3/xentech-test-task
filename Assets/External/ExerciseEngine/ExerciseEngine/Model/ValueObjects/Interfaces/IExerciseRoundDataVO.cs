using System.Collections.Generic;
using ExerciseEngine.Settings.Enums;

namespace ExerciseEngine.Model.ValueObjects.Interfaces
{
    public interface IExerciseRoundDataVO
    {
        /** round items **/
        List<IRoundItem> Items { get; }
        /** items that will results on right, mostly only one **/
        List<IRoundItem> Solutions { get; }
        /** indicate if the level changed with the last round  **/
        LevelState LevelState { get; }
        /** timeout for the round **/
        int Timeout { get; set; }
        WarmUpState WarmUpState { get; }

        int Difficulty { get; set; }

        int MemorizeTimeout { get; set; }
    }
}