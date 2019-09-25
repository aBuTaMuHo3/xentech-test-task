using System;
using System.Collections.Generic;
using System.Reflection;

namespace MinLibs.MVC
{
	internal class Injector
	{
		static readonly object[] EMPTY_PARAMS = new object[0];

		readonly IDictionary<Type, InjectionInfo> infoMap = new Dictionary<Type, InjectionInfo>();

		internal void Inject<T> (T instance,
								Func<Type, object, object> getInstanceHandler,
								Action<Action> cleanupHandler,
								BindingFlags bindingFlags,
								InjectorFlags injectorFlags)
		{
			var key = instance.GetType();
			var info = RetrieveInfo(key, bindingFlags, injectorFlags);

			if (info.HasInjections()) {
				InjectInstances(getInstanceHandler, instance, key, info.Injections, bindingFlags);
			}

			if (info.HasCalls<PostInjection>()) {
				InvokeMethods(instance, info.GetCalls<PostInjection>());
			}

			if (info.HasCalls<Cleanup>()) {
				RegisterCleanups(cleanupHandler, instance, info.GetCalls<Cleanup>());
			}
		}

		private InjectionInfo RetrieveInfo (Type type, BindingFlags bindingFlags, InjectorFlags injectorFlag)
		{
			InjectionInfo info;

			if (!infoMap.TryGetValue(type, out info)) {
				infoMap[type] = info = InfoParser.Parse(type, bindingFlags, injectorFlag);
			}

			return info;
		}

		private static void InjectInstances<T> (Func<Type, object, object> getInstance, T instance, Type type, IDictionary<string, Type> injectionMap,
										BindingFlags flags)
		{
			foreach (var pair in injectionMap) {
				var injection = getInstance(pair.Value, instance);
				var param = new[] {injection};
				type.InvokeMember(pair.Key, flags, null, instance, param);
			}
		}

		private static void InvokeMethods (object instance, IEnumerable<MethodInfo> methodInfos, object[] param = null)
		{
			param = param ?? EMPTY_PARAMS;

			foreach (var methodInfo in methodInfos) {
				InvokeMethod(methodInfo, instance, param);
			}
		}

		private static void InvokeMethod (MethodBase methodInfo, object instance, object[] param)
		{
			try {
				methodInfo.Invoke(instance, param);
			}
			catch (TargetInvocationException ex) {
				throw ex.GetBaseException();
			}
		}

		private static void RegisterCleanups<T> (Action<Action> cleanupHandler, T instance, IList<MethodInfo> methodInfos)
		{
			for (var i = 0; i < methodInfos.Count; i++) {
				var methodInfo = methodInfos[i];
				cleanupHandler(() => InvokeMethod(methodInfo, instance, EMPTY_PARAMS));
			}
		}
	}
}
