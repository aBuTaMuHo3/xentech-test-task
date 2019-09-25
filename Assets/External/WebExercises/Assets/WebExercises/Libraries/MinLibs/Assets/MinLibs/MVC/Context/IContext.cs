using System;
using System.Reflection;
using MinLibs.Signals;

namespace MinLibs.MVC
{
	public interface IContext : IContextRegistry, IContextFactory, IContextInjector
	{
		string Id { get; set; }

		IContext Parent { set; get; }

		ContextFlags ContextFlags { get; set; }

		InjectorFlags InjectorFlags { get; set; }

		BindingFlags BindingFlags { get; set; }

		Signal<string> Output { get; }

		Signal OnCleanUp { get; }

		void CleanUp ();
	}

	public interface IContextRegistry
	{
		void RegisterType (Type type, RegisterFlags flags = RegisterFlags.None);

		void RegisterType (Type keyType, Type valueType, RegisterFlags flags = RegisterFlags.None);

		void RegisterClass<T> (RegisterFlags flags = RegisterFlags.None) where T : class, new();

		void RegisterClass<TInterface, TClass> (RegisterFlags flags = RegisterFlags.None) where TInterface : class where TClass : class, TInterface, new();

		void RegisterInstance<T> (T instance, RegisterFlags flags = RegisterFlags.None);

		void RegisterInstance (Type type, object instance, RegisterFlags flags = RegisterFlags.None);

		void RegisterHandler<T> (Func<object, object> handler, RegisterFlags flags = RegisterFlags.NoCache);

		void RegisterHandler (Type type, Func<object, object> handler, RegisterFlags flags = RegisterFlags.NoCache);

		bool Has<T> (bool checkParent = false);

		bool Has (Type type, bool checkParent = false);

		void Unregister<T> ();

		void Unregister (Type type);
	}

	public interface IContextFactory
	{
		T Get<T> (object host = null);

		T Get<T> (Type key, object host = null);

		object GetInstance (Type key, object host = null);

		T Create<T> () where T : new();
	}

	public interface IContextInjector
	{
		void Inject<T> (T instance);
	}
}
