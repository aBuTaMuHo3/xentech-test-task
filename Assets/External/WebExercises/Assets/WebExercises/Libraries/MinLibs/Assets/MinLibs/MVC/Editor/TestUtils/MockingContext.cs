using System;
using NSubstitute;

namespace MinLibs.MVC.TestUtils
{
	public class MockingContext : Context
	{
		public MockingContext ()
		{
			ContextFlags |= ContextFlags.AutoResolve;
			InjectorFlags |= InjectorFlags.PreventPostInjection;
		}

		protected override object AutoResolve (Type type, object host)
		{
			return type.IsInterface ? ResolveInterface(type) : base.AutoResolve(type, host);
		}

		object ResolveInterface (Type type)
		{
			var substitute = Substitute.For(new[] {type}, new object[0]);
			RegisterInstance(type, substitute);

			return substitute;
		}
	}
}