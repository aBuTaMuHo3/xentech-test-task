using System;
using System.Collections.Generic;

namespace MinLibs.MVC
{
	public interface IRegistrations
	{
		RegistrationInfo Create();
	}

	public abstract class BaseRegistrations : IRegistrations
	{
		protected RegistrationInfo CreateInfo()
		{
			var info = new RegistrationInfo
			{
				ClassName = "Apply" + GetType().Name
			};

			return info;
		}

		public abstract RegistrationInfo Create();
	}

	public class RegistrationData
	{
		public Type ClassType { get; }

		private readonly IList<Type> interfaceTypes = new List<Type>();

		public IList<Type> InterfaceTypes => interfaceTypes.Count == 0 ? new List<Type> { ClassType } : interfaceTypes;

		public RegisterFlags Flags { get; private set; }

		public PlatformFlags Platforms { get; private set; } = PlatformFlags.All;

		public EditorFlags Editor { get; private set; } = EditorFlags.NotRelevant;

		public EnvFlags Envs { get; private set; } = EnvFlags.All;

		public ProviderFlags Providers { get; private set; } = ProviderFlags.All;

		public RegistrationData(Type classType)
		{
			ClassType = classType;
		}

		public RegistrationData As<U>()
		{
			var interfaces = ClassType.GetInterfaces();
			var interfaceType = typeof(U);

			if (Array.IndexOf(interfaces, interfaceType) == -1)
			{
				throw new Exception(ClassType + " has no interface " + interfaceType);
			}

			interfaceTypes.Add(interfaceType);

			return this;
		}

		public RegistrationData With(RegisterFlags flags)
		{
			Flags = flags;

			return this;
		}

		public RegistrationData On(EnvFlags flags)
		{
			Envs = flags;

			return this;
		}

		public RegistrationData As(PlatformFlags flags)
		{
			Platforms = flags;

			return this;
		}

		public RegistrationData For(ProviderFlags flags)
		{
			Providers = flags;

			return this;
		}

		public RegistrationData In(EditorFlags flags)
		{
			Editor = flags;

			return this;
		}
	}

	public class RegistrationInfo
	{
		public string ClassName { get; set; }
		public string NameSpace { get; set; }
		public string FilePath { get; set; }
		public IList<RegistrationData> Registrations { get; } = new List<RegistrationData>();

		public RegistrationData Add<T>()
		{
			var type = typeof(T);
			var data = new RegistrationData(type);
			Registrations.Add(data);

			return data;
		}
	}
}
