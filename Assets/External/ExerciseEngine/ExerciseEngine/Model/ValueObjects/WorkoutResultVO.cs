using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExerciseEngine.Goal;
using ExerciseEngine.Model.ValueObjects.Interfaces;
using SynaptikonFramework.Util;

namespace ExerciseEngine.Model.ValueObjects
{
    public class WorkoutResultVO : ExerciseResultVO, IWorkoutResultVO
    {

        public int Result { get; }

        public int ConfigVersion { get; }

        public string Variant { get; }

        public int TrainingId { get; }

        public int SessionId { get; }

        public int CycleId { get; private set; }

        public int WorkoutId { get; private set; }

        public long OnlineTimestamp { get; }

        public WorkoutResultVO(IExerciseResultVO exerciseResult, ISessionWorkoutVO workout)
        {
            Accuracy = exerciseResult.Accuracy;
            BadRuns = exerciseResult.BadRuns;
            BonusScore = exerciseResult.BonusScore;
            CurrentDifficulty = exerciseResult.CurrentDifficulty;
            FinishedTimestamp = exerciseResult.FinishedTimestamp;
            FirstErrorDifficulty = exerciseResult.FirstErrorDifficulty;

            var rawData = exerciseResult.RawData as System.Collections.Generic.IList<ExerciseEngine.Model.ValueObjects.FullRoundVO>;
            _history = rawData;
            GameHistory = exerciseResult.GameHistory;

            GoodRuns = exerciseResult.GoodRuns;
            MaxDifficulty = exerciseResult.MaxDifficulty;
            MeanDifficulty = exerciseResult.MeanDifficulty;
            MinDifficulty = exerciseResult.MinDifficulty;
            ReactionTimeAverage = exerciseResult.ReactionTimeAverage;
            ReactionTimeBadRunAverage = exerciseResult.ReactionTimeBadRunAverage;
            ReactionTimeGoodRunAverage = exerciseResult.ReactionTimeGoodRunAverage;
            Score = exerciseResult.Score;
            StartDifficulty = exerciseResult.StartDifficulty;

            CycleId = workout.CycleId;
            WorkoutId = workout.WorkoutId;
            SessionId = workout.SessionId;
            TrainingId = workout.TrainingId;
            Variant = workout.WorkoutConfiguration.Variant;
            ConfigVersion = workout.WorkoutConfiguration.ConfigVersion;

            OnlineTimestamp = 0;

            // Update Result base on goal type
            switch (workout.WorkoutConfiguration.GoalType)
            {
                case GoalType.Score:
                    Result = exerciseResult.FinalScore;
                    break;
                case GoalType.GoodRuns:
                    Result = exerciseResult.GoodRuns;
                    break;
            }
        }

        public WorkoutResultVO(int workoutId, int result, long finishedTimestamp, string gameHistory, int currentDifficulty, int startDifficulty, float meanDifficutly, int maxDifficulty, int minDifficulty, int reactionTimeGoodRunAverage, 
                               int reactionTimeBadRunAverage, int configVersion = 0, string variant = "", int traingingId = 0, int sessionId = 0, int cycleId = 0, long onlineTimestamp = 0, int firstErrorDifficulty = -1, int bonusScore = 0, int score = 0) 
        {
            Result = result;
            FinishedTimestamp = finishedTimestamp;
            var computedHistory = gameHistory.DecryptZeroOne().ToCharArray();
            GoodRuns = computedHistory.Count(x => x == '0');
            BadRuns = computedHistory.Count(x => x == '1');
            CurrentDifficulty = currentDifficulty;
            StartDifficulty = startDifficulty;
            MeanDifficulty = meanDifficutly;
            MaxDifficulty = maxDifficulty;
            MinDifficulty = minDifficulty;
            ReactionTimeGoodRunAverage = reactionTimeGoodRunAverage;
            ReactionTimeBadRunAverage = reactionTimeBadRunAverage;
            GameHistory = gameHistory;
            ConfigVersion = configVersion;
            Variant = variant;
            TrainingId = traingingId;
            SessionId = sessionId;
            CycleId = cycleId;
            WorkoutId = workoutId;
            OnlineTimestamp = onlineTimestamp;
            Accuracy = (float)GoodRuns / (float)(GoodRuns + BadRuns);
            try
            {
                ReactionTimeAverage = (ReactionTimeGoodRunAverage * GoodRuns + ReactionTimeBadRunAverage * BadRuns) / (GoodRuns + BadRuns);
            }
            catch
            {
                ReactionTimeAverage = 0;
            }

            // currently not stored properly
            _history = new List<FullRoundVO>();
            FirstErrorDifficulty = firstErrorDifficulty;
            BonusScore = bonusScore;
            Score = score;
        }

        public override string ToString()
        {
            if(RawData != null)
                return Newtonsoft.Json.JsonConvert.SerializeObject(RawData);
            return Newtonsoft.Json.JsonConvert.SerializeObject(new List<FullRoundVO>());
        }

        public void UpdateCycleId(int newCycleId)
        {
            this.CycleId = newCycleId;
        }
    }
}