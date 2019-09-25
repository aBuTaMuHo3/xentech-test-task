using System;
namespace SynaptikonFramework.MVC
{
    public interface IMVCView
    {
		/// <summary>
		/// global function for sending information to the view
		/// The views implementing this interface method have to cast the objects corresponding to them
		/// </summary>
		/// <param name="data">data value object implementing IViewValueObject</param>
        void UpdateView(IViewValueObject data);
    }
}
