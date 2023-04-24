using System;
using UnityEngine;

namespace Xenia.ColorPicker.Core.Data
{
    [Serializable]
    internal class SVDrawerData : IEquatable<SVDrawerData>, ICloneable
    {
        [Min(1)][SerializeField] internal int textureSize;
        [Range(0.1f, 1f)][SerializeField] internal float drawerSize;
        [Range(0f, 1f)][SerializeField] internal float alpha;

        private readonly static SVDrawerData _default;
        internal static SVDrawerData Default => (SVDrawerData)_default.Clone();

        static SVDrawerData()
        {
            _default = new SVDrawerData();
            _default.textureSize = 256;
            _default.drawerSize = 0.95f;
            _default.alpha = 1f;
        }

        public object Clone()
        {
            return MemberwiseClone();
        }

        public bool Equals(SVDrawerData other)
        {
            if(ReferenceEquals(this, other)) 
                return true;

            if (textureSize != other.textureSize)
                return false;
            if (drawerSize != other.drawerSize)
                return false;
            if (alpha != other.alpha)
                return false;

            return true;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return textureSize.GetHashCode() * 37 + drawerSize.GetHashCode() * 41
                + alpha.GetHashCode() * 43;
            }
        }
    }
}