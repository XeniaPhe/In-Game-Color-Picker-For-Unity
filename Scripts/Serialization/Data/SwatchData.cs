using System;
using Xenia.ColorPicker.Core.Parts.LowerPanel.Swatches;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using ColorUtility = Xenia.ColorPicker.Utility.ColorUtility;

namespace Xenia.ColorPicker.Serialization
{
    [Serializable]
    internal class SwatchData : ISerializable
    {
        private SwatchLayout layout;
        private bool swatchPanelOn;
        private int[] swatches;

        internal SwatchLayout Layout => layout;
        internal bool SwatchPanelOn => swatchPanelOn;
        internal IEnumerable<Color> Swatches => swatches.Select(s => ColorUtility.DecompressColor(s));


        [NonSerialized]
        private string reference;

        public string NonVolatileReference => reference;

        internal SwatchData(SwatchLayout layout, bool swatchPanelOn, IEnumerable<Color> swatches, string reference)
        {
            this.layout= layout;
            this.swatchPanelOn = swatchPanelOn;
            this.reference = reference;
            this.swatches = swatches.Select(s => ColorUtility.CompressColor(s)).ToArray();
        }
    }
}
