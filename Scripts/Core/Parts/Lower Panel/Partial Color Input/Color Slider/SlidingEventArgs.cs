using System;

namespace Xenia.ColorPicker.Core.Parts.LowerPanel.PartialColorInput
{
    internal class SlidingEventArgs : EventArgs
    {
        private float value;

        public float Value => value;
        internal SlidingEventArgs(float value)
        {
            this.value = value;
        }
    }
}