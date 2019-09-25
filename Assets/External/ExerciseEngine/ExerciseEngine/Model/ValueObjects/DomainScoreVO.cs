
using System;
using ExerciseEngine.Model.ValueObjects.Interfaces;

namespace ExerciseEngine.Model.ValueObjects
{
    public class DomainScoreVO : IDomainScoreVO
    {
        public DomainScoreVO(int domainId, int cycleId, int score, long timestamp)
        {
            DomainId = domainId;
            Score = score;
            Timestamp = timestamp;
            CycleId = cycleId;
        }

        public int DomainId { get; }
        public int Score { get; }
        public long Timestamp { get; }
        public int CycleId { get; }

        public float PercentageScore
        {
            get
            {
                return (float)Score / (float)ExerciseEngineConstants.NORMALIZED_MAX_SCORE;
            }
        }

        public int GetComplementoryScore(float coeficient)
        {
            return (int)Math.Pow(ExerciseEngineConstants.NORMALIZED_MAX_SCORE - Score, coeficient);
        }
    }
}