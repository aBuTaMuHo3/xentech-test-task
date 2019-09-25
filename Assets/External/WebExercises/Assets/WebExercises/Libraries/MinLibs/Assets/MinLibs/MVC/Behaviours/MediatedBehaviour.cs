using MinLibs.Signals;
using UnityEngine;

namespace MinLibs.MVC
{
	public abstract class MediatedBehaviour : MonoBehaviour, IMediatedBehaviour
	{
		public Signal OnEnabled { get; } = new Signal();
		public Signal OnDisabled { get; } = new Signal();
		public Signal OnRemove { get; } = new Signal();

		private bool isMediated;
		
		protected virtual void Awake ()
		{
			MediateBehaviour();
		}

		public virtual void SetParent (Transform parent, bool worldPositionStays = false)
		{
			transform.SetParent(parent, worldPositionStays);
			MediateBehaviour();
		}

		protected virtual void OnDestroy ()
		{
			OnRemove?.Dispatch();
			Cleanup();
		}

		protected virtual void OnEnable ()
		{
			OnEnabled?.Dispatch();
		}

		protected virtual void OnDisable ()
		{
			OnDisabled?.Dispatch();
		}

		public virtual void HandleMediation ()
		{

		}

		public virtual void SetActive (bool isActive)
		{
			gameObject.SetActive(isActive);
		}

		public virtual void Remove ()
		{
			if (gameObject) {
				Destroy(gameObject);
			}
			else {
				OnDestroy();
			}
		}

		protected virtual void Cleanup ()
		{
		}

		protected virtual void MediateBehaviour ()
		{
			if (isMediated) return;
			
			var root = transform.GetComponentInParent<IMediating>();
	
			if (root != null && !root.Equals(this)) {
				isMediated = true;
				root.OnMediate.Dispatch(this);
			}
		}
	}
}
