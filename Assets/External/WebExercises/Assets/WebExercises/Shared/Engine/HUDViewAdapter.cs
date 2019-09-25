using System.Collections.Generic;
using ExerciseEngine.HUD.Interfaces;
using ExerciseEngine.HUD.ValueObjects;
using ExerciseEngine.HUD.ValueObjects.Interfaces;
using ExerciseEngine.Model.ValueObjects;
using ExerciseEngine.View.ValueObjects.Interfaces;
using MinLibs.Logging;
using MinLibs.MVC;
using MinLibs.Utils;

namespace WebExercises.Shared
{
	public class HUDViewAdapter : IHUDViewAdapter
	{
		[Inject] public ILogging logger;
		[Inject] public IParser parser;
		[Inject] public IExerciseControllerProxy controllerProxy;
		
		private readonly HUDLevelInfo hudLevelInfo = new HUDLevelInfo();
		
		public void Dispose()
		{
			throw new System.NotImplementedException();
		}

		public event HudUpdateHandler OnUpdate;
		
		public void Update(List<IExerciseHudVO> data)
		{
			//logger.Trace("hud updates " + data.Count);
			
			foreach (var item in data)
			{
				//logger.Trace(item);
				//logger.Trace(item.ToJson(pretty: false));
				
				var itemType = item.GetType();
				
				if (itemType == typeof(StartTimeoutVO))
				{
					var vo = item as StartTimeoutVO;
					controllerProxy.OnStartTimeout.Dispatch(vo);
				}
				else if (itemType == typeof(StartExerciseTimeVO))
				{
					controllerProxy.OnStartExercise.Dispatch();
				}
				else if (itemType == typeof(ResetTimeoutVO))
				{
					controllerProxy.OnResetTimeout.Dispatch();
				}
				else if (itemType == typeof(UpdateExerciseTimeVO))
				{
					var vo = item as UpdateExerciseTimeVO;
					controllerProxy.OnUpdateTime.Dispatch(vo.Update);
				}
				else if (itemType == typeof(BadRunsToLevelDownUpdateVO))
				{
					HudLevelInfo.BadRuns = item as BadRunsToLevelDownUpdateVO;
				}
				else if (itemType == typeof(GoodRunsToLevelUpUpdateVO))
				{
					HudLevelInfo.GoodRuns = item as GoodRunsToLevelUpUpdateVO;
				}
				else if (itemType == typeof(DifficultyUpdateVO))
				{
//					var vo = item as DifficultyUpdateVO;
//
//					if (vo.CurrentValue == vo.ValueChange)
//					{
						HudLevelInfo.Difficulty = item as DifficultyUpdateVO;
//					}
					
				}
				else if (itemType == typeof(MaxDifficultyUpdateVO))
				{
					HudLevelInfo.MaxDifficulty = item as MaxDifficultyUpdateVO;
				}
				else if (itemType == typeof(GoodRunsUpdateVO))
				{
					var vo = item as GoodRunsUpdateVO;
					controllerProxy.OnUpdateRuns.Dispatch(vo.CurrentValue);
				}
				else if (itemType == typeof(BadRunsUpdateVO))
				{
					var vo = item as BadRunsUpdateVO;
					controllerProxy.OnUpdateRuns.Dispatch(-vo.CurrentValue);
				}
				else if (itemType == typeof(ScoreUpdateVO))
				{
					var vo = item as ScoreUpdateVO;
					controllerProxy.OnUpdateScore.Dispatch(vo.CurrentValue);
				}
				else
				{
					//logger.Warn(item);
					//logger.Warn(parser.Serialize(item));
				}
			}

			if (hudLevelInfo.HasUpdate)
			{
				hudLevelInfo.HasUpdate = false;
				controllerProxy.OnInitLevelHUD.Dispatch(hudLevelInfo);
			}
		}

		private HUDLevelInfo HudLevelInfo
		{
			get
			{
				hudLevelInfo.HasUpdate = true;
				return hudLevelInfo;
			}
		}

		public IExercisePaddings Paddings { get; set; }
		public IExercisePaddings Insets { get; }
		public IHudView View { get; }
	}

	public class HUDLevelInfo
	{
		public bool HasUpdate { get; set; }
		public BadRunsToLevelDownUpdateVO BadRuns { get; set; }
		public GoodRunsToLevelUpUpdateVO GoodRuns { get; set; }
		public DifficultyUpdateVO Difficulty { get; set; } = new DifficultyUpdateVO(0, 0);
		public MaxDifficultyUpdateVO MaxDifficulty { get; set; }
	}

	public interface IHUDViewAdapter : IExerciseHUD
	{
	}
}