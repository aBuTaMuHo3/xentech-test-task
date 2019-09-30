using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;


namespace WebExercises.FlashGlance
{
    //[RequireComponent(typeof(CanvasGroup))]
    public class FlashGlanceItem : MonoBehaviour
    {
        [SerializeField] private TMP_Text label;
        [SerializeField] private float duration;
        [SerializeField] private CanvasGroup group;
        [SerializeField] private Button button;

        private RectTransform _rect;

        private RectTransform Rect
        {
            get
            {
                _rect = _rect ?? (RectTransform)transform;
                return _rect;
            }
        }

        public void SetLabel(string text)
        {
            label.gameObject.SetActive(true);
            label.text = text;
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

        public float Size
        {
            private get { return Rect.rect.size.x; }
            set
            {
                Rect.sizeDelta = new Vector2(value, value);
            }
        }


        public int Rotation
        {
            get { return (int)transform.rotation.eulerAngles.z; }
            set
            {
                Vector3 rotation = new Vector3(0, 0, value);
                transform.localRotation = Quaternion.Euler(rotation);
            }
        }

        public void Show()
        {
            group.alpha = 1;
        }

        public void Hide()
        {
            group.alpha = 0;
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

        public bool IsHidden
        {
            get {return group.alpha == 0; }
        }

        public float Scale
        {
            get { return label.transform.localScale.x; }
            set
            {
               label.transform.localScale = new Vector3(value,value,value);
            }
        }

        public Button Button
        {
            get { return button; }
            
        }
    }
}
