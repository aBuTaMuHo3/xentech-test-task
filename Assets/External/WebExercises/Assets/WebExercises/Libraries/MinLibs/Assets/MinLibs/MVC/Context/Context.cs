using System.Reflection;
using MinLibs.Signals;

namespace MinLibs.MVC
{
	public partial class Context : IContext
	{
		public string Id { get; set; }

		public Signal OnCleanUp { get; } = new Signal();

		public Signal<string> Output { get; } = new Signal<string>();

		public IContext Parent {
			get {
				return parent;
			}

			set {
				if (HasParent) {
					parent.OnCleanUp.RemoveListener(CleanUp);
				}

				parent = value;

				if (HasParent) {
					parent.OnCleanUp.AddListener(CleanUp);
				}
			}
		}

		private bool HasParent => parent != null;

		private IContext parent;

		public Context ()
		{
			ContextFlags = ContextFlags.Exception;
			BindingFlags = BindingFlags.SetProperty | BindingFlags.SetField;
			InjectorFlags = InjectorFlags.None;

			RegisterInstance<IContext>(this);
			RegisterInstance<IContextRegistry>(this);
			RegisterInstance<IContextFactory>(this);
			RegisterInstance<IContextInjector>(this);
		}

		public void CleanUp ()
		{
			Parent = null;

			OnCleanUp.Dispatch();
			OnCleanUp.RemoveAllListeners();
		}
	}
}
