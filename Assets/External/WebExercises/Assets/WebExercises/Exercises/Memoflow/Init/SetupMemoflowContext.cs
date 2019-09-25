using WebExercises.Exercise;
 
 namespace WebExercises.Memoflow
 {
 	public class SetupMemoflowContext : SetupExerciseContext
 	{
 		protected override void SetupMediators()
 		{
 			base.SetupMediators();
 			
 			mediators.Map<IMemoflowItemPanel, MemoflowItemPanelMediator>();
 			mediators.Map<IMemoflowUI, MemoflowUIMediator>();
 		}
 	}
 }