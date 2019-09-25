using System.Collections.Generic;
using System.Linq;
using ExerciseEngine.Model.ValueObjects;
using ExerciseEngine.Model.ValueObjects.Interfaces;
using ExerciseEngine.Settings.Enums;

namespace FlashGlance.Model.ValueObjects
{
    public class FlashGlanceRoundDataVO : BaseRoundDataVO
    {
        // Items contain all the elemnts that are shown in the selection part of the exercise, 
        // they are the possible answers and are updated (almost) every round
        // Use them to update the view in CreateRound
        public FlashGlanceRoundDataVO(List<FlashGlanceRoundItemVO> items,
                                      List<IRoundItem> solutions,
                                      LevelState levelState, 
                                      WarmUpState warmUpState, 
                                      int timeout,
                                      List<FlashGlanceRoundItemVO> questQueue,
                                      int questIndex,
                                     int numberOfRows,
                                      int numberOfColumns) : base(items.Cast<IRoundItem>().ToList(), solutions, levelState, warmUpState, timeout)
        {
            QuestQueue = questQueue;
            QuestIndex = questIndex;
            NumberOfRows = numberOfRows;
            NumberOfColumns = numberOfColumns;
        }

        // This holds the order of asked numbers, to be used by the slider component.
        // It is not reset so we need to keep track of the current index of the asked item
        // USe them to update the slider in CreateRound
        public List<FlashGlanceRoundItemVO> QuestQueue { get; }
        
        // The index of the current searched item
        public int QuestIndex { get; }
        
        // Grid width, here its always 9 elements
        public int NumberOfRows { get; }
       
        // Grid height, here its always 4 elements
        public int NumberOfColumns { get; }

    }
}