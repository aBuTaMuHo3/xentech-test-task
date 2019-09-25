using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using WebExercises.Shared.Utils;

namespace WebExercises.Exercise
{
	public class HoverEffect : MonoBehaviour
	{
		[SerializeField] private float scaleFactor;
		[SerializeField] private float scaleTime;
		[SerializeField] private bool isActive = true;
		
		private EventTrigger eventTrigger;
		private float originalScale;

		public bool IsActive {
			get => isActive;
			set
			{
				if (value != isActive)
				{
					isActive = value;
					Reset();
				}
			}
			
		}

		private void Awake()
		{
			originalScale = transform.localScale.x;
			eventTrigger = transform.EnsureComponent<EventTrigger>();
			CreateEntry(EventTriggerType.PointerEnter, OnOver);
			CreateEntry(EventTriggerType.PointerExit, OnOut);
		}

		private void OnOver(BaseEventData arg0)
		{
			Scale(originalScale * scaleFactor);
		}

		private void OnOut(BaseEventData arg0)
		{
			Scale(originalScale);
		}

		private void Scale(float scale)
		{
			if (IsActive)
			{
				transform.DOScale(scale, scaleTime);
			}
		}

		private void OnEnable()
		{
			Reset();
		}

		private void OnDisable()
		{
			Reset();
		}

		private void CreateEntry(EventTriggerType type, UnityAction<BaseEventData> action)
		{
			var entry = new EventTrigger.Entry { eventID = type };
			entry.callback.AddListener(action);
			eventTrigger.triggers.Add(entry);
		}

		private void OnDestroy()
		{
			Reset();
		}

		private void Reset()
		{
			var scale = transform.localScale;
			scale.x = originalScale;
			scale.y = originalScale;
			transform.localScale = scale;
		}
	}
}