using WebExercises.Shared;

namespace WebExercises.HUD
{
	public interface IHUDLevel : IHUDItem
	{
		void SetLevel(string current, string max);
		void SetRuns(float percentage);
	}

	public class HUDLevelMediator : HUDItemMediator<IHUDLevel>
	{
		private int maxGoodRuns;
		private int maxBadRuns;

		protected override void Register()
		{
			base.Register();
			
			signals.Register(controller.OnUpdateRuns, OnUpdateRuns);
			
			InitView(TextKeys.HUD_LEVEL_HEADER);
		}

		private void OnUpdateRuns(int runs)
		{
			var max = runs < 0 ? maxBadRuns : maxGoodRuns;
			var current = runs % max;
			var factor = current / (float)max;
			mediated.SetRuns(factor);
		}

		protected override void UpdateHUD(HUDLevelInfo info)
		{
			base.UpdateHUD(info);

			var currentDiff = info.Difficulty.CurrentValue + 1;
			var maxDiff = info.MaxDifficulty.CurrentValue + 1;
			mediated.SetLevel(currentDiff.ToString(), maxDiff.ToString());

			maxGoodRuns = info.GoodRuns.CurrentValue;
			maxBadRuns = info.BadRuns.CurrentValue;
			
			mediated.SetRuns(0);
		}
	}
}