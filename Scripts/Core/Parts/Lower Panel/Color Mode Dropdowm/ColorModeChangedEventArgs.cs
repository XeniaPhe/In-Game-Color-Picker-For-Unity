using System;

namespace Xenia.ColorPicker.Core.Parts.LowerPanel
{
    internal class ColorModeChangedEventArgs : EventArgs
    {
        private ColorMode colorMode;
        internal ColorMode ColorMode => colorMode;
        internal ColorModeChangedEventArgs(ColorMode colorMode)
        {
            this.colorMode = colorMode;
        }
    }
}
