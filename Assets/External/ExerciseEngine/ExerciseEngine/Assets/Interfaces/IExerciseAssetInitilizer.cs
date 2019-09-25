using System;
using System.Threading.Tasks;

namespace ExerciseEngine.Assets.Interfaces
{
    public interface IExerciseAssetInitializer
    {
        Task InitAsync();
        void CustomDispose();
    }
}
