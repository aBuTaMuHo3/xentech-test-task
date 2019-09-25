using System;
namespace ExerciseEngine.Model.ValueObjects.Interfaces
{
    public interface IDomainScoreVO
    {
        int DomainId { get; }
        int CycleId { get; }
        int Score { get; }
        float PercentageScore { get; }
        int GetComplementoryScore(float coeficient);
        long Timestamp { get; }
    }
}
