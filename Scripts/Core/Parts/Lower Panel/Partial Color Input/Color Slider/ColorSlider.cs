using UnityEngine;
using UnityEngine.UI;
using Xenia.ColorPicker.Utility;
using UnityEngine.EventSystems;

namespace Xenia.ColorPicker.Core.Parts.LowerPanel.PartialColorInput
{
    internal class ColorSlider : PartialColorInputTransmitter, IPointerClickHandler
    {
        [DisableInInspector][SerializeField] int textureSize;

        private RectTransform self;
        private Image image;
        private ColorSliderHandle handle;

        protected override void Awake()
        {
            base.Awake();

            self = GetComponent<RectTransform>();
            image = GetComponent<Image>();
            handle = GetComponentInChildren<ColorSliderHandle>();

            textureSize = 255;

            handle.Sliding += OnSlide;
        }

        internal void Initialize(int textureSize)
        {
            this.textureSize = textureSize;
        }

        private void OnSlide(object sender, SlidingEventArgs e)
        {
            float sliderValue = e.Value;

            componentValue = sliderValue * componentUpperBoundary;

            if (RoundIfWholeValues())
                MoveTheHandle();

            UpdateColorPicker();
        }

        protected override void ColorModeChanged(object sender, ColorModeChangedEventArgs e)
        {
            base.ColorModeChanged(sender, e);
            MoveTheHandle();
            RepaintTexture();
        }

        public override void ColorChanged(object sender, ColorChangedEventArgs e)
        {
            base.ColorChanged(sender, e);
            MoveTheHandle();
            RepaintTexture();
        }

        private void MoveTheHandle()
        {
            handle.MoveTo(componentValue / componentUpperBoundary);
        }

        private bool RoundIfWholeValues()
        {
            if (wholeValues)
                componentValue = Mathf.RoundToInt(componentValue);

            return wholeValues;
        }

        private float RoundIfWholeValues(float hypothethicalValue)
        {
            if (wholeValues)
                hypothethicalValue = Mathf.RoundToInt(hypothethicalValue);

            return hypothethicalValue;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Vector2 clickPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(self, eventData.position, null, out clickPos);

            componentValue = (clickPos.x / self.rect.width + 0.5f) * componentUpperBoundary;
            RoundIfWholeValues();
            MoveTheHandle();
            UpdateColorPicker();
        }

        private void RepaintTexture()
        {
            Texture2D texture = new Texture2D(textureSize, 1, TextureFormat.ARGB32, true);
            texture.anisoLevel = 16;
            texture.filterMode = FilterMode.Trilinear;
            texture.wrapMode = TextureWrapMode.Clamp;

            Color[] pixelsHorizontal = new Color[textureSize];

            float value = 0f;
            float increment = 1f / (textureSize - 1);

            for (int i = 0; i < textureSize; i++, value += increment)
                texture.SetPixel(i, 1, GetColorIfComponentValueIs(RoundIfWholeValues(value * componentUpperBoundary)));

            texture.Apply();
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
            image.sprite = sprite;
        }
    }
}