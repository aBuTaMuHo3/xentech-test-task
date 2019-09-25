using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using MinLibs.MVC;
using MinLibs.Signals;
using MinLibs.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace WebExercises.Memoflow
{
	public class MemoflowItemPanelView : MediatedBehaviour, IMemoflowItemPanel
	{
		private const float STEP = 0.5f;
		
		[SerializeField] private float gap;
		[SerializeField] private MemoflowItem itemPrefab;
		[SerializeField] private Image arrow;

		private float itemWidth;

		private readonly IList<MemoflowItem> usedItems = new List<MemoflowItem>();
		private readonly IList<MemoflowItem> cachedItems = new List<MemoflowItem>();

		public Signal OnItemsMoved { get; } = new Signal();

		public void AddItem(int symbolId)
		{
			var newItem = InitItem(symbolId);
			itemWidth = newItem.Width;
			var distance = itemWidth + gap;
			var count = usedItems.Count;

			for (var i = 0; i < count; i++)
			{
				var item = usedItems[i];
				var target = CalcTarget(i, count);
				item.X = target * distance;
			}
		}

		public void AppendItem(int symbolId, int countDelta)
		{
			var oldCount = usedItems.Count;
			var newItem = InitItem(symbolId);
			var distance = itemWidth + gap;
			var target = CalcTarget(oldCount, oldCount);
			newItem.X = target * distance;
			
			var sequence = DOTween.Sequence();
			var count = usedItems.Count;

			for (var i = 0; i < count; i++)
			{
				var item = usedItems[i];

				if (i <= -countDelta)
				{
					sequence.Join(item.Disappear());
				}
				else if (i == count - 1)
				{
					sequence.Join(item.Appear());
				}
				else
				{
					sequence.Join(item.CoverIcon(countDelta == 0));
				}
				
				var index = i + countDelta;
				target = CalcTarget(index, count + countDelta) - STEP;
				sequence.Join(item.MoveTo(target * distance));
			}

			sequence.AppendCallback(() => OnItemsMoveFinished(countDelta));
		}

		private float CalcTarget(int index, int total)
		{
			var start = -(total / 2f - STEP);

			return start + index;
		}

		private void OnItemsMoveFinished(int countDelta)
		{
			for (var i = 0; i <= -countDelta; i++)
			{
				var firstItem = usedItems.Pop();
				firstItem.CleanUp();
				cachedItems.Add(firstItem);
			}

			OnItemsMoved.Dispatch();
		}

		private MemoflowItem InitItem(int symbolId)
		{
			var newItem = cachedItems.Count > 0 ? cachedItems.Pop() : Instantiate(itemPrefab, transform);
			newItem.gameObject.SetActive(true);
			newItem.SetLabel(symbolId.ToString());
			usedItems.Add(newItem);

			return newItem;
		}

		public void HideItems()
		{
			for (var i = 0; i < usedItems.Count - 1; i++)
			{
				usedItems[i].CoverIcon(true);
			}
		}

		public void ShowIndicator(bool isCorrect)
		{
			usedItems.Last().SetIndicator(isCorrect);
		}

		public void ShowArrow()
		{
			ResizeArrow();
			arrow.DOFade(1f, 1f);
		}

		private void ResizeArrow()
		{
			var count = usedItems.Count - 1;
			var width = count * (itemWidth + gap) + 26;
			var size = arrow.rectTransform.sizeDelta;
			size.x = width;
			arrow.rectTransform.sizeDelta = size;
		}
	}
}
