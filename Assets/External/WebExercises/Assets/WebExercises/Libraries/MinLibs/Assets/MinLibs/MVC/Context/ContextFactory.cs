using System;
using System.Collections.Generic;
using MinLibs.Utils;

namespace MinLibs.MVC
{
	public partial class Context
	{
		public ContextFlags ContextFlags { get; set; }
		
		readonly IDictionary<Type, object> instanceCache = new Dictionary<Type, object>();

		public T Get<T> (object host = null)
		{
			return Get<T>(typeof(T), host);
		}

		public T Get<T> (Type key, object host = null)
		{
			return (T)GetInstance(key, host);
		}

		public object GetInstance (Type type, object host = null)
		{
			var instance = instanceCache.Retrieve(type, null) ?? CreateInstance(type, host);

			if (instance != null) {
				EnsureInjection(type, instance);
			}
			else {
				instance = HandleMissingRegistration(type, host);
			}
			
			return instance;
		}

		private void EnsureInjection (Type type, object instance)
		{
			var injectionStrategy = typeRegistry.GetInjectionStrategy(type);

			if (injectionStrategy != InjectionStrategy.Never) {
				if (injectionStrategy == InjectionStrategy.Once) {
					typeRegistry.SetInjectionStrategy(type, InjectionStrategy.Never);
				}
				
				Inject(instance);
			}
		}

		public T Create<T> () where T : new()
		{
			var instance = Activator.CreateInstance(typeof(T));
			Inject(instance);

			return (T)instance;
		}

		private object CreateInstance (Type type, object host)
		{
			var instance = typeRegistry.CreateInstance(type, host);

			if (instance != null) {
				if (typeRegistry.GetInjectionStrategy(type) != InjectionStrategy.Always) {
					instanceCache[type] = instance;
				}
			}
			else if (HasParent) {
				instance = parent.GetInstance(type, host);

				if (instance != null)
				{
					Output.Dispatch("got " + type + " from " + parent);
				}
			}

			return instance;
		}

		private object HandleMissingRegistration (Type type, object host)
		{
			object instance = null;

			if (ContextFlags.Has(ContextFlags.AutoResolve) && type != null) {
				instance = AutoResolve(type, host);
			}
			else if (ContextFlags.Has(ContextFlags.Exception)) {
				throw new NotRegisteredException("not registered: " + type);
			}
			else if (ContextFlags.Has(ContextFlags.Warning)) {
				Output.Dispatch("not registered: " + type);
			}

			return instance;
		}

		protected virtual object AutoResolve (Type type, object host)
		{
			return type.IsClass ? ResolveClass(type, host) : null;
		}

		private object ResolveClass (Type type, object host)
		{
			RegisterType(type);

			return GetInstance(type, host);
		}
	}
}
