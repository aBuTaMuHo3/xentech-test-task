using WebExercises.Exercise;

namespace WebExercises.FlashGlance
{
    public class SetupFlashGlanceContext : SetupExerciseContext
    {
        protected override void SetupMediators()
        {
            base.SetupMediators();
            
            mediators.Map<IFlashGlanceView, FlashGlanceViewMediator>();
        }
    }
}
