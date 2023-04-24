using System;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Xenia.ColorPicker.Core.Data
{
    [Serializable]
    internal class OtherData : IEquatable<OtherData>, ICloneable
    {
        [SerializeField] internal bool includeLowerPanel;
        [SerializeField] internal Color lowerPanelColor;

        [SerializeField] internal bool includeSwatches;
        [SerializeField] internal bool includeHexadecimalInput;
        [SerializeField] internal Color hexadecimalLabelColor;
        [SerializeField] internal bool includeAlphaInput;
        [SerializeField] internal Color alphaLabelColor;

        [SerializeField] internal bool includeColorInput;
        [SerializeField] internal bool includeInputFields;
        [SerializeField] internal int colorSliderTextureSize;

        [SerializeField] internal bool includeRGB0_255;
        [SerializeField] internal bool includeRGB0_1;

        [SerializeField] internal Color rLabelColor;
        [SerializeField] internal Color gLabelColor;
        [SerializeField] internal Color bLabelColor;

        [SerializeField] internal bool includeHSV;

        [SerializeField] internal Color hLabelColor;
        [SerializeField] internal Color sLabelColor;
        [SerializeField] internal Color vLabelColor;

        [SerializeField] internal bool includeUpperPanel;
        [SerializeField] internal Color upperPanelColor;
        [SerializeField] internal bool includeColorPicker;
        [SerializeField] internal Color pixelatedDisplayBackgroundColor;
        [SerializeField] internal bool includeMemorySwatches;

        [SerializeField] internal bool includeTitlePanel;
        [SerializeField] internal Color titlePanelColor;

        [SerializeField] internal bool includeTitle;
        [SerializeField] internal string title;
        [SerializeField] internal bool includeCloseButton;

        internal bool IncludeLowerPanel
        {
            get
            {
                return includeLowerPanel &&
                    (includeSwatches || includeHexadecimalInput
                    || includeAlphaInput || includeColorInput);
            }
        }

        internal bool IncludeUpperPanel
        {
            get
            {
                return includeUpperPanel &&
                    (includeColorPicker || includeMemorySwatches);
            }
        }

        internal bool IncludeTitlePanel
        {
            get
            {
                return includeTitlePanel &&
                    (includeTitle || includeCloseButton);
            }
        }

        private readonly static OtherData _default;
        internal static OtherData Default => (OtherData)_default.Clone();

        static OtherData()
        {
            _default = new OtherData();
            Color defaultColor = new Color32(60, 60, 60, 255);
            Color labelColor = new Color32(167, 167, 167, 255);

            //get fields
            var properties = typeof(OtherData).Assembly
                .GetType("Xenia.ColorPicker.Core.Data.OtherData")
                .GetFields(BindingFlags.Instance | BindingFlags.NonPublic);

            //set all bool fields to true(coz there's lots of 'em)
            properties.Where(p => p.FieldType == typeof(bool)).ToList()
                .ForEach(p => p.SetValue(_default, true));

            //get all color fields
            var colorProps = properties.Where(p => p.FieldType == typeof(Color));
            var labelColorProps = colorProps.Where(p => p.Name.Contains("Label")).ToList();
            var otherColorProps = colorProps.Except(labelColorProps).ToList();

            //set all color fields
            labelColorProps.ForEach(p => p.SetValue(_default, labelColor));
            otherColorProps.ForEach(p => p.SetValue(_default, defaultColor));

            //set the rest of the fields
            _default.colorSliderTextureSize = 256;
            _default.titlePanelColor = Color.white;
            _default.title = "Color Picker";
        }

        public object Clone()
        {
            return MemberwiseClone();
        }

        public bool Equals(OtherData other)
        {
            if (ReferenceEquals(this, other))
                return true;

            if (!LowerPanelSettingsEquals(other))
                return false;

            if (!UpperPanelSettingsEquals(other))
                return false;

            if (!TitlePanelSettingsEquals(other))
                return false;

            return true;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = includeLowerPanel.GetHashCode() * 67;

                if (includeLowerPanel)
                {
                    hashCode += lowerPanelColor.GetHashCode() * 71;
                    hashCode += includeSwatches.GetHashCode() * 73;
                    hashCode += includeHexadecimalInput.GetHashCode() * 79;

                    if (includeHexadecimalInput)
                        hashCode += hexadecimalLabelColor.GetHashCode() * 83;

                    hashCode += includeAlphaInput.GetHashCode() * 89;

                    if (includeAlphaInput)
                        hashCode += alphaLabelColor.GetHashCode() * 97;

                    hashCode += includeColorInput.GetHashCode() * 101;

                    if (includeColorInput)
                    {
                        hashCode += includeRGB0_255.GetHashCode() * 103;
                        hashCode += includeRGB0_1.GetHashCode() * 107;

                        if (includeRGB0_255 || includeRGB0_1)
                        {
                            hashCode += rLabelColor.GetHashCode() * 109;
                            hashCode += gLabelColor.GetHashCode() * 113;
                            hashCode += bLabelColor.GetHashCode() * 127;
                        }

                        hashCode += includeHSV.GetHashCode() * 131;

                        if (includeHSV)
                        {
                            hashCode += hLabelColor.GetHashCode() * 137;
                            hashCode += sLabelColor.GetHashCode() * 139;
                            hashCode += vLabelColor.GetHashCode() * 149;
                        }
                    }

                    if (includeColorInput || includeAlphaInput)
                    {
                        hashCode += includeInputFields.GetHashCode() * 151;
                        hashCode += colorSliderTextureSize.GetHashCode() * 157;
                    }
                }

                hashCode += includeUpperPanel.GetHashCode() * 163;

                if (includeUpperPanel)
                {
                    hashCode += upperPanelColor.GetHashCode() * 167;
                    hashCode += includeColorPicker.GetHashCode() * 173;

                    if (includeColorPicker)
                        hashCode += pixelatedDisplayBackgroundColor.GetHashCode() * 179;

                    hashCode += includeMemorySwatches.GetHashCode() * 181;
                }

                hashCode += includeTitlePanel.GetHashCode() * 191;

                if (includeTitlePanel)
                {
                    hashCode += titlePanelColor.GetHashCode() * 193;
                    hashCode += includeTitle.GetHashCode() * 197;

                    if (includeTitle)
                        hashCode += title.GetHashCode() * 199;

                    hashCode += includeCloseButton.GetHashCode() * 211;
                }

                return hashCode;
            }
        }

        internal bool LowerPanelSettingsEquals(OtherData other)
        {
            if (IncludeLowerPanel != other.IncludeLowerPanel)
                return false;

            if (includeLowerPanel)
            {
                if (lowerPanelColor != other.lowerPanelColor)
                    return false;
                if (includeSwatches != other.includeSwatches)
                    return false;

                if (includeHexadecimalInput != other.includeHexadecimalInput)
                    return false;
                if (includeHexadecimalInput && (hexadecimalLabelColor != other.hexadecimalLabelColor))
                    return false;

                if (includeAlphaInput != other.includeAlphaInput)
                    return false;
                if (includeAlphaInput && (alphaLabelColor != other.alphaLabelColor))
                    return false;

                if (includeColorInput != other.includeColorInput)
                    return false;

                if (includeColorInput)
                {
                    if (includeRGB0_255 != other.includeRGB0_255)
                        return false;
                    if (includeRGB0_1 != other.includeRGB0_1)
                        return false;

                    if (includeRGB0_1 || includeRGB0_255)
                    {
                        if (rLabelColor != other.rLabelColor)
                            return false;
                        if (gLabelColor != other.gLabelColor)
                            return false;
                        if (bLabelColor != other.bLabelColor)
                            return false;
                    }

                    if (includeHSV != other.includeHSV)
                        return false;

                    if (includeHSV)
                    {
                        if (hLabelColor != other.hLabelColor)
                            return false;
                        if (sLabelColor != other.sLabelColor)
                            return false;
                        if (vLabelColor != other.vLabelColor)
                            return false;
                    }
                }

                if (includeColorInput || includeAlphaInput)
                {
                    if (includeInputFields != other.includeInputFields)
                        return false;
                    if (colorSliderTextureSize != other.colorSliderTextureSize)
                        return false;
                }
            }

            return true;
        }

        internal bool UpperPanelSettingsEquals(OtherData other)
        {
            if (IncludeUpperPanel != other.IncludeUpperPanel)
                return false;

            if (includeUpperPanel)
            {
                if (upperPanelColor != other.upperPanelColor)
                    return false;

                if (includeColorPicker != other.includeColorPicker)
                    return false;
                if (includeColorPicker && (pixelatedDisplayBackgroundColor != other.pixelatedDisplayBackgroundColor))
                    return false;

                if (includeMemorySwatches != other.includeMemorySwatches)
                    return false;
            }

            return true;
        }

        internal bool TitlePanelSettingsEquals(OtherData other)
        {
            if (IncludeTitlePanel != other.IncludeTitlePanel)
                return false;

            if (includeTitlePanel)
            {
                if (titlePanelColor != other.titlePanelColor)
                    return false;

                if (includeTitle != other.includeTitle)
                    return false;
                if (includeTitle && !title.Equals(other.title))
                    return false;

                if (includeCloseButton != other.includeCloseButton)
                    return false;
            }

            return true;
        }
    }
}
