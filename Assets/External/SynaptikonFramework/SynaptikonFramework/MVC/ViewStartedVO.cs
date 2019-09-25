using System;
using System.Collections.Generic;

namespace SynaptikonFramework.MVC
{
    public class ViewStartedVO : IViewValueObject
    {
        public IMVCView View { get; }
        public Dictionary<string, string> ViewExtraData { get; }

        public ViewStartedVO(IMVCView view, Dictionary<string,string> viewExtraData = null)
        {
            View = view;
            ViewExtraData = viewExtraData == null ? new Dictionary<string, string>() : viewExtraData;
        }
    }
}
