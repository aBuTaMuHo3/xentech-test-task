using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace WebExercises.FlashGlance
{
    public class FlashGlanceSliderItem : MonoBehaviour
    {
        [SerializeField] private TMP_Text label;
        [SerializeField] private Image basic;
        [SerializeField] private Image selected;
        [SerializeField] private float duration;
        [SerializeField] private CanvasGroup group;
        [SerializeField] private float radius;

        private RectTransform _rect;

        private RectTransform Rect
        {
            get
            {
                _rect = _rect ?? (RectTransform)transform;
                return _rect;
            }
        }

        public float X
        {
            private get { return transform.localPosition.x; }
            set
            {
                var pos = transform.localPosition;
                pos.x = value;
                transform.localPosition = pos;
            }
        }

        public float Y
        {
            private get { return transform.localPosition.y; }
            set
            {
                var pos = transform.localPosition;
                pos.y = value;
                transform.localPosition = pos;
            }
        }

        public void SetLabel(string text)
        {
            label.gameObject.SetActive(true);
            label.text = text;
        }

        public Tweener MoveTo(float target)
        {
            return transform.DOLocalMoveX(target, duration);
        }

        public Tweener MoveBy(float distance)
        {
            return transform.DOLocalMoveX(X + distance, duration);
        }

        public Sequence RotateBy(float angle)
        {
            Vector3 pos = Quaternion.Euler(0,0,angle) * transform.localPosition;

            return Rect.DOJumpAnchorPos(pos, radius, 1, duration);
        }

        public void SetRotation(float angle)
        {
            Vector3 pos = Quaternion.Euler(0f, 0f, angle) * transform.localPosition;
            transform.localPosition = pos;
        }

        public Tweener Appear()
        {
            group.alpha = 0;
            return group.DOFade(1f, duration);
        }

        public Tweener Disappear()
        {
            return group.DOFade(0f, duration);
        }

        public void Hide()
        {
            group.alpha = 0f;
        }

        public Sequence SetSearched()
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Join(transform.DOScale(Vector3.one, duration));
            sequence.Join(basic.DOFade(0f, duration));
            sequence.Join(selected.DOFade(1f, duration));
            return sequence;
        }

        public Sequence SetUpcoming()
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Join(transform.DOScale(Vector3.one/2, duration));
            sequence.Join(basic.DOFade(1f, duration));
            sequence.Join(selected.DOFade(0f, duration));
            return sequence;
        }
    }
}

