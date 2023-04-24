using System;
using Xenia.ColorPicker.Core.Parts.LowerPanel;

namespace Xenia.ColorPicker.Serialization
{
    [Serializable]
    internal struct ColorModeData : ISerializable
    {
        private ColorMode colorMode;
        internal ColorMode ColorMode => colorMode;

        [NonSerialized]
        private string reference;

        public string NonVolatileReference => reference;

        internal ColorModeData(ColorMode colorMode, string reference)
        {
            this.colorMode = colorMode;
            this.reference = reference;
        }
    }
}
