using System;
using System.Collections.Generic;
using System.Reflection;
using MinLibs.Utils;

namespace MinLibs.MVC
{
	public static class InfoParser
	{
		const BindingFlags BINDING_FLAGS = BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy;
		
		public static InjectionInfo Parse (Type type, BindingFlags bindingFlags, InjectorFlags injectorFlags)
		{
			var info = new InjectionInfo();

			if (bindingFlags.Has(BindingFlags.SetProperty)) {
				ParsePropertyAttributes<Inject>(type, info);
			}

			if (bindingFlags.Has(BindingFlags.SetField)) {
				ParseFieldAttributes<Inject>(type, info);
			}

			if (!injectorFlags.Has(InjectorFlags.PreventPostInjection)) {
				ParseMethodAttributes<PostInjection>(type, info);
			}

			if (!injectorFlags.Has(InjectorFlags.PreventCleanup)) {
				ParseMethodAttributes<Cleanup>(type, info);
			}

			return info;
		}

		private static void ParseFieldAttributes<T> (Type type, InjectionInfo info)
		{
			var fields = type.GetFields(BINDING_FLAGS);

			for (var i = 0; i < fields.Length; i++) {
				var field = fields[i];
				ParseAttributes<T>(field, field.FieldType, info);
			}
		}

		private static void ParsePropertyAttributes<T> (Type type, InjectionInfo info)
		{
			var properties = type.GetProperties(BINDING_FLAGS);

			for (var i = 0; i < properties.Length; i++) {
				var property = properties[i];
				ParseAttributes<T>(property, property.PropertyType, info);
			}
		}

		private static void ParseAttributes<T> (MemberInfo memberInfo, Type type, InjectionInfo info)
		{
			var customAttributes = memberInfo.GetCustomAttributes(typeof(T), true);
			var count = customAttributes.Length;

			if (count == 0) {
				return;
			}

			if (count == 1 && info.AddInjection(memberInfo.Name, type)) {
				return;
			}
			
			throw new AlreadyInjectedException("duplicate injection of " + type + " in " + memberInfo.ReflectedType);
		}

		private static void ParseMethodAttributes<T> (Type type, InjectionInfo info) where T : Attribute
		{
			var methods = type.GetMethods();

			for (var i = 0; i < methods.Length; i++) {
				var method = methods[i];
				var attributes = method.GetCustomAttributes(typeof(T), true);

				if (attributes.Length > 0) {
					info.GetCalls<T>(true).Add(method);
				}
			}
		}
	}

	public class InjectionInfo
	{
		public  IDictionary<string, Type> Injections { get;  } = new Dictionary<string, Type>();
		readonly IDictionary<Type, List<MethodInfo>> calls = new Dictionary<Type, List<MethodInfo>>();

		public bool HasInjections ()
		{
			return Injections.Count > 0;
		}

		internal bool AddInjection (string key, Type value)
		{
			return Injections.AddNewEntry(key, value);
		}

		public List<MethodInfo> GetCalls<T> (bool useCreator = false) where T : Attribute
		{
			return calls.Retrieve(typeof(T), useCreator);
		}

		public bool HasCalls<T> () where T : Attribute
		{
			return calls.ContainsKey(typeof(T));
		}
	}
}
