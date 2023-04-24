using System;
using UnityEngine;

namespace Xenia.ColorPicker.Core.Data
{
    [Serializable]
    internal class SVSelectorData : IEquatable<SVSelectorData>, ICloneable
    {
        [Min(1)][SerializeField] internal int textureSize;
        [Range(0.01f, 0.5f)][SerializeField] internal float selectorSize;
        [Range(0f, 1f)][SerializeField] internal float innerCircleRatio;
        [SerializeField] internal Color color;

        private readonly static SVSelectorData _default;
        internal static SVSelectorData Default => (SVSelectorData)_default.Clone();

        static SVSelectorData()
        {
            _default = new SVSelectorData();
            _default.textureSize = 256;
            _default.selectorSize = 0.1f;
            _default.innerCircleRatio = 0.82f;
            _default.color = Color.white;
        }

        public object Clone()
        {
            return MemberwiseClone();
        }

        public bool Equals(SVSelectorData other)
        {
            if (ReferenceEquals(this, other))
                return true;

            if (textureSize != other.textureSize)
                return false;
            if (selectorSize != other.selectorSize)
                return false;
            if (innerCircleRatio != other.innerCircleRatio)
                return false;
            if (color != other.color)
                return false;

            return true;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return textureSize.GetHashCode() * 47 + selectorSize.GetHashCode() * 53
                + innerCircleRatio.GetHashCode() * 59 + color.GetHashCode() * 61;
            }
        }
    }
}