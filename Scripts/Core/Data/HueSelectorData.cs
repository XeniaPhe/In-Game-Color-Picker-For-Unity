using System;
using UnityEngine;

namespace Xenia.ColorPicker.Core.Data
{
    [Serializable]
    internal class HueSelectorData : IEquatable<HueSelectorData>, ICloneable
    {
        [Min(1)][SerializeField] internal int textureSize;
        [Range(0.1f, 1f)][SerializeField] internal float selectorSize;
        [Range(0f, 0.95f)][SerializeField] internal float innerCircleSize;
        [SerializeField] internal Color color;

        private readonly static HueSelectorData _default;
        internal static HueSelectorData Default => (HueSelectorData)_default.Clone();

        static HueSelectorData()
        {
            _default = new HueSelectorData();
            _default.textureSize = 256;
            _default.selectorSize = 1f;
            _default.innerCircleSize = 0.82f;
            _default.color = Color.white;
        }

        public object Clone()
        {
            return MemberwiseClone();
        }

        public bool Equals(HueSelectorData other)
        {
            if(ReferenceEquals(this,other))
                return true;

            if (textureSize != other.textureSize)
                return false;
            if (selectorSize != other.selectorSize)
                return false;
            if (innerCircleSize != other.innerCircleSize)
                return false;
            if (color != other.color)
                return false;

            return true;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return textureSize.GetHashCode() * 19 + selectorSize.GetHashCode() * 23
                + innerCircleSize.GetHashCode() * 29 + color.GetHashCode() * 31;
            }
        }
    }
}