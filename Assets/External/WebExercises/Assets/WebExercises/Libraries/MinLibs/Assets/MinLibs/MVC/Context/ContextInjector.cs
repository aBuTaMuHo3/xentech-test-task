using System.Reflection;

namespace MinLibs.MVC
{
	public partial class Context
	{
		public BindingFlags BindingFlags { get; set; }
		public InjectorFlags InjectorFlags { get; set; }
		
		readonly Injector injector = new Injector();

		public void Inject<T> (T instance)
		{
			injector.Inject(instance, GetInstance, OnCleanUp.AddListener, BindingFlags, InjectorFlags);
		}
	}
}
