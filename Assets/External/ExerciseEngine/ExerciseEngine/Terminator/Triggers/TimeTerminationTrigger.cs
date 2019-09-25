using ExerciseEngine.Terminator.Triggers.Interfaces;
using SynaptikonFramework.Interfaces.Util.Timer;

namespace ExerciseEngine.Terminator.Triggers
{
    public class TimeTerminationTrigger : ITerminatorTrigger
    {
        public ITimerUpdateVO TimerUpdateVO { get; }

        public TimeTerminationTrigger(ITimerUpdateVO timerUpdateVO)
        {
            TimerUpdateVO = timerUpdateVO;
        }
    }
}
