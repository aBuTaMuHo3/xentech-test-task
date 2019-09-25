using System;
using ExerciseEngine.Model;
using Newtonsoft.Json;

namespace FlashGlance.Model
{
    public class FlashGlanceConfiguration : BaseExerciseConfiguration<FlashGlanceLevelSettings>
    {
        public int GetStartingItemsByLevel(int level)
        {
            return Levels.Length > 0 ? Levels[level].StartingItems : 1;
        }

        public float GetRotationProbabilityByLevel(int level)
        {
            return Levels.Length > 0 ? Levels[level].RotationProbability : 0;
        }

        public float GetScalingProbabilityByLevel(int level)
        {
            return Levels.Length > 0 ? Levels[level].ScalingProbability : 0;
        }

        public FlashGlanceMaxMinVo GetStartingCyphersByLevel(int level)
        {
            return Levels.Length > 0 ? Levels[level].StartingCypher : new FlashGlanceMaxMinVo(){Max = 1, Min = 1};
        }

        public FlashGlanceMaxMinVo GetNumberOfItemsByLevel(int level)
        {
            return Levels.Length > 0 ? Levels[level].NumberOfItems : new FlashGlanceMaxMinVo() { Max = 1, Min = 1 };
        }

        public FlashGlanceEventVo GetNewItemtemSpawningByLevel(int level)
        {
            return Levels.Length > 0 ? Levels[level].NewItemtemSpawning : new FlashGlanceEventVo() { Amount = 0, MaxDelay = 0, MinDelay = 0};
        }

        public FlashGlanceEventVo GetItemMovingByLevel(int level)
        {
            return Levels.Length > 0 ? Levels[level].ItemMoving : new FlashGlanceEventVo() { Amount = 0, MaxDelay = 0, MinDelay = 0 };
        }

        public FlashGlanceEventVo GetItemSwitchingByLevel(int level)
        {
            return Levels.Length > 0 ? Levels[level].ItemSwitching : new FlashGlanceEventVo() { Amount = 0, MaxDelay = 0, MinDelay = 0 };
        }

        public string GetNextItemFormulaByLevel(int level)
        {
            return Levels.Length > 0 ? Levels[level].NextItemFormula : "x + 1";
        }

        [JsonProperty(PropertyName = "mapWidth")]
        public int MapWidth { get; set; }

        [JsonProperty(PropertyName = "mapHeight")]
        public int MapHeight { get; set; }
    }

    public class FlashGlanceLevelSettings : BaseLevelSettings
    {
        [JsonProperty(PropertyName = "startItems")]
        public int StartingItems { get; set; }

        [JsonProperty(PropertyName = "startCypher")]
        public FlashGlanceMaxMinVo StartingCypher { get; set; }

        [JsonProperty(PropertyName = "itemsOnRound")]
        public FlashGlanceMaxMinVo NumberOfItems { get; set; }

        [JsonProperty(PropertyName = "rotationProbability")]
        public float RotationProbability { get; set; }

        [JsonProperty(PropertyName = "scalingProbability")]
        public float ScalingProbability { get; set; }

        [JsonProperty(PropertyName = "itemSpawning")]
        public FlashGlanceEventVo NewItemtemSpawning { get; set; }

        [JsonProperty(PropertyName = "itemMoving")]
        public FlashGlanceEventVo ItemMoving { get; set; }

        [JsonProperty(PropertyName = "itemSwitching")]
        public FlashGlanceEventVo ItemSwitching { get; set; }

        [JsonProperty(PropertyName = "nextItemFormula")]
        public String NextItemFormula { get; set; }
    }

    public class FlashGlanceMaxMinVo
    {
        [JsonProperty(PropertyName = "max")]
        public int Max { get; set; }

        [JsonProperty(PropertyName = "min")]
        public int Min { get; set; }
    }

    public class FlashGlanceEventVo
    {
        [JsonProperty(PropertyName = "minDelay")]
        public float MinDelay { get; set; }

        [JsonProperty(PropertyName = "maxDelay")]
        public float MaxDelay { get; set; }

        [JsonProperty(PropertyName = "amount")]
        public int Amount { get; set; }
    }
}