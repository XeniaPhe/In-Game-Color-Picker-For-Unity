using UnityEngine;

namespace Xenia.ColorPicker.Core.Parts.LowerPanel.PartialColorInput
{
    internal class GSColorComponentStrategy : ColorComponentStrategy
    {
        internal override float GetColorComponent(HSVColor color, ColorMode colorMode)
        {
            float component = 0;

            switch (colorMode)
            {
                case ColorMode.RGB0_255:
                    component = color.ToRGB().g * 255f;
                    break;
                case ColorMode.RGB0_1:
                    component = color.ToRGB().g;
                    break;
                case ColorMode.HSV:
                    component = color.S * 100f;
                    break;
            }

            return component;
        }

        internal override Color GetColorIfComponentValueIs(float component, HSVColor currentColor, ColorMode colorMode)
        {
            if (colorMode == ColorMode.HSV && currentColor.V < 0.20f)
                currentColor.V = 0.20f;

            return base.GetColorIfComponentValueIs(component, currentColor, colorMode);
        }

        internal override HSVColor ChangeComponentInTheColor(float component, HSVColor color, ColorMode colorMode)
        {
            switch (colorMode)
            {
                case ColorMode.RGB0_255:
                    Color rgbColor = color.ToRGB();
                    rgbColor.g = component / 255f;
                    color = HSVColor.FromRGB(rgbColor);
                    break;
                case ColorMode.RGB0_1:
                    rgbColor = color.ToRGB();
                    rgbColor.g = component;
                    color = HSVColor.FromRGB(rgbColor);
                    break;
                case ColorMode.HSV:
                    color.S = component / 100f;
                    break;
            }

            return color;
        }
    }
}
