using MinLibs.Signals;
using UnityEngine;

namespace MinLibs.MVC
{
	public interface IMediatedBehaviour : IMediated
	{
		Signal OnEnabled { get; }
		Signal OnDisabled { get; }

		void SetParent (Transform parent, bool worldPositionStays = false);
		void SetActive (bool isActive);
	}
}
