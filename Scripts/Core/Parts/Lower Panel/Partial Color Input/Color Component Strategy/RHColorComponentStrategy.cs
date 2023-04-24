using UnityEngine;

namespace Xenia.ColorPicker.Core.Parts.LowerPanel.PartialColorInput
{
    internal class RHColorComponentStrategy : ColorComponentStrategy
    {
        internal override float GetColorComponent(HSVColor color, ColorMode colorMode)
        {
            float component = 0;

            switch (colorMode)
            {
                case ColorMode.RGB0_255:
                    component = color.ToRGB().r * 255f;
                    break;
                case ColorMode.RGB0_1:
                    component = color.ToRGB().r;
                    break;
                case ColorMode.HSV:
                    component = color.H * 360f;
                    break;
            }

            return component;
        }

        internal override Color GetColorIfComponentValueIs(float component, HSVColor currentColor, ColorMode colorMode)
        {
            if (colorMode == ColorMode.HSV)
                return new HSVColor(component / 360f, 1f, 1f, 1f).ToRGB();

            return base.GetColorIfComponentValueIs(component, currentColor, colorMode);
        }

        internal override HSVColor ChangeComponentInTheColor(float component, HSVColor color, ColorMode colorMode)
        {
            switch (colorMode)
            {
                case ColorMode.RGB0_255:
                    Color rgbColor = color.ToRGB();
                    rgbColor.r = component / 255f;
                    color = HSVColor.FromRGB(rgbColor);
                    break;
                case ColorMode.RGB0_1:
                    rgbColor = color.ToRGB();
                    rgbColor.r = component;
                    color = HSVColor.FromRGB(rgbColor);
                    break;
                case ColorMode.HSV:
                    color.H = component / 360f;
                    break;
            }

            return color;
        }

        internal override float GetComponentUpperBoundary(ColorMode colorMode)
        {
            if (colorMode != ColorMode.HSV)
                return base.GetComponentUpperBoundary(colorMode);

            return 360f;
        }
    }
}
