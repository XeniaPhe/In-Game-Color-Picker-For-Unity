using UnityEngine;

namespace Xenia.ColorPicker.Core.Parts.LowerPanel.PartialColorInput
{
    internal class AlphaColorComponentStrategy : ColorComponentStrategy
    {
        internal override HSVColor ChangeComponentInTheColor(float component, HSVColor color, ColorMode colorMode)
        {
            switch (colorMode)
            {
                case ColorMode.RGB0_255:
                    color.Alpha = component / 255f;
                    break;
                case ColorMode.RGB0_1:
                    color.Alpha = component;
                    break;
                case ColorMode.HSV:
                    color.Alpha = component / 100f;
                    break;
            }

            return color;
        }

        internal override float GetColorComponent(HSVColor color, ColorMode colorMode)
        {
            float component = 0;

            switch (colorMode)
            {
                case ColorMode.RGB0_255:
                    component = color.Alpha * 255f;
                    break;
                case ColorMode.RGB0_1:
                    component = color.Alpha;
                    break;
                case ColorMode.HSV:
                    component = color.Alpha * 100f;
                    break;
            }

            return component;
        }

        internal override Color GetColorIfComponentValueIs(float component, HSVColor currentColor, ColorMode colorMode)
        {
            return ChangeComponentInTheColor(component, currentColor, colorMode).ToRGB();
        }
    }
}
