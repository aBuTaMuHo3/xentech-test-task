using MinLibs.Signals;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace WebExercises.Shared.Components
{
	[RequireComponent(typeof(Image))]
	[RequireComponent(typeof(EventTrigger))]
	public class ToggleButton : MonoBehaviour
	{
		[SerializeField] private Image icon;
		[Header("Sprites")]
		[SerializeField] private Sprite onIcon;
		[SerializeField] private Sprite offIcon;

		private Image _image;
		private EventTrigger _events;

		private bool _isOn;
		
		public Signal<bool> OnToggle { get; } = new Signal<bool>();

		public bool IsOn
		{
			private get { return _isOn; }
			set
			{
				_isOn = value;
				UpdateIcon();
			}
		}

		public bool ActOnDown;

		private void Awake()
		{
			_image = GetComponent<Image>();
			_events = GetComponent<EventTrigger>();

			CreateEntry(EventTriggerType.PointerDown, OnDown);
			CreateEntry(EventTriggerType.PointerClick, OnClick);
			
			UpdateIcon();
		}

		private void CreateEntry(EventTriggerType type, UnityAction<BaseEventData> action)
		{
			var entry = new EventTrigger.Entry { eventID = type };
			entry.callback.AddListener(action);
			_events.triggers.Add(entry);
		}

		private void OnDown(BaseEventData data)
		{
			if (ActOnDown)
			{
				Toggle();
			}
		}

		private void OnClick(BaseEventData data)
		{
			if (!ActOnDown)
			{
				Toggle();
			}
		}

		private void Toggle()
		{
			IsOn = !IsOn;
			OnToggle.Dispatch(IsOn);
		}

		private void UpdateIcon()
		{
			icon.sprite = _isOn ? onIcon : offIcon;
		}
	}
}