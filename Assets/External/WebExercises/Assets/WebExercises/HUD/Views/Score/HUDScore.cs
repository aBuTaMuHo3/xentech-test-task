using WebExercises.Shared;

namespace WebExercises.HUD
{
	public interface IHUDScore : IHUDItem
	{
		void SetScore(string score);
	}

	public class HUDScoreMediator : HUDItemMediator<IHUDScore>
	{
		protected override void Register()
		{
			base.Register();
			
			signals.Register(controller.OnUpdateScore, SetScore);
			
			InitView(TextKeys.HUD_SCORE_HEADER);
			SetScore(0);
		}

		private void SetScore(int score)
		{
			const string FORMAT = "N0";
			var text = score.ToString(FORMAT);
			mediated.SetScore(text);
		}
	}
}