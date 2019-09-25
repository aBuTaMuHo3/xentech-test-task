using System;
using System.Reflection;

namespace MinLibs.MVC
{
	[Flags]
	public enum ContextFlags
	{
		None = 0,
		Warning = 1,
		Exception = 2,
		AutoResolve = 4
	}

	[Flags]
	public enum RegisterFlags
	{
		None = 0,
		NoCache = 1,
		PreventInjections = 2,
		Rebind = 4,
		Parameter = 8
	}

	[Flags]
	public enum InjectorFlags
	{
		None = 0,
		PreventPostInjection = 1,
		PreventCleanup = 2
	}

	public static class FlagExtensions
	{
		public static bool Has (this ContextFlags flags, ContextFlags flag)
		{
			return (flags & flag) != 0;
		}

		public static bool Has (this RegisterFlags flags, RegisterFlags flag)
		{
			return (flags & flag) != 0;
		}

		public static bool Has (this InjectorFlags flags, InjectorFlags flag)
		{
			return (flags & flag) != 0;
		}

		public static bool Has (this BindingFlags flags, BindingFlags flag)
		{
			return (flags & flag) != 0;
		}
	}
}
