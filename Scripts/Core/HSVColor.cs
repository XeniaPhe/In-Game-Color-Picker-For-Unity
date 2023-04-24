using System;
using UnityEngine;


namespace Xenia.ColorPicker.Core
{
    internal struct HSVColor : IEquatable<HSVColor>
    {
        private float h, s, v, a;

        internal float H { get => h; set => AssignIfValid(ref h, value); }
        internal float S { get => s; set => AssignIfValid(ref s, value); }
        internal float V { get => v; set => AssignIfValid(ref v, value); }
        internal float Alpha { get => a; set => AssignIfValid(ref a, value); }

        internal HSVColor(float h, float s, float v, float a = 1f)
        {
            this.h = h < 0f || h > 1f ? 0f : h;
            this.s = s < 0f || s > 1f ? 0f : s;
            this.v = v < 0f || v > 1f ? 0f : v;
            this.a = a < 0f || a > 1f ? 0f : a;
        }

        private void AssignIfValid(ref float component, float value)
        {
            if (value < 0f || value > 1f)
                return;

            component = value;
        }

        internal HSVColor Inverse()
        {
            float h = (this.h + 0.5f) % 1f;
            float s = 1f - this.s;
            float v = 1f - this.v;

            return new HSVColor(h, s, v, a);
        }

        internal Color ToRGB()
        {
            Color color = Color.HSVToRGB(h, s, v);
            color.a = a;
            return color;
        }

        internal static HSVColor FromRGB(Color color)
        {
            float h, s, v;
            Color.RGBToHSV(color, out h, out s, out v);
            return new HSVColor(h, s, v, color.a);
        }

        public override string ToString()
        {
            return string.Concat("H : ", h.ToString("f2"), ", S : ", s.ToString("f2"), ", V : ", v.ToString("f2"), ", A : ", a.ToString("f2"));
        }

        public override bool Equals(object obj)
        {
            if (obj is not HSVColor)
                return false;

            return Equals((HSVColor)obj);
        }

        public bool Equals(HSVColor other)
        {
            return h.Equals(other.h) && s.Equals(other.s) && v.Equals(other.v) && a.Equals(other.a);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return h.GetHashCode() + s.GetHashCode() + v.GetHashCode() + a.GetHashCode();
            }
        }

        public static implicit operator HSVColor(Color color)
        {
            return FromRGB(color);
        }

        public static implicit operator HSVColor(Color32 color)
        {
            return FromRGB(color);
        }

        public static implicit operator Color(HSVColor color)
        {
            return color.ToRGB();
        }

        public static implicit operator Color32(HSVColor color)
        {
            return color.ToRGB();
        }

        public static bool operator ==(HSVColor color1, HSVColor color2)
        {
            return color1.Equals(color2);
        }

        public static bool operator !=(HSVColor color1, HSVColor color2)
        {
            return !color1.Equals(color2);
        }
    }
}
