using System;
using UnityEngine;
using ColorUtility = Xenia.ColorPicker.Utility.ColorUtility;

namespace Xenia.ColorPicker.Serialization
{
    [Serializable]
    internal class ColorPickerData : ISerializable
    {
        private bool draggable;
        private bool useColorPreview;
        private bool closeKeyEnabled;
        private bool openKeyEnabled;
        private bool adjustSVSelectorBasedOnBrightness;

        private KeyCode closeKey;
        private KeyCode openKey;

        private int currentColor;
        private float xPos;
        private float yPos;

        internal bool Draggable => draggable;
        internal bool UseColorPreview => useColorPreview;
        internal bool AdjustSVSelectorBasedOnBrightness => adjustSVSelectorBasedOnBrightness;
        internal bool CloseKeyEnabled => closeKeyEnabled;
        internal bool OpenKeyEnabled => openKeyEnabled;
        internal KeyCode CloseKey => closeKey;
        internal KeyCode OpenKey => openKey;
        internal Color CurrentColor => ColorUtility.DecompressColor(currentColor);
        internal Vector2 Position => new Vector2(xPos, yPos);


        [NonSerialized]
        private string reference;
        public string NonVolatileReference => reference;

        internal ColorPickerData(bool draggable, bool adjustSVSelectorBasedOnBrightness, bool closeKeyEnabled, bool openKeyEnabled,
           bool useColorPreview, KeyCode closeKey, KeyCode openKey, Color currentColor, Vector2 position, string reference)
        {
            this.draggable = draggable;
            this.useColorPreview = useColorPreview;
            this.adjustSVSelectorBasedOnBrightness = adjustSVSelectorBasedOnBrightness;
            this.closeKeyEnabled = closeKeyEnabled;
            this.openKeyEnabled = openKeyEnabled;
            this.closeKey = closeKey;
            this.openKey = openKey;
            this.currentColor = ColorUtility.CompressColor(currentColor);
            xPos = position.x;
            yPos = position.y;

            this.reference = reference;
        }
    }
}
