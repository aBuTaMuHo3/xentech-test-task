using System;
namespace ExerciseEngine.Model.ValueObjects.Interfaces
{
    public interface IBackgroundConfiguration
    {

        int[] BackgroundStartColor { get; set; }

        int[] BackgroundEndColor { get; set; }

        float BackgroundVectorAngle { get; set; }

    }
}
