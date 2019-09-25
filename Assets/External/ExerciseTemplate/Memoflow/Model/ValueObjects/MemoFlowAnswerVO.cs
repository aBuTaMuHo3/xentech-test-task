using ExerciseEngine.Model.ValueObjects.Interfaces;

namespace Memoflow.Model.ValueObjects {
    public class MemoFlowAnswerVO : IRoundItem {

        public MemoFlowAnswerVO(bool correct) {
            Correct = correct;
        }

        public bool Correct { get; }
    }
}
