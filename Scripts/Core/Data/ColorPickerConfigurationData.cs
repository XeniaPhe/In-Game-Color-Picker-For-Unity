using System;
using UnityEngine;

namespace Xenia.ColorPicker.Core.Data
{
    [Serializable]
    internal class ColorPickerConfigurationData : IEquatable<ColorPickerConfigurationData>, ICloneable
    {
        [SerializeField] internal HueDrawerData hueDrawerData;
        [SerializeField] internal HueSelectorData hueSelectorData;
        [SerializeField] internal SVDrawerData svDrawerData;
        [SerializeField] internal SVSelectorData svSelectorData;
        [SerializeField] internal OtherData otherData;

        private readonly static ColorPickerConfigurationData _default;
        internal static ColorPickerConfigurationData Default => (ColorPickerConfigurationData)_default.Clone();

        static ColorPickerConfigurationData()
        {
            _default = new ColorPickerConfigurationData();
            _default.hueDrawerData = HueDrawerData.Default;
            _default.hueSelectorData = HueSelectorData.Default;
            _default.svDrawerData = SVDrawerData.Default;
            _default.svSelectorData = SVSelectorData.Default;
            _default.otherData = OtherData.Default;
        }

        public object Clone()
        {
            ColorPickerConfigurationData clone = new ColorPickerConfigurationData();

            clone.hueDrawerData = (HueDrawerData)hueDrawerData.Clone();
            clone.hueSelectorData = (HueSelectorData)hueSelectorData.Clone();
            clone.svDrawerData = (SVDrawerData)svDrawerData.Clone();
            clone.svSelectorData = (SVSelectorData)svSelectorData.Clone();
            clone.otherData = (OtherData)otherData.Clone();

            return clone;
        }

        public bool Equals(ColorPickerConfigurationData other)
        {
            if (ReferenceEquals(this, other))
                return true;

            if (!hueDrawerData.Equals(other.hueDrawerData))
                return false;
            if (!hueSelectorData.Equals(other.hueSelectorData))
                return false;
            if (!svDrawerData.Equals(other.svDrawerData))
                return false;
            if (!svSelectorData.Equals(other.svSelectorData))
                return false;
            if (!otherData.Equals(other.otherData))
                return false;

            return true;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return hueDrawerData.GetHashCode() * 193
                + hueSelectorData.GetHashCode() * 197
                + svDrawerData.GetHashCode() * 199
                + svSelectorData.GetHashCode() * 211;
            }
        }
    }
}