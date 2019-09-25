using ExerciseEngine.Model.Tutorial.Interfaces;

namespace ExerciseEngine.Model.ValueObjects.Interfaces
{
    public interface IExerciseConfiguration : IBackgroundConfiguration
    {
        int MaxDifficulty { get; }
		int MinDifficulty { get; }

        string ModelClass { get; }
        string ControllerClass { get; }
        string ViewClass { get; }
        string AssetClass { get; }
        string[] TerminatorClasses { get; }

        int GetTimeoutByLevel(int level);
        int GetMemorizeTimeoutByLevel(int level);

        int GetScoresByLevel(int level);
        int GetGoodRunsByLevel(int level);
        int GetBadRunsByLevel(int level);
        int GetWarmUpRoundsByLevel(int level);

        int NumberOfLevels { get; }
        int[][] MultiplierSteps { get; }

        ITutorialConfiguration[] TutorialConfigurations { get; }

        bool UsesDictionary { get; }
        
        bool EnforceTimeout { get; }
    }
}