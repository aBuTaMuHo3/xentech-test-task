using ExerciseEngine.Colors;
using MinLibs.MVC;
using WebExercises.Exercise;
using WebExercises.Exercise.Editor;
using WebExercises.Memoflow;
using WebExercises.Shared;

namespace WebExercises.FlashGlance.Editor
{
    public class FlashGlanceRegistrations : ExerciseRegistrations
    {
        public override RegistrationInfo Create()
        {
            var info = base.Create();
            info.FilePath = "Modules/Exercises/FlashGlance/Init";
            
            info.Add<SetupFlashGlanceContext>().As<ISetupContext>();
            info.Add<StartFlashGlance>().As<IStartExercise>();
            
            info.Add<BaseColorManagerInitializer>().As<IColorManagerInitializer>();

            info.Add<FlashGlanceViewMediator>().With(RegisterFlags.NoCache);
            
            return info;
        }
    }
}