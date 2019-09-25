using System;

namespace MinLibs.MVC
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public class Inject : Attribute
	{

	}

	[AttributeUsage(AttributeTargets.Method)]
	public class PostInjection : Attribute
	{

	}

	[AttributeUsage(AttributeTargets.Method)]
	public class Cleanup : Attribute
	{

	}
}
