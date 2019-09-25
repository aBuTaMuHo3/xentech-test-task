using System;
using System.Collections.Generic;
using MinLibs.Utils;

namespace MinLibs.MVC
{
	internal class TypeRegistry
	{
		readonly IDictionary<Type, Type> typeMap = new Dictionary<Type, Type>();
		readonly IDictionary<Type, Func<object, object>> handlerMap = new Dictionary<Type, Func<object, object>>();
		readonly IDictionary<Type, InjectionStrategy> injectionStrategyMap = new Dictionary<Type, InjectionStrategy>();

		internal void RegisterHandler (Type key, Func<object, object> handler, RegisterFlags flags)
		{
			Register(handlerMap, key, handler, flags);
		}

		internal void RegisterType (Type key, Type value, RegisterFlags flags)
		{
			if (value != null && value.IsInterface) {
				throw new CannotRegisterInterfaceAsValueException(value.Name + " is an interface");
			}

			Register(typeMap, key, value, flags);
		}

		internal object CreateInstance (Type type, object host)
		{
			return CreateWithActivator(type) ?? CreateWithHandler(type, host);
		}

		private object CreateWithActivator (Type type)
		{
			var mapped = typeMap.Retrieve(type, null);

			return mapped != null ? Activator.CreateInstance(mapped) : null;
		}

		private object CreateWithHandler (Type type, object host)
		{
			var handler = handlerMap.Retrieve(type, null);

			return handler?.Invoke(host);
		}

		internal bool Has (Type type)
		{
			return typeMap.ContainsKey(type) || handlerMap.ContainsKey(type);
		}

		internal void Remove (Type type)
		{
			typeMap.Remove(type);
			handlerMap.Remove(type);
			injectionStrategyMap.Remove(type);
		}

		internal InjectionStrategy GetInjectionStrategy (Type type)
		{
			return injectionStrategyMap.Retrieve(type, InjectionStrategy.Never);
		}

		internal void SetInjectionStrategy (Type type, InjectionStrategy strategy)
		{
			injectionStrategyMap[type] = strategy;
		}

		private void Register<T> (IDictionary<Type, T> dict, Type key, T value, RegisterFlags flags)
		{
			Store(dict, key, value, flags);
			
			var strategy = ProcessInjectionStrategy(flags);
			SetInjectionStrategy(key, strategy);
		}

		private static InjectionStrategy ProcessInjectionStrategy (RegisterFlags flags)
		{
			var injectionStrategy = InjectionStrategy.Once;

			if (flags.Has(RegisterFlags.PreventInjections)) {
				injectionStrategy = InjectionStrategy.Never;
			}
			else if (flags.Has(RegisterFlags.NoCache)) {
				injectionStrategy = InjectionStrategy.Always;
			}

			return injectionStrategy;
		}

		private static void Store<T> (IDictionary<Type, T> dict, Type key, T value, RegisterFlags flags)
		{
			if (!dict.AddNewEntry(key, value)) {
				if (flags.Has(RegisterFlags.Rebind)) {
					dict.UpdateEntry(key, value);
				}
				else {
					throw new AlreadyRegisteredException(key + " not storable in " + dict);
				}
			}
		}
	}

	internal enum InjectionStrategy
	{
		Never = 0,
		Once = 1,
		Always = 2
	}
}
