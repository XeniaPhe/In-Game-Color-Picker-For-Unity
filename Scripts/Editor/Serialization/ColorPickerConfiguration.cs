using UnityEngine;
using Xenia.ColorPicker.Core.Data;
using Xenia.ColorPicker.Utility;

namespace Xenia.ColorPicker.EditorScripts.Serialization
{
    internal class ColorPickerConfiguration : ScriptableObject
    {
        [DisableInInspector][SerializeField] internal byte[] hueDrawerPNG;
        [DisableInInspector][SerializeField] internal byte[] hueSelectorPNG;
        [DisableInInspector][SerializeField] internal byte[] svSelectorPNG;
        [DisableInInspector][SerializeField] internal ColorPickerConfigurationData data;

        public override bool Equals(object other)
        {
            if (other.GetType() != typeof(ColorPickerConfiguration))
                return false;

            ColorPickerConfiguration otherConfig = (ColorPickerConfiguration)other;

            if (!hueDrawerPNG.Equals(otherConfig.hueDrawerPNG))
                return false;
            if (!hueSelectorPNG.Equals(otherConfig.hueSelectorPNG))
                return false;
            if (!svSelectorPNG.Equals(otherConfig.svSelectorPNG))
                return false;
            if (!data.Equals(otherConfig.data))
                return false;

            return true;
        }

        public override int GetHashCode()
        {
            return hueDrawerPNG.GetHashCode() * 37 + hueSelectorPNG.GetHashCode() * -41
                + svSelectorPNG.GetHashCode() * 43 + data.GetHashCode() * -47;
        }
    }
}