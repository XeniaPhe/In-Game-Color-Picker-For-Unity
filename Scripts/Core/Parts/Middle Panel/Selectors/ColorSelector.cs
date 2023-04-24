using UnityEngine;
using UnityEngine.EventSystems;

namespace Xenia.ColorPicker.Core.Parts.MiddlePanel
{
    internal abstract class ColorSelector : MonoBehaviour, IDragHandler, IColorChangeSubscriber
    {
        [SerializeField] protected ColorPicker colorPicker;
        [SerializeField] protected RectTransform targetDrawer;

        protected RectTransform rectTransform;

        protected virtual void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            colorPicker.ColorChangedInternal += ColorChanged;
        }

        protected abstract void CorrectPosition(Vector3 position);

        protected abstract void UpdateColorPicker();

        public abstract void ColorChanged(object sender, ColorChangedEventArgs e);

        public virtual void OnDrag(PointerEventData eventData)
        {
            Translate(rectTransform.localPosition + new Vector3(eventData.delta.x, eventData.delta.y, 0));
        }

        internal virtual void SelectPosition(Vector3 pos)
        {
            Translate(pos);
        }

        private void Translate(Vector3 pos)
        {
            CorrectPosition(pos);
            UpdateColorPicker();
        }
    }
}