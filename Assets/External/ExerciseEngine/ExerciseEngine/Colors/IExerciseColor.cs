using System;
namespace ExerciseEngine.Colors
{
    public interface IExerciseColor<out T>
    {
        string Name { get; }

        string Hex { get; }

        T Color { get; }
    }
}
