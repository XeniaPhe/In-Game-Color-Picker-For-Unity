using System;

namespace Xenia.ColorPicker.Core
{
    internal class ColorChangedEventArgs : EventArgs
    {
        private HSVColor color;

        internal HSVColor Color { get => color; }

        internal ColorChangedEventArgs(HSVColor color)
        {
            this.color = color;
        }
    }
}