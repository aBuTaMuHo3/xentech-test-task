using System;
namespace SynaptikonFramework.MVC
{
    public class ViewStoppedVO : IViewValueObject
    {
        public IMVCView View { get; }

		public ViewStoppedVO(IMVCView view)
		{
			View = view;
		}
    }
}
