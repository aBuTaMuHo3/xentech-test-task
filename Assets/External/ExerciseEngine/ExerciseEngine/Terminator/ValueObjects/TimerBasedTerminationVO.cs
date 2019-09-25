using ExerciseEngine.Terminator.ValueObjects.Interfaces;
using SynaptikonFramework.Interfaces.Util.Timer;

namespace ExerciseEngine.Terminator.ValueObjects
{
    public class TimerBasedTerminationVO : IExerciseTerminatorUpdateVO
    {
        public ITimerUpdateVO TimerUpdateVO { get; }

        public TimerBasedTerminationVO(ITimerUpdateVO timerUpdateVO)
        {
            TimerUpdateVO = timerUpdateVO;
        }
    }
}
