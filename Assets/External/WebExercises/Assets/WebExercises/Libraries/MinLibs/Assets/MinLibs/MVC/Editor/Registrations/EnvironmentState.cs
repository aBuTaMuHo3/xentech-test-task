using System;
using UnityEngine;

namespace MinLibs.MVC
{
	[CreateAssetMenu(menuName = "MinMVC/Create Environment States")]
	public class EnvironmentStates : ScriptableObject
	{
		public string currentState;

		public EnvironmentState[] states;
	}

	[Serializable]
	public class EnvironmentState
	{
		public string name;

		public PlatformFlags platform = PlatformFlags.All;

		public EditorFlags editor = EditorFlags.NotRelevant;

		public EnvFlags env = EnvFlags.CI;

		public ProviderFlags provider = ProviderFlags.All;
	}
}