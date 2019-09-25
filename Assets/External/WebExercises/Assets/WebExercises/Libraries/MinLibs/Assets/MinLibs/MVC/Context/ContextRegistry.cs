using System;

namespace MinLibs.MVC
{
	public partial class Context
	{
		readonly TypeRegistry typeRegistry = new TypeRegistry();

		public void RegisterClass<T> (RegisterFlags flags = RegisterFlags.None) where T : class, new()
		{
			RegisterClass<T, T>(flags);
		}

		public void RegisterClass<TInterface, TClass> (RegisterFlags flags = RegisterFlags.None)
			where TInterface : class where TClass : class, TInterface, new()
		{
			RegisterType(typeof(TInterface), typeof(TClass), flags);
		}

		public void RegisterType (Type type, RegisterFlags flags = RegisterFlags.None)
		{
			RegisterType(type, type, flags);
		}

		public void RegisterType (Type keyType, Type valueType, RegisterFlags flags = RegisterFlags.None)
		{
			typeRegistry.RegisterType(keyType, valueType, flags);
			instanceCache.Remove(keyType);
		}

		public void RegisterHandler<T> (Func<object, object> handler, RegisterFlags flags = RegisterFlags.NoCache)
		{
			RegisterHandler(typeof(T), handler, flags);
		}

		public void RegisterHandler (Type type, Func<object, object> handler, RegisterFlags flags = RegisterFlags.NoCache)
		{
			typeRegistry.RegisterHandler(type, handler, flags);
			instanceCache.Remove(type);
		}

		public void RegisterInstance<T> (T instance, RegisterFlags flags = RegisterFlags.None)
		{
			RegisterInstance(typeof(T), instance, flags);
		}

		public void RegisterInstance (Type type, object instance, RegisterFlags flags = RegisterFlags.None)
		{
			flags &= ~RegisterFlags.NoCache;
			typeRegistry.RegisterType(type, null, flags);
			instanceCache[type] = instance;
		}

		public void Unregister<T> ()
		{
			Unregister(typeof(T));
		}

		public void Unregister (Type type)
		{
			typeRegistry.Remove(type);
			instanceCache.Remove(type);
		}

		public bool Has<T> (bool checkParent = false)
		{
			return Has(typeof(T), checkParent);
		}

		public bool Has (Type type, bool checkParent = false)
		{
			return typeRegistry.Has(type) || checkParent && HasParent && parent.Has(type);
		}
	}
}
