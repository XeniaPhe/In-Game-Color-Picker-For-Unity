using UnityEngine;
using UnityEngine.UI;
using ColorUtility = Xenia.ColorPicker.Utility.ColorUtility;

namespace Xenia.ColorPicker.Core.Parts.MiddlePanel
{
    internal class SVSelector : ColorSelector
    {
        private float squareHalfSize;
        private float s, v;

        private Color normalColor;
        private Color inverseColor;
        private bool normallyBright;
        private Image self;

        protected override void Awake()
        {
            base.Awake();
            squareHalfSize = targetDrawer.rect.width / 2f;

            self = GetComponent<Image>();
            normalColor = self.color;
            inverseColor = ((HSVColor)normalColor).Inverse();
            normallyBright = ColorUtility.Brightness(normalColor) > 0.5f;
        }

        protected override void CorrectPosition(Vector3 position)
        {
            if (Mathf.Abs(position.x) > squareHalfSize)
                position.x = squareHalfSize * Mathf.Sign(position.x);

            if (Mathf.Abs(position.y) > squareHalfSize)
                position.y = squareHalfSize * Mathf.Sign(position.y);

            s = (position.x + squareHalfSize) / (2 * squareHalfSize);
            v = (position.y + squareHalfSize) / (2 * squareHalfSize);

            rectTransform.localPosition = position;
        }

        protected override void UpdateColorPicker()
        {
            colorPicker.AssignColorHSV(s: s, v: v);
        }

        public override void ColorChanged(object sender, ColorChangedEventArgs e)
        {
            s = 2 * (e.Color.S - 0.5f);
            v = 2 * (e.Color.V - 0.5f);

            Vector3 newPos = new Vector3(s * squareHalfSize , v * squareHalfSize, 0f);
            rectTransform.localPosition = newPos;

            if (!((ColorPicker)sender).AdjustSVSelectorColorBasedOnBrightness)
                return;

            if (ColorUtility.Brightness(e.Color) > 0.5f == normallyBright)
                self.color = inverseColor;
            else
                self.color = normalColor;
        }
    }
}