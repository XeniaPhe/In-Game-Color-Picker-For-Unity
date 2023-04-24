using UnityEngine;

namespace Xenia.ColorPicker.Core.Parts.LowerPanel.PartialColorInput
{
    internal class BVColorComponentStrategy : ColorComponentStrategy
    {
        internal override float GetColorComponent(HSVColor color, ColorMode colorMode)
        {
            float component = 0;

            switch (colorMode)
            {
                case ColorMode.RGB0_255:
                    component = color.ToRGB().b * 255f;
                    break;
                case ColorMode.RGB0_1:
                    component = color.ToRGB().b;
                    break;
                case ColorMode.HSV:
                    component = color.V * 100f;
                    break;
            }

            return component;
        }

        internal override HSVColor ChangeComponentInTheColor(float component, HSVColor color, ColorMode colorMode)
        {
            switch (colorMode)
            {
                case ColorMode.RGB0_255:
                    Color rgbColor = color.ToRGB();
                    rgbColor.b = component / 255f;
                    color = HSVColor.FromRGB(rgbColor);
                    break;
                case ColorMode.RGB0_1:
                    rgbColor = color.ToRGB();
                    rgbColor.b = component;
                    color = HSVColor.FromRGB(rgbColor);
                    break;
                case ColorMode.HSV:
                    color.V = component / 100f;
                    break;
            }

            return color;
        }
    }
}
