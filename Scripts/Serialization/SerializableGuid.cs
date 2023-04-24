using System;
using UnityEngine;

namespace Xenia.ColorPicker.Serialization
{
    [Serializable]
    internal struct SerializableGuid : IEquatable<SerializableGuid>
    {
        [SerializeField] private long part1;
        [SerializeField] private long part2;

        internal static readonly SerializableGuid Empty;

        static SerializableGuid()
        {
            Empty = Guid.Empty;
        }

        internal SerializableGuid(Guid guid)
        {
            byte[] bytes = guid.ToByteArray();

            part1 = 0;
            part2 = 0;

            for (int i = 0; i < 8; i++)
            {
                part1 |= (long)bytes[i] << (i << 3);
            }

            for (int i = 0; i < 8; i++)
            {
                part2 |= (long)bytes[i + 8] << (i << 3);
            }

            part1 = Math.Abs(part1);
            part2 = Math.Abs(part2);
        }

        public bool Equals(SerializableGuid other)
        {
            return this == other;
        }


        public override bool Equals(object obj)
        {
            if (obj is null || obj is not SerializableGuid)
                return false;

            return (SerializableGuid)obj == this;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return part1.GetHashCode() * 2 + part2.GetHashCode() * 3;
            }
        }

        public override string ToString()
        {
            return string.Concat(part1.ToString("x16"),part2.ToString("x16"));
        }


        public static implicit operator SerializableGuid(Guid guid)
        {
            return new SerializableGuid(guid);
        }

        public static bool operator ==(SerializableGuid left, SerializableGuid right)
        {
            return left.part1 == right.part1 && left.part2 == right.part2;
        }

        public static bool operator !=(SerializableGuid left, SerializableGuid right)
        {
            return left.part1 != right.part1 || left.part2 != right.part2;
        }
    }
}
