using ExerciseEngine.Model.ValueObjects.Interfaces;

namespace Memoflow.Model
{
    public class MemoflowRoundItemVO: IRoundItem
    {
        public bool Correct { get; }

        public MemoflowRoundItemVO(bool correct)
        {
            Correct = correct;
        }
    }
}
