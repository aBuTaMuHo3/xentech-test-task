using ExerciseEngine.Model.Enum;

namespace ExerciseEngine.Colors {
    public interface IColorManager<out T> {
        int NumColors { get; }

        IExerciseColor<T> GetColorByName(AvailableColors name);
        T GetCocosColorByName(AvailableColors name);
        IColorManagerInitializer BasicColorManager { get; }
    }
}
