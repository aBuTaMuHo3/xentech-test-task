using System;
using UnityEngine;

namespace MinLibs.MVC
{
	[Flags]
	public enum PlatformFlags
	{
		None = 0,
		Android = 1,
		iOS = 2,
		WebGL = 4,
		Mobile = Android | iOS,
		All = WebGL | Mobile
	}

	[Flags]
	public enum EditorFlags
	{
		None = 0,
		IsEditor = 1,
		IsNotEditor = 2,
		NotRelevant = IsEditor | IsNotEditor
	}

	[Flags]
	public enum EnvFlags
	{
		None = 0,
		CI = 1,
		Staging = 2,
		Production = 4,
		All = CI | Staging | Production
	}

	[Flags]
	public enum ProviderFlags
	{
		None = 0,
		Apple = 1,
		Google = 2,
		Amazon = 4,
		Facebook = 8,
		All = Apple | Google | Amazon | Facebook
	}

	public static class FlagsExtension
	{
		public static bool Has (this PlatformFlags flags, PlatformFlags flag)
		{
			return (flags & flag) != 0;
		}

		public static bool Has (this EditorFlags flags, EditorFlags flag)
		{
			return (flags & flag) != 0;
		}

		public static bool Has (this EnvFlags flags, EnvFlags flag)
		{
			return (flags & flag) != 0;
		}

		public static bool Has (this ProviderFlags flags, ProviderFlags flag)
		{
			return (flags & flag) != 0;
		}
	}
}