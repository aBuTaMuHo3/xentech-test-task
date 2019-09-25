using ExerciseEngine.HUD.ValueObjects.Interfaces;
using ExerciseEngine.View.ValueObjects.Interfaces;

namespace ExerciseEngine.View.ValueObjects {
    public class ShowScoreAnimationVO: IExerciseViewUpdateVO, IExerciseHudVO {

        public ShowScoreAnimationVO(int score, int scoreChange) {
            Score = score;
            ScoreChange = scoreChange;
        }

        public int Score { get; }
        public int ScoreChange { get; }
    }
}