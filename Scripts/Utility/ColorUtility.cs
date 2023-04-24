using UnityEngine;

namespace Xenia.ColorPicker.Utility
{
    internal static class ColorUtility
    {
        internal static int CompressColor(Color color)
        {
            Color32 color32 = color;
            int compressedColor = 0;

            compressedColor |= color32.a;
            compressedColor |= color32.b << 8;
            compressedColor |= color32.g << 16;
            compressedColor |= color32.r << 24;

            return compressedColor;
        }

        internal static Color DecompressColor(int color)
        {
            Color32 color32 = new Color32(0, 0, 0, 0);

            color32.a |= (byte)(color);
            color32.b |= (byte)(color >> 8);
            color32.g |= (byte)(color >> 16);
            color32.r |= (byte)(color >> 24);

            return color32;
        }

        internal static float Brightness(Color color)
        {
            return (color.r + color.g + color.b) / 3f;
        }
    }
}
