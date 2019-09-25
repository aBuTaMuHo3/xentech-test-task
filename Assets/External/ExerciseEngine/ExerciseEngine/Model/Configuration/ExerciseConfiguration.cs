using System;
using System.Collections.Generic;
using ExerciseEngine.Model.Multiplier;
using ExerciseEngine.Model.Tutorial.Interfaces;
using ExerciseEngine.Model.ValueObjects.Interfaces;
using ExerciseEngine.Model.Tutorial;
using Newtonsoft.Json;
using SynaptikonFramework.Util;

namespace ExerciseEngine.Model
{
    public class BaseExerciseConfiguration<T> : BackgroundConfiguration, IExerciseConfiguration where T : ILevelSettings
    {
        [JsonProperty(PropertyName = "maxD")]
        public int MaxDifficulty { get; set; }

        [JsonProperty(PropertyName = "minD")]
        public int MinDifficulty { get; set; }

        [JsonProperty(PropertyName = "modelClassName")]
        public string ModelClass { get; set; }

        [JsonProperty(PropertyName = "controllerClassName")]
        public string ControllerClass { get; set; }

        [JsonProperty(PropertyName = "viewClassName")]
        public string ViewClass { get; set; }

        [JsonProperty(PropertyName = "assetClassName")]
        public string AssetClass { get; set; }

        [JsonProperty(PropertyName = "terminatorClassNames")]
        public string[] TerminatorClasses { get; set; }

        [JsonProperty(PropertyName = "tutorial"), JsonConverter(typeof(InterfaceArrayConverter<ITutorialConfiguration, TutorialConfiguration>))]
        public ITutorialConfiguration[] TutorialConfigurations { get; set; }

        [JsonProperty(PropertyName = "levels")]
        public T[] Levels { get; set; }
        
        [JsonProperty(PropertyName = "enforceTimeout")]
        public bool EnforceTimeout { get; set; }

        protected IMultiplier _multiplierConfig;

        public int NumberOfLevels
        {
            get
            {
                return Levels.Length;
            }
        }

        public int[][] MultiplierSteps
        {
            get
            {
                List<int[]> stepsIncreaseByLevel = new List<int[]>();
                foreach (T level in Levels)
                {
                    stepsIncreaseByLevel.Add(level.MultiplierSteps);
                }
                return stepsIncreaseByLevel.ToArray();
            }
        }

        public virtual bool UsesDictionary => false;

        public virtual int GetScoresByLevel(int level)
        {
            return Levels.Length > 0 ? Levels[level].Score : 1;
        }

        public int GetGoodRunsByLevel(int level)
        {
            return Levels.Length > 0 ? Levels[level].NumGoodRuns : 1;
        }

        public int GetBadRunsByLevel(int level)
        {
            return Levels.Length > 0 ? Levels[level].NumBadRuns : 1;
        }

        public virtual int GetTimeoutByLevel(int level)
        {
            return Levels.Length > 0 ? Levels[level].TimeoutMilliseconds : -1;
        }

        public virtual int GetMemorizeTimeoutByLevel(int level)
        {
            return 0;
        }

        public virtual int GetWarmUpRoundsByLevel(int level)
        {
            return Levels.Length > 0 ? Levels[level].WarmUpRounds : 0;
        }
    }

    public interface ILevelSettings
    {
        int NumGoodRuns { get; }
        int NumBadRuns { get; }
        int Score { get; }
        float Timeout { get; }
        int TimeoutMilliseconds { get; }
        int[] MultiplierSteps { get; }
        int WarmUpRounds { get; }
    }

    public class BaseLevelSettings : ILevelSettings
    {
        #region ILevelSettings implementation

        [JsonProperty(PropertyName = "goodRuns")]
        public int NumGoodRuns { get; set; }

        [JsonProperty(PropertyName = "badRuns")]
        public int NumBadRuns { get; set; }

        [JsonProperty(PropertyName = "score")]
        public int Score { get; set; }

        [JsonProperty(PropertyName = "timeout")]
        public float Timeout { get; set; }
        public int TimeoutMilliseconds
        {
            get
            {
                return System.Math.Abs(Timeout - -1) < float.Epsilon ? -1 : (int)(Timeout * 1000);
            }
        }

        [JsonProperty(PropertyName = "multiplierSteps")]
        public int[] MultiplierSteps { get; set; }

        [JsonProperty(PropertyName = "warmUpRounds")]
        public int WarmUpRounds { get; set; }


        #endregion

        public BaseLevelSettings()
        {
            NumGoodRuns = 1;
            NumBadRuns = 1;
            Score = 1;
            Timeout = -1;
            MultiplierSteps = new int[] { };
        }
    }

    public struct Range
    {

        [JsonProperty(PropertyName = "min")]
        public int Min { get; set; }

        [JsonProperty(PropertyName = "max")]
        public int Max { get; set; }
    }

    public struct RangeFloat
    {

        [JsonProperty(PropertyName = "min")]
        public float Min { get; set; }

        [JsonProperty(PropertyName = "max")]
        public float Max { get; set; }

        public float GetRandomValue(Random random)
        {   
            return (float)(random.NextDouble() * (Max - Min) + Min);
        }
    }

    public class BackgroundConfiguration : IBackgroundConfiguration
    {
        public BackgroundConfiguration()
        {
            BackgroundStartColor = new int[] { 143, 221, 224 };
            BackgroundEndColor = new int[] { 74, 140, 214 };
            BackgroundVectorAngle = 225;
        }

        [JsonProperty(PropertyName = "backgroundStartColour")]
        public int[] BackgroundStartColor { get; set; }

        [JsonProperty(PropertyName = "backgroundEndColour")]
        public int[] BackgroundEndColor { get; set; }

        [JsonProperty(PropertyName = "backgroundVectorAngle")]
        public float BackgroundVectorAngle { get; set; }
    }
}