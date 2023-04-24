using UnityEngine;
using Xenia.ColorPicker.Utility;
using Xenia.ColorPicker.Core.Parts.LowerPanel.PartialColorInput;
using TMPro;

namespace Xenia.ColorPicker.Core.Parts.LowerPanel
{
    internal class ColorLabel : MonoBehaviour
    {
        [SerializeField] ColorModeDropdown dropdown;
        [SerializeField] ColorComponent colorComponent;

        [DisableInInspector][SerializeField] Color rgbLabelColor;
        [DisableInInspector][SerializeField] Color hsvLabelColor;

        private TMP_Text label;

        private void Awake()
        {
            label = GetComponent<TMP_Text>();
            dropdown.ColorModeChanged += ColorModeChanged;
        }

        internal void Initialize(Color rgbLabelColor, Color hsvLabelColor)
        {
            this.rgbLabelColor = rgbLabelColor;
            this.hsvLabelColor = hsvLabelColor;
        }

        private void ColorModeChanged(object sender, ColorModeChangedEventArgs e)
        {
            switch (e.ColorMode)
            {
                case ColorMode.RGB0_255:
                case ColorMode.RGB0_1:
                    label.color = rgbLabelColor;
                    label.text = colorComponent.ToString()[0].ToString();
                    break;
                case ColorMode.HSV:
                    label.color = hsvLabelColor;
                    label.text = colorComponent.ToString()[^1].ToString();
                    break;
            }
        }
    }
}
