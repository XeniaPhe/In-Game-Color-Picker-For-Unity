using UnityEngine;

namespace Xenia.ColorPicker.Core.Parts.LowerPanel.PartialColorInput
{
    internal abstract class ColorComponentStrategy
    {
        internal abstract float GetColorComponent(HSVColor color, ColorMode colorMode);

        internal abstract HSVColor ChangeComponentInTheColor(float component, HSVColor currentColor, ColorMode colorMode);

        internal virtual Color GetColorIfComponentValueIs(float component, HSVColor currentColor, ColorMode colorMode)
        {
            HSVColor color = ChangeComponentInTheColor(component, currentColor, colorMode);
            color.Alpha = 1f;
            return color.ToRGB();
        }

        internal virtual float GetComponentUpperBoundary(ColorMode colorMode)
        {
            float boundary = 0f;

            switch (colorMode)
            {
                case ColorMode.RGB0_255:
                    boundary = 255f;
                    break;
                case ColorMode.RGB0_1:
                    boundary = 1f;
                    break;
                case ColorMode.HSV:
                    boundary = 100f;
                    break;
            }

            return boundary;
        }
    }
}
