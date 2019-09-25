namespace ExerciseEngine.Model.ValueObjects.Interfaces
{
    public interface IWorkoutHistogramVO
    {
        IWorkoutHistoramBinVO[] Bins { get; }
        float GetResultPerformance(int result, bool maxCapped = true);
        int GetResultNormalisedPerformance(int result);
        int GetBinResult(int binNumber);
        int GetResultFromPerformance(float performance);
        int ThresholdResult { get; }
        int MaxResult { get; }
    }
}