using System;
using UnityEngine;

namespace Xenia.ColorPicker.Core.Data
{
    [Serializable]
    internal class HueDrawerData : IEquatable<HueDrawerData>, ICloneable
    {
        [Min(1)][SerializeField] internal int textureSize;
        [Range(0.5f, 1f)][SerializeField] internal float drawerSize;
        [Range(0f, 0.95f)][SerializeField] internal float innerCircleSize;
        [Range(0f, 360f)][SerializeField] internal float rotation;
        [Range(0f, 1f)][SerializeField] internal float alpha;
        [SerializeField] internal Color outsideColor;
        [SerializeField] internal Color insideColor;

        private readonly static HueDrawerData _default;
        internal static HueDrawerData Default => (HueDrawerData)_default.Clone();

        static HueDrawerData()
        {
            _default = new HueDrawerData();
            Color defaultColor = new Color32(60, 60, 60, 255);

            _default.textureSize = 512;
            _default.drawerSize = 0.95f;
            _default.innerCircleSize = 0.82f;
            _default.rotation = 0f;
            _default.alpha = 1f;
            _default.outsideColor = defaultColor;
            _default.insideColor = defaultColor;
        }

        public object Clone()
        {
            return MemberwiseClone();
        }

        public bool Equals(HueDrawerData other)
        {
            if (ReferenceEquals(this, other))
                return true;

            if (textureSize != other.textureSize)
                return false;
            if (drawerSize != other.drawerSize)
                return false;
            if (innerCircleSize != other.innerCircleSize)
                return false;
            if (rotation != other.rotation)
                return false;
            if (alpha != other.alpha)
                return false;
            if (outsideColor != other.outsideColor)
                return false;
            if (insideColor != other.insideColor)
                return false;

            return true;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return textureSize.GetHashCode() * 2
                + drawerSize.GetHashCode() * 3 + innerCircleSize.GetHashCode() * 5
                + rotation.GetHashCode() * 7 + alpha.GetHashCode() * 11
                + outsideColor.GetHashCode() * 13 + insideColor.GetHashCode() * 17;
            }
        }
    }
}