using UnityEngine;
using UnityEngine.EventSystems;

namespace Xenia.ColorPicker.Core.Parts.LowerPanel.PartialColorInput
{
    internal delegate void SlidingEventHandler(object sender, SlidingEventArgs e);

    internal class ColorSliderHandle : MonoBehaviour, IDragHandler
    {
        internal event SlidingEventHandler Sliding;

        private RectTransform self;

        private float colorBarHalfWidth;
        private float colorBarWidth;

        private void Awake()
        {
            self = GetComponent<RectTransform>();

            colorBarWidth = GetComponentInParent<ColorSlider>().GetComponent<RectTransform>().rect.width - self.rect.width;
            colorBarHalfWidth = colorBarWidth / 2;
        }

        internal void MoveTo(float pos)
        {
            self.localPosition = new Vector3(pos * colorBarWidth - colorBarHalfWidth, 0, 0);
        }

        public void OnDrag(PointerEventData eventData)
        {
            float targetX = eventData.delta.x + self.localPosition.x;
            float actualX = (targetX > colorBarHalfWidth) ? colorBarHalfWidth : (targetX < -colorBarHalfWidth) ? -colorBarHalfWidth : targetX;

            self.localPosition = new Vector3(actualX, 0, 0);

            OnSliding(new SlidingEventArgs((actualX + colorBarHalfWidth) / colorBarWidth));
        }

        protected virtual void OnSliding(SlidingEventArgs e)
        {
            Sliding?.Invoke(this, e);
        }
    }
}
