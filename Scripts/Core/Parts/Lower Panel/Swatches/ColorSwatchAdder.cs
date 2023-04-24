using UnityEngine;
using UnityEngine.UI;

namespace Xenia.ColorPicker.Core.Parts.LowerPanel.Swatches
{
    internal class ColorSwatchAdder : MonoBehaviour
    {
        private ColorSwatch swatch;
        private SwatchController controller;

        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(AddSwatch);
            swatch = GetComponentInChildren<ColorSwatch>();
        }

        internal void Initialize(SwatchController controller, ColorPicker colorPicker)
        {
            this.controller = controller;
            swatch.Color = colorPicker.CurrentColor;
            colorPicker.ColorChangedInternal += OnColorChanged;
        }

        private void AddSwatch()
        {
            controller.AddSwatch(swatch.Color);
        }

        private void OnColorChanged(object sender, ColorChangedEventArgs e)
        {
            swatch.Color = e.Color;
        }
    }
}
