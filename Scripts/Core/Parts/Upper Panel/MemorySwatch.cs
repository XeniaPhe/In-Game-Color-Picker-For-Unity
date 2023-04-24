using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Xenia.ColorPicker.Core.Parts.UpperPanel
{
    internal class MemorySwatch : MonoBehaviour, IColorChangeSubscriber, IPointerClickHandler
    {
        private enum SwatchMode
        {
            OriginalColor,
            UpdatedColor
        }

        [SerializeField] ColorPicker colorPicker;
        [SerializeField] SwatchMode swatchMode;

        private Image swatch;

        private void Awake()
        {
            swatch = GetComponent<Image>();
            if (swatchMode == SwatchMode.UpdatedColor)
                colorPicker.ColorChangedInternal += ColorChanged;
        }

        private void Start()
        {
            if (swatchMode == SwatchMode.OriginalColor)
                swatch.color = colorPicker.CurrentColor;
        }

        public void ColorChanged(object sender, ColorChangedEventArgs e)
        {
            swatch.color = e.Color.ToRGB();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (swatchMode == SwatchMode.OriginalColor)
                colorPicker.CurrentColorHSV = swatch.color;
        }
    }
}
