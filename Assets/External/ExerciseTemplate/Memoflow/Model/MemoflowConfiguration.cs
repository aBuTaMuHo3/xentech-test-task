using ExerciseEngine.Model.Enum;
using ExerciseEngine.Model;
using Newtonsoft.Json;
using SynaptikonFramework.Util;

namespace Memoflow.Model
{
    public class MemoflowConfiguration : BaseExerciseConfiguration<MemoflowLevelSettings>
    {
        [JsonConverter(typeof(TolerantEnumConverter))]
        [JsonProperty(PropertyName = "colorPerDiff")]
        public AvailableColors[] ColorNamesPerDifficulty;

        [JsonProperty(PropertyName = "matchProbability")]
        public float MatchProbability;

        [JsonProperty(PropertyName = "maxColors")]
        public int MaxColors;

        [JsonProperty(PropertyName = "maxSymbols")]
        public int MaxSymbols;

        public MemoflowStimuliDefinition StimuliDefinition = MemoflowStimuliDefinition.SINGLE_SYMBOL;
    }

    public class MemoflowLevelSettings : BaseLevelSettings
    {
        [JsonProperty(PropertyName = "numCards")]
        public int NumCards;
    }
}
